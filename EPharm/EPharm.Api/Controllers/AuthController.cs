using System.Security.Claims;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.Jwt;
using EPharm.Domain.Interfaces.User;
using EPharm.Domain.Models.Jwt;
using EPharm.Infrastructure.Context.Entities.Identity;
using EPharm.Infrastructure.Interfaces.IdentityRepositoriesInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    UserManager<AppIdentityUser> userManager,
    IAppIdentityUserRepository userRepository,
    IUserService userService,
    ITokenService tokenService
) : ControllerBase
{
    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await userService.CreateUser(request);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("login")]
    [HttpPost]
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
            return BadRequest(ex.Message);
        }
    }

    [Route("refreshToken")]
    [HttpPost]
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
