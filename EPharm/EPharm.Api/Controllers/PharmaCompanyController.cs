using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Domain.Interfaces.Pharma;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = IdentityData.Admin)]
public class PharmaCompanyController(IPharmaCompanyService pharmaCompanyService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetPharmaCompanyDto>>> GetAllPharmaCompanies()
    {
        var result = await pharmaCompanyService.GetAllPharmaCompaniesAsync();
        if (result.Any()) return Ok(result);
        
        return NotFound("Pharmaceutical companies not found.");
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetPharmaCompanyDto>> GetAllPharmaCompanies(int id)
    {
        var result = await pharmaCompanyService.GetPharmaCompanyByIdAsync(id);
        if (result is not null) return Ok(result);
        
        return NotFound($"Pharmaceutical company with ID: {id} not found.");
    }
    
    [HttpPut("{id:int}")]
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
    public async Task<ActionResult> DeletePharmaCompany(int id)
    {
        var result = await pharmaCompanyService.DeletePharmaCompanyAsync(id);

        if (result) return NoContent();

        Log.Error("Error deleting pharma company");
        return BadRequest($"Pharmaceutical company with ID: {id} could not be deleted.");
    } 
}
