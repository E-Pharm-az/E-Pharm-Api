using System.Security.Claims;
using EPharm.Domain.Dtos.AuthDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<IEnumerable<GetUserDto>>> GetAllUsers()
    {
        var results = await userService.GetAllUsersAsync();
        if (results.Any()) return Ok(results);

        return NotFound("Users not found.");
    }

    [HttpGet("{userId}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<GetUserDto>> GetUserById(string userId)
    {
        var result = await userService.GetUserByIdAsync(userId);
        if (result is not null) return Ok(result);

        return NotFound($"User with ID: {userId} not found.");
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] EmailDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await userService.CreateCustomerAsync(request);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            Log.Error("Error creating user, {Error}", ex.Message);
            return StatusCode(StatusCodes.Status409Conflict, new { message = "User with this email already exists." });
        }
        catch (Exception ex)
        {
            Log.Error("Error creating user, {Error}", ex.Message);
            return BadRequest("Error creating user.");
        }
    }

    [HttpPost]
    [Route("initialize-user")]
    public async Task<IActionResult> InitializeUser([FromBody] InitializeUserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await userService.InitializeUserAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Error initializing user, {Error}", ex.Message);
            return BadRequest("Error initializing user.");
        }
    }

    [HttpPost]
    [Route("register/admin")]
    [Authorize(Roles = IdentityData.SuperAdmin)]
    public async Task<IActionResult> RegisterAdmin([FromBody] EmailDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await userService.CreateAdminAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Error creating admin, {Error}", ex.Message);
            return BadRequest("Error creating admin.");
        }
    }


    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> UpdateUser(string id, [FromBody] EmailDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!User.IsInRole(IdentityData.Admin) && currentUserId != id)
            return Forbid();

        var result = await userService.UpdateUserAsync(id, userDto);

        if (result)
            return Ok("User updated successfully.");

        Log.Error("Error updating user");
        return BadRequest("Error updating user.");
    }

    [HttpDelete("{userId}")]
    [Authorize]
    public async Task<ActionResult> DeleteUser(string userId)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!User.IsInRole(IdentityData.Admin) && currentUserId != userId)
            return Forbid();

        var result = await userService.DeleteUserAsync(userId);

        if (result) return NoContent();

        Log.Error("Error updating user");
        return BadRequest($"User with ID: {userId} could not be deleted.");
    }

    [HttpPost]
    [Route("initiate-password-change")]
    public async Task<IActionResult> InitiatePasswordChange([FromBody] InitiatePasswordChangeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await userService.InitiatePasswordChange(request, configuration["AppUrls:EpharmClient"]!);
            return Ok();
        }
        catch (ArgumentNullException)
        {
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            Log.Warning("Warning creating email template, details: {Error}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred. Please try again later." });
        }
        catch (Exception ex)
        {
            Log.Error("Error initiating password change. Details: {Error}", ex);
            return BadRequest("Error initiating password change.");
        }
    }

    [HttpPost]
    [Route("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordWithTokenRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await userService.ChangePassword(request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Error changing password. Details: {Error}", ex);
            return BadRequest("Error changing password.");
        }
    }
}
