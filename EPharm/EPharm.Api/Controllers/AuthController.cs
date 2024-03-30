using System.Security.Claims;
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
public class AuthController(IConfiguration configuration, UserManager<AppIdentityUser> userManager, ITokenService tokenService) : ControllerBase
{
    [HttpPost]
    [Route("login/store")]
    public async Task<IActionResult> LoginStore([FromBody] AuthRequest request)
    {
        return await ProcessLogin(request, IdentityData.Customer);
    }
    
    [HttpPost]
    [Route("login/pharma")]
    public async Task<IActionResult> LoginPharm([FromBody] AuthRequest request)
    {
        return await ProcessLogin(request, IdentityData.PharmaCompanyManager);
    }
    
    [HttpPost]
    [Route("login/admin")]
    public async Task<IActionResult> LoginAdmin([FromBody] AuthRequest request)
    {
        return await ProcessLogin(request, IdentityData.Admin);
    }

    private async Task<IActionResult> ProcessLogin(AuthRequest request, string requiredRole)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return BadRequest("Bad credentials");

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
            return BadRequest("Bad credentials");

        try
        {
            var roles = (await userManager.GetRolesAsync(user)).ToList();
            if (!roles.Contains(requiredRole))
                return BadRequest("Bad credentials");

            var auth = tokenService.CreateToken(user, roles);
            
            user.RefreshToken = tokenService.RefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            auth.RefreshToken = user.RefreshToken;
            
            await userManager.UpdateAsync(user);
            
            HttpContext.Response.Cookies.Append("token", auth.Token, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["JwtSettings:ExpirationMinutes"]))
            });
            
            HttpContext.Response.Cookies.Append("refreshToken", user.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Expires = user.RefreshTokenExpiryTime
            });

            return Ok(auth);
        }
        catch (Exception ex)
        {
            Log.Error("Error logging in, User id: {UserId}, Error: {Error}", user.Id, ex.Message);
            return BadRequest("An unexpected error occurred while logging in");
        }
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

            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
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
}
