using EPharm.Domain.Dtos.AuthDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Models.Identity;
using EPharm.Infrastructure.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, UserManager<AppIdentityUser> userManager) : ControllerBase
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
    [Authorize]
    public async Task<ActionResult<GetUserDto>> GetUserById(string userId)
    {
        var result = await userService.GetUserByIdAsync(userId);
        if (result is  null)
            return NotFound($"User with ID: {userId} not found.");
        
        var currentUserId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;

        if (!User.IsInRole(IdentityData.Admin) && currentUserId != result.Id)
            return Forbid();

        return Ok(result);
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
        catch (Exception ex) when (ex.Message == "USER_ALREADY_EXISTS")
        {
            return Conflict(new { message = "User with this email already exists." });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating user.");
            return BadRequest("Error creating user.");
        }
    }

    [HttpPost]
    [Route("initialize")]
    public async Task<IActionResult> Initialize([FromBody] InitializeUserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await userService.InitializeUserAsync(request);
            return Ok();
        }
        catch (Exception ex) when (ex.Message == "USER_NOT_FOUND")
        {
            Log.Warning(ex, "User initialization failed: User not found");
            return NotFound(new { error = "User not found", code = "USER_NOT_FOUND" });
        }
        catch (Exception ex) when (ex.Message == "USER_ALREADY_INITIALIZED")
        {
            Log.Warning(ex, "User initialization failed: User already initialized");
            return BadRequest(new { error = "User already initialized", code = "USER_ALREADY_INITIALIZED" });
        }
        catch (Exception ex) when (ex.Message == "USER_UPDATE_FAILED")
        {
            Log.Error(ex, "User initialization failed: Unable to update user");
            return StatusCode(500, new { error = "Failed to update user", code = "USER_UPDATE_FAILED" });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unexpected error during user initialization");
            return StatusCode(500, new { error = "An unexpected error occurred", code = "INTERNAL_SERVER_ERROR" });
        }
    }
    
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            await userService.ConfirmEmailAsync(request);
            return Ok("Email confirmed successfully");
        }
        catch (Exception ex) when (ex.Message == "INVALID_CODE")
        {
            return BadRequest("Invalid code.");
        }
        catch (Exception ex) when (ex.Message == "CODE_EXPIRED")
        {
            return BadRequest("Code expired.");
        }
        catch (Exception ex) when (ex.Message == "USER_NOT_FOUND")
        {
            return BadRequest("User not found.");
        }
        catch (Exception ex) when (ex.Message == "TOO_MANY_ATTEMPTS")
        {
            return BadRequest("Too many attempts.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unexpected error during email confirmation");
            return StatusCode(500, "An unexpected error occurred");
        }
    }
    
    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] EmailDto request)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return NotFound("User not found");

            await userService.SendEmailConfirmationAsync(user);
            return Ok();
        }
        catch (Exception ex) when (ex.Message == "USER_NOT_FOUND")
        {
            return BadRequest("User not found");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error sending email.");
            return StatusCode(500, "An unexpected error occurred");
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
            Log.Error(ex, "Error creating admin.");
            return BadRequest("Error creating admin.");
        }
    }


    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> UpdateUser(string id, [FromBody] EmailDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var currentUserId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;

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
        var currentUserId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;

        if (!User.IsInRole(IdentityData.Admin) && currentUserId != userId)
            return Forbid();

        var result = await userService.DeleteUserAsync(userId);

        if (result) return NoContent();

        Log.Error("Error updating user");
        return BadRequest($"User with ID: {userId} could not be deleted.");
    }

    [HttpPost]
    [Route("initiate-password-change")]
    public async Task<IActionResult> InitiatePasswordChange([FromBody] EmailDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await userService.InitiatePasswordChange(request);
            return Ok();
        }
        catch (ArgumentNullException)
        {
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            Log.Warning(ex, "Warning creating email template.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred. Please try again later." });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error initiating password change.");
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
            Log.Error(ex, "Error changing password.");
            return BadRequest("Error changing password.");
        }
    }
}
