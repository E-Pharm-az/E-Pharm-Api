using AutoMapper;
using EPharm.Domain.Dtos.EmailDto;
using EPharm.Domain.Dtos.PasswordChangeDto;
using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using EPharm.Infrastructure.Context.Entities.Identity;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EPharm.Domain.Services.CommonServices;

public class UserService(
    UserManager<AppIdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IPharmaCompanyManagerService pharmaCompanyManagerService,
    IPharmaCompanyService pharmaCompanyService,
    IEmailSender emailSender,
    IEmailService emailService,
    IUnitOfWork unitOfWork,
    IConfiguration configuration,
    IMapper mapper) : IUserService
{
    public async Task<IEnumerable<GetUserDto>> GetAllUsersAsync()
    {
        var users = await userManager.Users.ToListAsync();
        return mapper.Map<IEnumerable<GetUserDto>>(users);
    }

    public async Task<GetUserDto?> GetUserByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        return mapper.Map<GetUserDto>(user);
    }

    public async Task<GetUserDto> CreateCustomerAsync(CreateUserDto createUserDto)
    {
        var existingUser = await userManager.FindByEmailAsync(createUserDto.Email);
        if (existingUser is not null)
            throw new InvalidOperationException("User with this email already exists.");

        var user = await CreateUserAsync(createUserDto, [IdentityData.Customer], configuration["AppUrls:EpharmClient"]!);
        return user;
    }

    public async Task<GetPharmaCompanyManagerDto> CreatePharmaManagerAsync(int pharmaCompanyId, CreateUserDto createUserDto)
    {
        var user = await CreateUserAsync(createUserDto, [IdentityData.PharmaCompanyManager],
            configuration["AppUrls:PharmaPortalClient"]!);

        var pharmaCompanyManagerEntity = mapper.Map<CreatePharmaCompanyManagerDto>(user);
        pharmaCompanyManagerEntity.ExternalId = user.Id;
        pharmaCompanyManagerEntity.PharmaCompanyId = pharmaCompanyId;

        var result = await pharmaCompanyManagerService.CreatePharmaCompanyManagerAsync(pharmaCompanyManagerEntity);
        return result;
    }

    public async Task<GetPharmaCompanyManagerDto> CreatePharmaAdminAsync(CreateUserDto createUserDto, CreatePharmaCompanyDto createPharmaCompanyDto)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync();
            var user = await CreateUserAsync(createUserDto, [IdentityData.PharmaCompanyAdmin, IdentityData.PharmaCompanyManager], configuration["AppUrls:PharmaPortalClient"]!);

            var pharmaCompany = await pharmaCompanyService.CreatePharmaCompanyAsync(createPharmaCompanyDto, user.Id);

            var pharmaCompanyAdminEntity = mapper.Map<CreatePharmaCompanyManagerDto>(user);
            pharmaCompanyAdminEntity.ExternalId = user.Id;
            pharmaCompanyAdminEntity.PharmaCompanyId = pharmaCompany.Id;

            var result = await pharmaCompanyManagerService.CreatePharmaCompanyManagerAsync(pharmaCompanyAdminEntity);
            await unitOfWork.CommitTransactionAsync();

            return result;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<GetUserDto> CreateAdminAsync(CreateUserDto createUserDto)
    {
        var user = await CreateUserAsync(createUserDto, [IdentityData.Admin], configuration["AppConfig:AdminPortalClient"]!);
        return user;
    }

    public async Task<bool> UpdateUserAsync(string id, CreateUserDto createUserDto)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user is null)
            return false;

        mapper.Map(createUserDto, user);

        var result = await userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return false;

        var result = await userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    public async Task InitiatePasswordChange(InitiatePasswordChangeRequest passwordChangeRequest, string url)
    {
        var user = await userManager.FindByEmailAsync(passwordChangeRequest.Email);
        ArgumentNullException.ThrowIfNull(user);

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(token));
        
        var emailTemplate = emailService.GetEmail("change-password");
        ArgumentNullException.ThrowIfNull(emailTemplate);
        
        emailTemplate = emailTemplate.Replace("{url}", $"{url}/change-password?userId={user.Id}&token={encodedToken}");

        await emailSender.SendEmailAsync(new CreateEmailDto
        {
            Email = user.Email!,
            Subject = "Confirm your account",
            Message = emailTemplate
        });
    }
    
    public async Task ChangePassword(ChangePasswordWithTokenRequest passwordWithTokenRequest)
    {
        var user = await userManager.FindByIdAsync(passwordWithTokenRequest.UserId);
        ArgumentNullException.ThrowIfNull(user);

        var decodedToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(passwordWithTokenRequest.Token));
        await userManager.ResetPasswordAsync(user, decodedToken, passwordWithTokenRequest.NewPassword);
    } 

    private async Task<GetUserDto> CreateUserAsync(CreateUserDto createUserDto, string[] identityRole, string url)
    {
        var userEntity = mapper.Map<AppIdentityUser>(createUserDto);
        userEntity.UserName = createUserDto.Email;

        if (identityRole.Any(role => role != IdentityData.Customer))
            userEntity.EmailConfirmed = true;

        var result = await userManager.CreateAsync(userEntity, createUserDto.Password);

        foreach (var role in identityRole)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        if (!result.Succeeded)
            throw new InvalidOperationException($"Failed to create user. Details: {string.Join("; ", result.Errors.Select(e => e.Description))}");

        foreach (var role in identityRole)
            await userManager.AddToRoleAsync(userEntity, role);

        var token = await userManager.GenerateEmailConfirmationTokenAsync(userEntity);
        var encodedToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(token));

        var emailTemplate = emailService.GetEmail("confirmation-email");
        ArgumentNullException.ThrowIfNull(emailTemplate);
        
        emailTemplate = emailTemplate.Replace("{url}", $"{url}/confirm-email?userId={userEntity.Id}&token={encodedToken}");

        await emailSender.SendEmailAsync(new CreateEmailDto
        {
            Email = userEntity.Email,
            Subject = "Confirm your account",
            Message = emailTemplate
        });

        return mapper.Map<GetUserDto>(userEntity);
    }
}
