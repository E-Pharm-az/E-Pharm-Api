using System.Security.Claims;
using EPharm.Domain.Dtos.AuthDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.JwtContracts;
using EPharm.Domain.Models.Identity;
using EPharm.Domain.Models.Jwt;
using EPharm.Infrastructure.Entities.Identity;
using EPharm.Infrastructure.Interfaces.Pharma;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace EPharm.Domain.Services.Common;

public class AuthService(
    UserManager<AppIdentityUser> userManager,
    ITokenService tokenService,
    IPharmacyRepository pharmacyRepository,
    IPharmacyStaffRepository pharmacyStaffRepository,
    IConfiguration configuration)
    : IAuthService
{
    
    private const int MaxFailedLoginAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(30);
    
    public async Task<AuthResponse> ProcessLoginAsync(AuthRequest request, string role)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new Exception("INVALID_CREDENTIALS");

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            throw new Exception("INVALID_CREDENTIALS");

        if (!user.EmailConfirmed)
            throw new Exception("EMAIL_NOT_CONFIRMED");

        var roles = (await userManager.GetRolesAsync(user)).ToList();
        if (!roles.Contains(role))
            throw new Exception("INVALID_ROLE");

        var response = await CreateTokenBasedOnRole(user, roles);
        
        user.RefreshToken = tokenService.RefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(Convert.ToInt32(configuration["JwtSettings:RefreshTokenExpirationDays"]));
        await userManager.UpdateAsync(user);

        response.RefreshToken = user.RefreshToken;
        return response;
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
            }

            throw new Exception("CODE_EXPIRED");
        }

        user.CodeVerificationFailedAttempts++;
        await userManager.UpdateAsync(user);

        throw new Exception("INVALID_CODE");
    }

    public async Task<AuthResponse> RefreshTokenAsync(string accessToken, string refreshToken, string role)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
            throw new Exception("INVALID_TOKEN");

        var emailClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(emailClaim))
            throw new Exception("INVALID_TOKEN");

        var user = await userManager.FindByEmailAsync(emailClaim);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new Exception("INVALID_TOKEN");

        var roles = (await userManager.GetRolesAsync(user)).ToList();
        
        if (!roles.Contains(role))
            throw new Exception("INVALID_ROLE");

        var response = await CreateTokenBasedOnRole(user, roles);
        response.RefreshToken = user.RefreshToken;

        return response;
    }

    private async Task<AuthResponse> CreateTokenBasedOnRole(AppIdentityUser user, List<string> roles)
    {
        if (roles.Contains(IdentityData.PharmacyAdmin))
        {
            var pharmacy = await pharmacyRepository.GetByOwnerId(user.Id);
            if (pharmacy == null)
                throw new Exception("PHARMACY_NOT_FOUND");
            
            return tokenService.CreateToken(user, roles, pharmacy.Id);
        }

        if (roles.Contains(IdentityData.PharmacyStaff))
        {
            var staff = await pharmacyStaffRepository.GetByExternalIdAsync(user.Id);
            if (staff == null)
                throw new Exception("PHARMACY_STAFF_NOT_FOUND");
            
            return tokenService.CreateToken(user, roles, staff.PharmacyId);
        }

        return tokenService.CreateToken(user, roles);
    }
}
