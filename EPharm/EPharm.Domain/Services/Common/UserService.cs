using AutoMapper;
using EPharm.Domain.Dtos.AuthDto;
using EPharm.Domain.Dtos.EmailDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Models.Identity;
using EPharm.Infrastructure.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EPharm.Domain.Services.Common;

public class UserService(
    UserManager<AppIdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IEmailSender emailSender,
    IEmailService emailService,
    IConfiguration configuration,
    IPasswordHasher<AppIdentityUser> passwordHasher,
    IMapper mapper) : IUserService
{
    private const int MaxFailedLoginAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(30);
    
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
            throw new Exception("USER_NOT_FOUND");

        if (user.IsAccountSetup == true)
            throw new Exception("USER_ALREADY_INITIALIZED");

        if (user.Code != initializeUserDto.Code || user.CodeExpiryTime < DateTime.UtcNow)
            throw new Exception("INVALID_CODE");

        mapper.Map(initializeUserDto, user);
        user.PasswordHash = passwordHasher.HashPassword(user, initializeUserDto.Password);
        user.IsAccountSetup = true;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new Exception("USER_UPDATE_FAILED");
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

    public async Task InitiatePasswordChange(InitiatePasswordChangeRequest passwordChangeRequest)
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

    public async Task<AppIdentityUser> CreateUserAsync<T>(T user, string[]? identityRoles = null) where T: EmailDto
    {
        var existingUser = await userManager.FindByEmailAsync(user.Email);
        if (existingUser is not null)
        {
            if (existingUser.IsAccountSetup == true)
                throw new Exception("USER_ALREADY_EXISTS");

            return existingUser;
        }

        var userEntity = mapper.Map<AppIdentityUser>(user);
        userEntity.UserName = user.Email;

        var result = await userManager.CreateAsync(userEntity, configuration["UniqueKey"]!);

        if (identityRoles == null)
            return userEntity;
        
        foreach (var role in identityRoles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        if (!result.Succeeded)
            throw new InvalidOperationException(
                $"Failed to create user. Details: {string.Join("; ", result.Errors.Select(e => e.Description))}");

        await userManager.AddToRolesAsync(userEntity, identityRoles);

        return userEntity;
    }
    
    
    public async Task ConfirmEmailAsync(ConfirmEmailDto request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
            throw new Exception("USER_NOT_FOUND");

        // Checks if lockout duration ended
        if (user.LockoutEnd > DateTime.UtcNow)
            throw new Exception("TOO_MANY_ATTEMPTS");

        if (user.CodeVerificationFailedAttempts >= MaxFailedLoginAttempts)
        {
            // If failed attempts count is larger than max count, then if the lockout end expired, reset the count to 0. Else lock the user.
            if (user.LockoutEnd < DateTime.UtcNow)
            {
                user.CodeVerificationFailedAttempts = 0;
                await userManager.UpdateAsync(user);
            }
            else
            {
                user.LockoutEnd = DateTime.UtcNow.Add(LockoutDuration);
                await userManager.UpdateAsync(user);
                
                throw new Exception("TOO_MANY_ATTEMPTS");
            }
        }

        if (user.Code == request.Code)
        {
            if (user.CodeExpiryTime > DateTime.UtcNow)
            {
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);
                return;
            }

            throw new Exception("CODE_EXPIRED");
        }

        user.CodeVerificationFailedAttempts++;
        await userManager.UpdateAsync(user);

        throw new Exception("INVALID_CODE");
    }

    public async Task SendEmailConfirmationAsync(AppIdentityUser user)
    {
        var code = RandomCodeGenerator.GenerateCode();
        user.Code = code;
        user.CodeExpiryTime = DateTime.UtcNow.AddHours(1);
        await userManager.UpdateAsync(user);

        var emailTemplate = emailService.GetEmail("confirmation");
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
