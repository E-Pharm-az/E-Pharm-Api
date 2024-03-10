using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Domain.Interfaces.Pharma;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers.PharmaControllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PharmaCompanyController(IPharmaCompanyService pharmaCompanyService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<IEnumerable<GetPharmaCompanyDto>>> GetAllPharmaCompanies()
    {
        var result = await pharmaCompanyService.GetAllPharmaCompaniesAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Pharmaceutical companies not found.");
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = IdentityData.Admin + "," + IdentityData.PharmaCompanyAdmin + "," + IdentityData.PharmaCompanyManager)]
    public async Task<ActionResult<GetPharmaCompanyDto>> GetAllPharmaCompanies(int id)
    {
        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(id);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti);
            if (company.PharmaCompanyOwnerId != userId.Value)
                return Forbid();
        }
        
        var result = await pharmaCompanyService.GetPharmaCompanyByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Pharmaceutical company with ID: {id} not found.");
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> UpdatePharmaCompany(int id, [FromBody] CreatePharmaCompanyDto pharmaCompanyDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var result = await pharmaCompanyService.UpdatePharmaCompanyAsync(id, pharmaCompanyDto);

        if (result) return Ok("Pharmaceutical company updated with success.");

        Log.Error("Error updating pharma company");
        return BadRequest("Error updating pharmaceutical company.");
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> DeletePharmaCompany(int id)
    {
        var result = await pharmaCompanyService.DeletePharmaCompanyAsync(id);

        if (result) return NoContent();

        Log.Error("Error deleting pharma company");
        return BadRequest($"Pharmaceutical company with ID: {id} could not be deleted.");
    }
}
