using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Models.Jwt;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]/{role}")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request, [FromRoute] string role)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response = await authService.ProcessLoginAsync(request, role);
            SetAuthCookies(response);
            return Ok(response);
        }
        catch (Exception ex) when (ex.Message == "INVALID_CREDENTIALS")
        {
            return BadRequest("Invalid email or password.");
        }
        catch (Exception ex) when (ex.Message == "EMAIL_NOT_CONFIRMED")
        {
            return BadRequest("Email not confirmed.");
        }
        catch (Exception ex) when (ex.Message == "INVALID_ROLE")
        {
            return StatusCode(403, "Invalid role for this user.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unexpected error during login for email: {Email}", request.Email);
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }

    [HttpGet("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromRoute] string role)
    {
        try
        {
            var accessToken = HttpContext.Request.Cookies["accessToken"];
            var refreshToken = HttpContext.Request.Cookies["refreshToken"];
            
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                return BadRequest("Token not found.");
                
            var response = await authService.RefreshTokenAsync(accessToken, refreshToken, role);
            SetAuthCookies(response);
            return Ok(response);
        }
        catch (Exception ex) when (ex.Message == "INVALID_TOKEN")
        {
            return BadRequest("Invalid token.");
        }
        catch (Exception ex) when (ex.Message == "INVALID_ROLE")
        {
            return StatusCode(403, "Invalid role for this user.");
        }
        catch (Exception ex) when (ex.Message == "PHARMACY_NOT_FOUND")
        {
            return BadRequest("Pharmacy not found.");
        }
        catch (Exception ex) when (ex.Message == "PHARMACY_STAFF_NOT_FOUND")
        {
            return BadRequest("Pharmacy staff not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while refreshing token.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        RemoveAuthCookies();
        return Ok();
    }

    private void SetAuthCookies(AuthResponse response)
    {
        SetCookie("accessToken", response.Token, DateTime.MaxValue);
        SetCookie("refreshToken", response.RefreshToken, DateTime.UtcNow.AddDays(7));
    }

    private void RemoveAuthCookies()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(-1)
        };

        Response.Cookies.Delete("accessToken", cookieOptions);
        Response.Cookies.Delete("refreshToken", cookieOptions);
    }

    private void SetCookie(string key, string value, DateTime expires)
    {
        HttpContext.Response.Cookies.Append(key, value, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.None,
            Expires = expires,
        });
    }
}
