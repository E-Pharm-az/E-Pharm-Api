using System.Text;
using AutoMapper;
using EPharm.Domain.Dtos.AuthDto;
using EPharm.Domain.Dtos.EmailDto;
using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using EPharm.Infrastructure.Context.Entities.Identity;
using EPharm.Infrastructure.Interfaces.Base;
using EPharm.Infrastructure.Interfaces.PharmaRepositoriesInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EPharm.Domain.Services.Common;

public class UserService(
    UserManager<AppIdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IPharmaCompanyManagerService pharmaCompanyManagerService,
    IPharmaCompanyService pharmaCompanyService,
    IPharmacyStaffRepository pharmacyStaffRepository,
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

    public async Task CreateCustomerAsync(EmailDto emailDto)
    {
        var user = await CreateUserAsync(emailDto, [IdentityData.Customer]);
        await SendEmailConfirmationAsync(user);
    }

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

    public async Task InvitePharmaAsync(EmailDto emailDto)
    {
        var user = await CreateUserAsync(emailDto, [IdentityData.PharmaCompanyAdmin, IdentityData.PharmaCompanyManager]);
        await pharmaCompanyManagerService.CreatePharmaCompanyManagerAsync(new CreatePharmaCompanyManagerDto { Email = user.Email, ExternalId = user.Id });

        await SendEmailInvitationAsync(user);
    }

    public async Task InitializePharmaAsync(string userId, string token, CreatePharmaDto createPharmaDto)
    {
        var user = await userManager.FindByEmailAsync(createPharmaDto.UserRequest.Email);
        if (user is null)
            throw new InvalidOperationException("USER_NOT_FOUND");

        if (user.Id != userId)
            throw new InvalidOperationException("INVALID_USER");

        var pharmacyManager = await pharmacyStaffRepository.GetPharmaCompanyManagerByExternalIdAsync(userId);
        if (pharmacyManager is null)
            throw new InvalidOperationException("USER_NOT_FOUND");

        try
        {
            await unitOfWork.BeginTransactionAsync();

            await userManager.ConfirmEmailAsync(user, token);
            user.PasswordHash = passwordHasher.HashPassword(user, createPharmaDto.UserRequest.Password);

            var pharmacy =
                await pharmaCompanyService.CreatePharmaCompanyAsync(createPharmaDto.PharmaCompanyRequest, user.Id);
            
            pharmacyManager.PharmaCompanyId = pharmacy.Id;
            pharmacyStaffRepository.Update(pharmacyManager);

            await pharmaCompanyManagerService.UpdatePharmaCompanyManagerAsync(pharmacyManager.Id,
                new CreatePharmaCompanyManagerDto { PharmaCompanyId = pharmacy.Id });

            await unitOfWork.CommitTransactionAsync();
            await unitOfWork.SaveChangesAsync();
            await userManager.UpdateAsync(user);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
    
    public async Task CreatePharmaAsync(CreatePharmaDto createPharmaDto)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync();
            
            var existingUser = await userManager.FindByEmailAsync(createPharmaDto.UserRequest.Email);
            if (existingUser is not null)
            {
                if (existingUser.EmailConfirmed)
                    throw new InvalidOperationException("USER_ALREADY_EXISTS");
            }

            var userEntity = mapper.Map<AppIdentityUser>(createPharmaDto.UserRequest);
            userEntity.UserName = createPharmaDto.UserRequest.Email;
            userEntity.EmailConfirmed = true;

            if (!await roleManager.RoleExistsAsync(IdentityData.PharmaCompanyAdmin))
                await roleManager.CreateAsync(new IdentityRole(IdentityData.PharmaCompanyAdmin));
            
            if (!await roleManager.RoleExistsAsync(IdentityData.PharmaCompanyManager))
                await roleManager.CreateAsync(new IdentityRole(IdentityData.PharmaCompanyManager));
            
            var result = await userManager.CreateAsync(userEntity, configuration["UniqueKey"]!);

            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
            
            var pharmaCompany = await pharmaCompanyService.CreatePharmaCompanyAsync(createPharmaDto.PharmaCompanyRequest, userEntity.Id);

            var pharmaCompanyAdminEntity = mapper.Map<CreatePharmaCompanyManagerDto>(userEntity);
            pharmaCompanyAdminEntity.ExternalId = userEntity.Id;
            pharmaCompanyAdminEntity.PharmaCompanyId = pharmaCompany.Id;

           await pharmaCompanyManagerService.CreatePharmaCompanyManagerAsync(pharmaCompanyAdminEntity);
           await userManager.UpdateAsync(userEntity);
           
           await unitOfWork.CommitTransactionAsync();
           await unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<GetPharmaCompanyManagerDto> CreatePharmaManagerAsync(int pharmaCompanyId,
        EmailDto emailDto)
    {
        var user = await CreateUserAsync(emailDto, [IdentityData.PharmaCompanyManager]);

        var pharmaCompanyManagerEntity = mapper.Map<CreatePharmaCompanyManagerDto>(user);
        pharmaCompanyManagerEntity.ExternalId = user.Id;
        pharmaCompanyManagerEntity.PharmaCompanyId = pharmaCompanyId;

        return await pharmaCompanyManagerService.CreatePharmaCompanyManagerAsync(pharmaCompanyManagerEntity);
    }

    public async Task<GetUserDto> CreateAdminAsync(EmailDto emailDto)
    {
        var user = await CreateUserAsync(emailDto, [IdentityData.Admin]);
        await SendEmailConfirmationAsync(user);
        
        return mapper.Map<GetUserDto>(user);
    }

    public async Task<bool> UpdateUserAsync(string id, EmailDto emailDto)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user is null)
            return false;

        mapper.Map(emailDto, user);

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

    private async Task<AppIdentityUser> CreateUserAsync(EmailDto emailDto, string[] identityRole)
    {
        var existingUser = await userManager.FindByEmailAsync(emailDto.Email);
        if (existingUser is not null)
        {
            if (existingUser.EmailConfirmed)
                throw new InvalidOperationException("USER_ALREADY_EXISTS");

            return existingUser;
        }

        var userEntity = mapper.Map<AppIdentityUser>(emailDto);
        userEntity.UserName = emailDto.Email;
        
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

        return userEntity;
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

    public async Task SendEmailInvitationAsync(AppIdentityUser user)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var emailTemplate = emailService.GetEmail("pharmacy-invitation");
        if (emailTemplate is null) 
            throw new KeyNotFoundException("EMAIL_NOT_FOUND");
        
        emailTemplate = emailTemplate.Replace("{url}", $"{configuration["PharmaPortalClient"]}/join?token={encodedToken}?userId={user.Id}");
        
        await emailSender.SendEmailAsync(new CreateEmailDto
        {
            Email = user.Email,
            Subject = "Welcome to E-Pharm: Complete Your Pharmacy Registration",
            Message = emailTemplate
        });
    }
}
