using System.Security.Claims;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
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
    public async Task<IActionResult> Register([FromBody] CreateUserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await userService.CreateCustomerAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Error creating user, {Error}", ex.Message);
            return BadRequest("Error creating user.");
        }
    }

    [HttpPost]
    [Route("register/admin")]
    [Authorize(Roles = IdentityData.SuperAdmin)]
    public async Task<IActionResult> RegisterAdmin([FromBody] CreateUserDto request)
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

    [HttpPost]
    [Route("register/pharma-admin")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<IActionResult> RegisterPharmaAdmin([FromBody] CreatePharmaAdminDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await userService.CreatePharmaAdminAsync(request.UserRequest, request.PharmaCompanyRequest);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Error creating pharma admin, {Error}", ex.Message);
            return BadRequest("Error creating admin.");
        }
    }

    [HttpPost]
    [Route("register/{companyId:int}/pharma-manager")]
    [Authorize(Roles = IdentityData.PharmaCompanyAdmin)]
    public async Task<IActionResult> RegisterPharmaManager(int companyId, [FromBody] CreateUserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await userService.CreatePharmaManagerAsync(companyId, request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Error creating pharma manager, {Error}", ex.Message);
            return BadRequest("Error creating pharma manager");
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> UpdateUser(string id, [FromBody] CreateUserDto userDto)
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
}
