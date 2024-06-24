using EPharm.Domain.Dtos.RegulatoryInformationDto;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/[controller]/{pharmaCompanyId:int}/[controller]")]
public class RegulatoryInformationController(IRegulatoryInformationService regulatoryInformationService, IPharmacyService pharmacyService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    public async Task<ActionResult<IEnumerable<GetRegulatoryInformationDto>>> GetAllCompanyRegulatoryInformation(int pharmaCompanyId)
    {
        var company = await pharmacyService.GetPharmacyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            
            if (company.OwnerId != userId)
                return Forbid();
        }
        
        var result = await regulatoryInformationService.GetAllCompanyRegulatoryInformationAsync(pharmaCompanyId);
        if (result.Any()) return Ok(result);

        return NotFound("Regulatory information not found.");
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    public async Task<ActionResult<GetRegulatoryInformationDto>> GetRegulatoryInformationById(int pharmaCompanyId, int id)
    {
        var company = await pharmacyService.GetPharmacyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            
            if (company.OwnerId != userId)
                return Forbid();
        }
        
        var result = await regulatoryInformationService.GetRegulatoryInformationByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Regulatory information with ID: {id} not found.");
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    public async Task<ActionResult<GetRegulatoryInformationDto>> CreateRegulatoryInformation(
        int pharmaCompanyId, [FromBody] CreateRegulatoryInformationDto regulatoryInformationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var company = await pharmacyService.GetPharmacyByIdAsync(pharmaCompanyId);

        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
    
        if (company.OwnerId != userId)
            return Forbid(); 

        try
        {
            var result = await regulatoryInformationService.AddCompanyRegulatoryInformationAsync(pharmaCompanyId, regulatoryInformationDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            
            Log.Error("Error creating pharma company manger, {Error}", ex.Message);
            return BadRequest("Error creating pharma company manger.");
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    public async Task<ActionResult> UpdateRegulatoryInformation(int pharmaCompanyId, int id, [FromBody] CreateRegulatoryInformationDto regulatoryInformation)
    {
        var company = await pharmacyService.GetPharmacyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            
            if (company.OwnerId != userId)
                return Forbid();
        }
        
        var result = await regulatoryInformationService.UpdateCompanyRegulatoryInformationAsync(id, regulatoryInformation);
        if (result) return Ok();

        return NotFound($"Regulatory information with ID: {id} not found.");
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    public async Task<ActionResult> DeleteRegulatoryInformation(int pharmaCompanyId, int id)
    {
        var company = await pharmacyService.GetPharmacyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            
            if (company.OwnerId != userId)
                return Forbid();
        }
        
        var result = await regulatoryInformationService.DeleteCompanyRegulatoryInformationAsync(id);
        if (result) return Ok();

        return NotFound($"Regulatory information with ID: {id} not found.");
    }
}
