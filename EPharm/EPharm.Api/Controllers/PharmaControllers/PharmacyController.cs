using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
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
    public async Task<ActionResult<IEnumerable<GetPharmaCompanyDto>>> GetAllPharmaCompanies()
    {
        var result = await pharmacyService.GetAllPharmaCompaniesAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Pharmaceutical companies not found.");
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = IdentityData.Admin + "," + IdentityData.PharmaCompanyAdmin + "," + IdentityData.PharmaCompanyManager)]
    public async Task<ActionResult<GetPharmaCompanyDto>> GetAllPharmaCompanies(int id)
    {
        var company = await pharmacyService.GetPharmaCompanyByIdAsync(id);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            if (company.OwnerId != userId)
                return Forbid();
        }
        
        var result = await pharmacyService.GetPharmaCompanyByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Pharmaceutical company with ID: {id} not found.");
    }
    
    
    [HttpPost]
    [Route("invite")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<IActionResult> InvitePharmaAdmin([FromBody] EmailDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await pharmacyService.InvitePharmaAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Error inviting pharma admin, {Error}", ex.Message);
            return BadRequest("Error inviting pharma admin.");
        }
    }

    [HttpPost]
    [Route("initialize")]
    public async Task<IActionResult> InitializePharmaCompany([FromQuery] string userId, [FromQuery] string token, [FromBody] CreatePharmaDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await pharmacyService.InitializePharmaAsync(userId, token, request);
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
    public async Task<IActionResult> RegisterPharmaAdmin([FromBody] CreatePharmaDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await pharmacyService.CreatePharmaAsync(request);
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
    public async Task<ActionResult> UpdatePharmaCompany(int id, [FromBody] CreatePharmaCompanyDto pharmaCompanyDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var result = await pharmacyService.UpdatePharmaCompanyAsync(id, pharmaCompanyDto);

        if (result)
            return Ok("Pharmaceutical company updated with success.");

        Log.Error("Error updating pharma company");
        return BadRequest("Error updating pharmaceutical company.");
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> DeletePharmaCompany(int id)
    {
        var result = await pharmacyService.DeletePharmaCompanyAsync(id);

        if (result) return NoContent();

        Log.Error("Error deleting pharma company");
        return BadRequest($"Pharmaceutical company with ID: {id} could not be deleted.");
    }
}
