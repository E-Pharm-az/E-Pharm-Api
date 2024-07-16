using System.Security.Claims;
using EPharm.Domain.Dtos.AuthDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.JwtContracts;
using EPharm.Domain.Models.Identity;
using EPharm.Domain.Models.Jwt;
using EPharm.Infrastructure.Entities.Identity;
using EPharm.Infrastructure.Interfaces.Pharma;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IConfiguration configuration,
    UserManager<AppIdentityUser> userManager,
    IPharmacyRepository pharmacyRepository,
    IPharmacyStaffRepository pharmacyStaffRepository,
    ITokenService tokenService,
    IUserService userService) : ControllerBase
{
    private const int MaxFailedLoginAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(30);

    [HttpPost]
    [Route("store/login")]
    public async Task<IActionResult> StoreLogin([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response = await ProcessLogin(request, IdentityData.Customer);
            return Ok(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error logging in.");
            return BadRequest("An unexpected error occurred while logging in");
        }
    }

    [HttpPost]
    [Route("pharmacy/login")]
    public async Task<IActionResult> PharmacyLogin([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response = await ProcessLogin(request, IdentityData.PharmacyStaff);
            var user = await userManager.FindByEmailAsync(request.Email);
            
            var roles = (await userManager.GetRolesAsync(user)).ToList();
            
            foreach (var role in roles)
                Console.WriteLine(role);
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error logging in.");
            return BadRequest("An unexpected error occurred while logging in");
        }
    }

    [HttpPost]
    [Route("admin/login")]
    public async Task<IActionResult> AdminLogin([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var response = await ProcessLogin(request, IdentityData.Admin);
            return Ok(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error logging in.");
            return BadRequest("An unexpected error occurred while logging in");
        }
    }

    [HttpPost]
    [Route("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] EmailDto request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return BadRequest("User not found");

        try
        {
            await userService.SendEmailConfirmationAsync(user);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error sending email.");
            return StatusCode(500, "An unexpected error occurred");
        }
    }

    [HttpPost]
    [Route("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto request)
    {
        if (string.IsNullOrEmpty(request.Email))
            return BadRequest("Invalid request parameters");

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return BadRequest("User not found");

        // Checks if lockout duration ended
        if (user.LockoutEnd > DateTime.UtcNow)
            return BadRequest("Too many attempts, please try again later.");

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
                return BadRequest("To many tries, please try again later.");
            }
        }

        if (user.Code == request.Code)
        {
            if (user.CodeExpiryTime > DateTime.UtcNow)
            {
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);
                return Ok("Email confirmed successfully");
            }

            return BadRequest("Code expired. Please generate a new one.");
        }

        user.CodeVerificationFailedAttempts++;
        await userManager.UpdateAsync(user);

        return BadRequest("Invalid code.");
    }

    [HttpGet]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {
            var request = new TokenModel
            {
                Token = HttpContext.Request.Cookies["accessToken"],
                RefreshToken = HttpContext.Request.Cookies["refreshToken"]
            };

            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.RefreshToken))
            {
                Log.Warning("Token or refreshToken is missing in the request");
                return BadRequest("Token or refreshToken is missing");
            }

            var principal = tokenService.GetPrincipalFromExpiredToken(request.Token);

            if (principal == null)
            {
                Log.Warning("Invalid client request: Principal is null");
                return BadRequest("Invalid client request");
            }

            var emailClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(emailClaim))
            {
                Log.Warning("Invalid client request: Email claim is missing");
                return BadRequest("Invalid client request");
            }

            var user = await userManager.FindByEmailAsync(emailClaim);

            if (user == null || user.RefreshToken != request.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                Log.Warning("Invalid access token or refresh token");
                return BadRequest("Invalid access token or refresh token");
            }

            var roles = (await userManager.GetRolesAsync(user)).ToList();

            var response = await CreateTokenBasedOnRole(user, roles);
            response.RefreshToken = user.RefreshToken;
            
            SetAccessTokenCookie(response.Token);
            SetRefreshTokenCookie(user.RefreshToken);

            return Ok(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while refreshing token");
            return StatusCode(500, "An unexpected error occurred");
        }
    }

    [HttpGet]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Response.Cookies.Delete("accessToken");
        HttpContext.Response.Cookies.Delete("refreshToken");

        return Ok();
    }

    private async Task<AuthResponse> ProcessLogin(AuthRequest request, string requiredRole)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new Exception("EMAIL_NOT_FOUND");

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            throw new Exception("BAD_CREDENTIALS");

        if (!user.EmailConfirmed)
            throw new Exception("EMAIL_NOT_CONFIRMED");

        var roles = (await userManager.GetRolesAsync(user)).ToList();
        if (!roles.Contains(requiredRole))
            throw new Exception("BAD_CREDENTIALS");

        var auth = await CreateTokenBasedOnRole(user, roles);

        user.RefreshToken = tokenService.RefreshToken();
        auth.RefreshToken = user.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(Convert.ToInt32(configuration["JwtSettings:RefreshTokenExpirationDays"]));

        await userManager.UpdateAsync(user);
        
        SetAccessTokenCookie(auth.Token);
        SetRefreshTokenCookie(user.RefreshToken);

        return auth;
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

    private void SetAccessTokenCookie(string value) =>
        SetCookie("accessToken", value, DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["JwtSettings:ExpirationMinutes"])));
    
    private void SetRefreshTokenCookie(string value) =>
        SetCookie("refreshToken", value, DateTime.UtcNow.AddDays(Convert.ToInt32(configuration["JwtSettings:RefreshTokenExpirationDays"])));

    private void SetCookie(string key, string value, DateTime expires)
    {
        HttpContext.Response.Cookies.Append(key, value, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.Strict,
            Expires = expires
        });
    }
}
