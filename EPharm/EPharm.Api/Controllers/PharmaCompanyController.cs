using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PharmaCompanyController(IPharmaCompanyService pharmaCompanyService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetPharmaCompanyDto>>> GetAllPharmaCompanies()
    {
        var result = await pharmaCompanyService.GetAllPharmaCompaniesAsync();
        if (result.Any()) return Ok(result);
        
        return NotFound("Pharmaceutical companies not found.");
    }
    
    [HttpGet("{pharmaCompanyId:int}")]
    public async Task<ActionResult<GetPharmaCompanyDto>> GetAllPharmaCompanies(int pharmaCompanyId)
    {
        var result = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);
        if (result is not null) return Ok(result);
        
        return NotFound($"Pharmaceutical company with ID: {pharmaCompanyId} not found.");
    }
    
    [HttpPost]
    public async Task<ActionResult<GetPharmaCompanyDto>> CreatePharmaCompany([FromBody] CreatePharmaCompanyDto pharmaCompanyDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        await pharmaCompanyService.CreatePharmaCompanyAsync(pharmaCompanyDto);

        return Ok("Pharmaceutical company created with success.");
    } 
    
    [HttpPut]
    public async Task<ActionResult> UpdatePharmaCompany([FromBody] CreatePharmaCompanyDto pharmaCompanyDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var result = await pharmaCompanyService.UpdatePharmaCompanyAsync(pharmaCompanyDto);
            
        if (result) return Ok("Pharmaceutical company updated with success.");

        return BadRequest("Error updating pharmaceutical company.");
    }
   
    [HttpDelete("{pharmaCompanyId:int}")] 
    public async Task<ActionResult> DeletePharmaCompany(int pharmaCompanyId)
    {
        var result = await pharmaCompanyService.DeletePharmaCompanyAsync(pharmaCompanyId);

        if (result) return Ok($"Pharmaceutical company with ID: {pharmaCompanyId} deleted with success.");

        return BadRequest($"Pharmaceutical company with ID: {pharmaCompanyId} could not be deleted.");
    } 
}