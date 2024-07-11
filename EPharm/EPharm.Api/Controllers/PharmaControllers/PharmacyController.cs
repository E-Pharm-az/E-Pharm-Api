using EPharm.Domain.Dtos.PharmacyDtos;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using EPharmApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers.PharmaControllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PharmacyController(IPharmacyService pharmacyService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<IEnumerable<GetPharmacyDto>>> GetAllPharmacies()
    {
        var result = await pharmacyService.GetAllPharmacyAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Pharmaceutical companies not found.");
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = IdentityData.Admin + "," + IdentityData.PharmacyStaff)]
    public async Task<ActionResult<GetPharmacyDto>> GetAllPharmacies(int id)
    {
        var company = await pharmacyService.GetPharmacyByIdAsync(id);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            if (company.Owner.Id != userId)
                return Forbid();
        }
        
        var result = await pharmacyService.GetPharmacyByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Pharmaceutical company with ID: {id} not found.");
    }
    
    [HttpPost]
    [Route("invite")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<IActionResult> Invite([FromBody] EmailDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await pharmacyService.InviteAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error inviting pharma admin,");
            return BadRequest("Error inviting pharma admin.");
        }
    }
    
    [HttpPost]
    [Route("verify")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyInvitation([FromQuery] string userId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var user = await pharmacyService.VerifyInvitationAsync(userId);
            return Ok(user);
        }
        catch (Exception ex) when (ex.Message == "INVALID_INVITATION")
        {
            return BadRequest("Invalid invitation");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error verifying invitation");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("{userId}")]
    [PharmacyOwner]
    public async Task<IActionResult> Create(string userId, [FromBody] CreatePharmacyDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await pharmacyService.CreateAsync(userId, request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error initializing pharma company.");
            return BadRequest("Error initializing pharmaceutical company.");
        }
    }

    [HttpPost("register")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<IActionResult> Register([FromBody] CreatePharmaDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await pharmacyService.Register(request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating pharma admin.");
            return BadRequest("Error creating admin.");
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> UpdatePharmaCompany(int id, [FromBody] CreatePharmacyDto pharmacyDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var result = await pharmacyService.UpdateAsync(id, pharmacyDto);

        if (result)
            return Ok("Pharmaceutical company updated with success.");

        Log.Error("Error updating pharma company");
        return BadRequest("Error updating pharmaceutical company.");
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> DeletePharmaCompany(int id)
    {
        var result = await pharmacyService.DeleteAsync(id);

        if (result) return NoContent();

        Log.Error("Error deleting pharma company");
        return BadRequest($"Pharmaceutical company with ID: {id} could not be deleted.");
    }
}
