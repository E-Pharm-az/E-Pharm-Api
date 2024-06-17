using AutoMapper;
using EPharm.Domain.Dtos.AuthDto;
using EPharm.Domain.Dtos.EmailDto;
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
    IPasswordHasher<AppIdentityUser> passwordHasher,
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

    public async Task<GetUserDto> CreateCustomerAsync(RegisterUserDto registerUserDto) =>
        await CreateUserAsync(registerUserDto, [IdentityData.Customer]);

    public async Task InitializeUserAsync(InitializeUserDto initializeUserDto)
    {
        var user = await userManager.FindByEmailAsync(initializeUserDto.Email);
        if (user is null)
            throw new InvalidOperationException("User with this email does not exist.");

        if (user.Code != initializeUserDto.Code)
            throw new InvalidOperationException("The code provided is not valid.");

        mapper.Map(initializeUserDto, user);

        user.PasswordHash = passwordHasher.HashPassword(user, initializeUserDto.Password);

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                $"Failed to update user. Details: {string.Join("; ", result.Errors.Select(e => e.Description))}");
    }

    public async Task<GetPharmaCompanyManagerDto> CreatePharmaManagerAsync(int pharmaCompanyId,
        RegisterUserDto registerUserDto)
    {
        var user = await CreateUserAsync(registerUserDto, [IdentityData.PharmaCompanyManager]);

        var pharmaCompanyManagerEntity = mapper.Map<CreatePharmaCompanyManagerDto>(user);
        pharmaCompanyManagerEntity.ExternalId = user.Id;
        pharmaCompanyManagerEntity.PharmaCompanyId = pharmaCompanyId;

        return await pharmaCompanyManagerService.CreatePharmaCompanyManagerAsync(pharmaCompanyManagerEntity);
    }

    public async Task<GetPharmaCompanyManagerDto> CreatePharmaAdminAsync(RegisterUserDto registerUserDto,
        CreatePharmaCompanyDto createPharmaCompanyDto)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync();
            var user = await CreateUserAsync(registerUserDto,
                [IdentityData.PharmaCompanyAdmin, IdentityData.PharmaCompanyManager]);

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

    public async Task<GetUserDto> CreateAdminAsync(RegisterUserDto registerUserDto)
    {
        var user = await CreateUserAsync(registerUserDto, [IdentityData.Admin]);
        return user;
    }

    public async Task<bool> UpdateUserAsync(string id, RegisterUserDto registerUserDto)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user is null)
            return false;

        mapper.Map(registerUserDto, user);

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

        var code = RandomCodeGenerator.GenerateCode();
        user.Code = code;
        user.CodeExpiryTime = DateTime.UtcNow.AddHours(1);
        
        await userManager.UpdateAsync(user);

        var emailTemplate = emailService.GetEmail("change-password");
        if (emailTemplate is null)
            throw new KeyNotFoundException("The email template for 'change-password' was not found.");

        emailTemplate = emailTemplate.Replace("{code}", code.ToString());

        await emailSender.SendEmailAsync(new CreateEmailDto
        {
            Email = user.Email!,
            Subject = "Change your password",
            Message = emailTemplate
        });
    }

    public async Task ChangePassword(ChangePasswordWithTokenRequest passwordWithTokenRequest)
    {
        var user = await userManager.FindByEmailAsync(passwordWithTokenRequest.Email);
        if (user == null)
            throw new ArgumentException("Invalid email address.");

        if (user.Code == passwordWithTokenRequest.Code && user.CodeExpiryTime > DateTime.UtcNow)
        {
            user.PasswordHash = passwordHasher.HashPassword(user, passwordWithTokenRequest.Password);
            await userManager.UpdateAsync(user);
        }
        else
            throw new ArgumentException("Invalid or expired code. Please generate a new one.");
    }

    private async Task<GetUserDto> CreateUserAsync(RegisterUserDto registerUserDto, string[] identityRole)
    {
        var existingUser = await userManager.FindByEmailAsync(registerUserDto.Email);
        if (existingUser is not null)
        {
            if (existingUser.EmailConfirmed)
                throw new InvalidOperationException("User with this email already exists.");

            await SendEmailConfirmationAsync(existingUser);
            return mapper.Map<GetUserDto>(existingUser);
        }

        var userEntity = mapper.Map<AppIdentityUser>(registerUserDto);
        userEntity.UserName = registerUserDto.Email;

        if (identityRole.Any(role => role != IdentityData.Customer))
            userEntity.EmailConfirmed = true;

        var result = await userManager.CreateAsync(userEntity, configuration["UniqueKey"]!);

        foreach (var role in identityRole)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        if (!result.Succeeded)
            throw new InvalidOperationException(
                $"Failed to create user. Details: {string.Join("; ", result.Errors.Select(e => e.Description))}");

        foreach (var role in identityRole)
            await userManager.AddToRoleAsync(userEntity, role);

        await SendEmailConfirmationAsync(userEntity);

        return mapper.Map<GetUserDto>(userEntity);
    }

    public async Task SendEmailConfirmationAsync(AppIdentityUser user)
    {
        var code = RandomCodeGenerator.GenerateCode();
        user.Code = code;
        user.CodeExpiryTime = DateTime.UtcNow.AddHours(1);
        await userManager.UpdateAsync(user);

        var emailTemplate = emailService.GetEmail("confirmation-email");
        ArgumentNullException.ThrowIfNull(emailTemplate);

        emailTemplate = emailTemplate.Replace("{code}", code.ToString());

        await emailSender.SendEmailAsync(new CreateEmailDto
        {
            Email = user.Email,
            Subject = "Confirm your account",
            Message = emailTemplate
        });
    }
}
