using System.Security.Claims;
using EPharm.Domain.Dtos.AuthDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.JwtContracts;
using EPharm.Domain.Models.Identity;
using EPharm.Domain.Models.Jwt;
using EPharm.Infrastructure.Context.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IConfiguration configuration,
    UserManager<AppIdentityUser> userManager,
    ITokenService tokenService,
    IUserService userService
) : ControllerBase
{
    private const int MaxFailedLoginAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(30);

    [HttpPost]
    [Route("login/store")]
    public async Task<IActionResult> LoginStore([FromBody] AuthRequest request)
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
            Log.Error("Error logging in, Error: {Error}", ex.Message);
            return BadRequest("An unexpected error occurred while logging in");
        }
    }

    [HttpPost]
    [Route("login/pharma")]
    public async Task<IActionResult> LoginPharm([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            var response = await ProcessLogin(request, IdentityData.PharmaCompanyManager);
            return Ok(response);
        }
        catch (Exception ex)
        {
            Log.Error("Error logging in, Error: {Error}", ex.Message);
            return BadRequest("An unexpected error occurred while logging in");
        }
    }

    [HttpPost]
    [Route("login/admin")]
    public async Task<IActionResult> LoginAdmin([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var response =  await ProcessLogin(request, IdentityData.Admin);
            return Ok(response);
        }
        catch (Exception ex)
        {
            Log.Error("Error logging in, Error: {Error}", ex.Message);
            return BadRequest("An unexpected error occurred while logging in");
        }
    }

    [HttpPost]
    [Route("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendEmailDto request)
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
            Log.Error("Error sending email, Error: {Error}", ex.Message);
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
            }
            else
            {
                user.LockoutEnd = DateTime.UtcNow.Add(LockoutDuration);
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
                Token = HttpContext.Request.Cookies["token"],
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

            var roles = await userManager.GetRolesAsync(user);

            var response = tokenService.CreateToken(user, roles.ToList());
            response.RefreshToken = user.RefreshToken;

            HttpContext.Response.Cookies.Append("token", response.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["JwtSettings:ExpirationMinutes"]))
            });

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
        HttpContext.Response.Cookies.Delete("token");
        HttpContext.Response.Cookies.Delete("refreshToken");

        return Ok();
    }

    private async Task<AuthResponse> ProcessLogin(AuthRequest request, string requiredRole)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
            throw new ArgumentException("Bad credentials");

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
            throw new ArgumentException("Bad credentials");

        if (!user.EmailConfirmed)
            throw new ArgumentException("Email not confirmed, please check your email for confirmation.");

        var roles = (await userManager.GetRolesAsync(user)).ToList();
        if (!roles.Contains(requiredRole))
            throw new ArgumentException("Bad credentials");

        var auth = tokenService.CreateToken(user, roles);

        user.RefreshToken = tokenService.RefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        auth.RefreshToken = user.RefreshToken;

        await userManager.UpdateAsync(user);

        HttpContext.Response.Cookies.Append("token", auth.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["JwtSettings:ExpirationMinutes"]))
        });

        HttpContext.Response.Cookies.Append("refreshToken", user.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.None,
            Expires = user.RefreshTokenExpiryTime
        });

        return auth;
    }
}
