using System.Security.Claims;
using EPharm.Domain.Interfaces.Jwt;
using EPharm.Domain.Models.Jwt;
using EPharm.Infrastructure.Context.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(UserManager<AppIdentityUser> userManager, ITokenService tokenService) : ControllerBase
{
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
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
            var auth = tokenService.CreateToken(user);

            user.RefreshToken = tokenService.RefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            auth.RefreshToken = user.RefreshToken;

            await userManager.UpdateAsync(user);

            return Ok(auth);
        }
        catch (Exception ex)
        {
            Log.Error("Error logging in, User id: {UserId}, Error: {Error}", user.Id, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenModel request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var principal = tokenService.GetPrincipalFromExpiredToken(request.Token);

        if (principal == null)
            return BadRequest("Invalid client request");

        var email = principal.Claims.First(x => x.Type == ClaimTypes.Email);

        var user = await userManager.FindByEmailAsync(email.Value);

        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return BadRequest("Invalid access token or refresh token");
        }

        var response = tokenService.CreateToken(user);
        response.RefreshToken = tokenService.RefreshToken();

        user.RefreshToken = response.RefreshToken;
        await userManager.UpdateAsync(user);

        return Ok(response);
    }
}
