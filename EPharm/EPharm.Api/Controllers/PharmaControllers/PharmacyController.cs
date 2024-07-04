using EPharm.Domain.Dtos.PharmacyDtos;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using EPharm.Infrastructure.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers.PharmaControllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PharmacyController(IPharmacyService pharmacyService, IUserService userService) : ControllerBase
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
    public async Task<IActionResult> InvitePharmacy([FromBody] EmailDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await pharmacyService.InvitePharmacyAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Error inviting pharma admin, {Error}", ex.Message);
            return BadRequest("Error inviting pharma admin.");
        }
    }

    [HttpPost]
    [Route("verify")]
    [AllowAnonymous]
    public async Task<IActionResult> Validate([FromQuery] string userId)
    {
        var user = await userService.GetUserByIdAsync(userId);
        
        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    [Route("onboard")]
    public async Task<IActionResult> InitializePharmacy([FromQuery] string userId, [FromQuery] string token, [FromBody] CreatePharmaDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await pharmacyService.InitializePharmacyAsync(userId, token, request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Error initializing pharma company, {Error}", ex.Message);
            return BadRequest("Error initializing pharmaceutical company.");
        }
    }

    [HttpPost]
    [Route("register")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<IActionResult> RegisterPharmacyAdmin([FromBody] CreatePharmaDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await pharmacyService.CreatePharmacyAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Error creating pharma admin, {Error}", ex.Message);
            return BadRequest("Error creating admin.");
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> UpdatePharmaCompany(int id, [FromBody] CreatePharmacyDto pharmacyDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var result = await pharmacyService.UpdatePharmacyAsync(id, pharmacyDto);

        if (result)
            return Ok("Pharmaceutical company updated with success.");

        Log.Error("Error updating pharma company");
        return BadRequest("Error updating pharmaceutical company.");
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> DeletePharmaCompany(int id)
    {
        var result = await pharmacyService.DeletePharmacyAsync(id);

        if (result) return NoContent();

        Log.Error("Error deleting pharma company");
        return BadRequest($"Pharmaceutical company with ID: {id} could not be deleted.");
    }
}
