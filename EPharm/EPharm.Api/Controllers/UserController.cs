using System.Security.Claims;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.User;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return BadRequest(ex.Message);
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
            return BadRequest(ex.Message);
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
            return BadRequest(ex.Message);
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
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> UpdateUser([FromBody] GetUserDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!User.IsInRole(IdentityData.Admin) && currentUserId != userDto.Id)
            return Forbid();

        var result = await userService.UpdateUserAsync(userDto);

        if (result)
            return Ok("User updated successfully.");

        return BadRequest("Error updating user.");
    }

    [HttpDelete("{userId}")]
    [Authorize]
    public async Task<ActionResult> DeleteUser(string userId)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!User.IsInRole(IdentityData.Admin) && currentUserId != userId)
            return Forbid();

        var result = await userService.DeleteUserAsync(userId);

        if (result) return Ok($"User with ID: {userId} deleted with success.");

        return BadRequest($"User with ID: {userId} could not be deleted.");
    }
}
