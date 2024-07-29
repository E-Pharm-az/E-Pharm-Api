using EPharm.Domain.Dtos.RegulatoryInformationDto;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Domain.Models.Identity;
using EPharmApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/[controller]")]
public class RegulatoryInformationController(IRegulatoryInformationService regulatoryInformationService, IPharmacyService pharmacyService) : ControllerBase
{
    [HttpGet("pharmacy")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    [RequirePharmacyId]
    public async Task<ActionResult<IEnumerable<GetRegulatoryInformationDto>>> GetAllCompanyRegulatoryInformation([FromQuery] int? pharmacyId = null)
    {
        if (User.IsInRole(IdentityData.Admin))
        {
            if (pharmacyId is null)
                return BadRequest("PharmacyId is required.");
            
            var pharmacy = await pharmacyService.GetPharmacyByIdAsync(pharmacyId.Value);

            if (pharmacy is null)
                return NotFound("Pharmacy not found.");
        }
        else
        {
            pharmacyId = (int)HttpContext.Items["PharmacyId"]!;
        }

        try
        {
            var result = await regulatoryInformationService.GetAllCompanyRegulatoryInformationAsync(pharmacyId.Value);
            if (result.Any()) return Ok(result);

            return NotFound("Regulatory information not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting regulatory information for pharmacy.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    [RequirePharmacyId]
    public async Task<ActionResult<GetRegulatoryInformationDto>> GetRegulatoryInformationById(int id, [FromQuery] int? pharmacyId = null)
    {
        if (User.IsInRole(IdentityData.Admin))
        {
            if (pharmacyId is null)
                return BadRequest("PharmacyId is required.");
            
            var pharmacy = await pharmacyService.GetPharmacyByIdAsync(pharmacyId.Value);

            if (pharmacy is null)
                return NotFound("Pharmacy not found.");
        }
        else
        {
            pharmacyId = (int)HttpContext.Items["PharmacyId"]!;
        }

        try
        {
            var result = await regulatoryInformationService.GetRegulatoryInformationByIdAsync(id);
            if (result is null)
                return NotFound("Regulatory information not found.");
            
            if (result.PharmacyId != pharmacyId.Value)
                return Forbid();
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting regulatory information.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    [RequirePharmacyId]
    public async Task<ActionResult<GetRegulatoryInformationDto>> CreateRegulatoryInformation([FromBody] CreateRegulatoryInformationDto regulatoryInformationDto, [FromQuery] int? pharmacyId = null)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        if (User.IsInRole(IdentityData.Admin))
        {
            if (pharmacyId is null)
                return BadRequest("PharmacyId is required.");
            
            var pharmacy = await pharmacyService.GetPharmacyByIdAsync(pharmacyId.Value);

            if (pharmacy is null)
                return NotFound("Pharmacy not found.");
        }
        else
        {
            pharmacyId = (int)HttpContext.Items["PharmacyId"]!;
        }

        try
        {
            var result = await regulatoryInformationService.AddCompanyRegulatoryInformationAsync(pharmacyId.Value, regulatoryInformationDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while creating regulatory information for pharmacy.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
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
            
            if (company.Owner.Id != userId)
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
            
            if (company.Owner.Id != userId)
                return Forbid();
        }
        
        var result = await regulatoryInformationService.DeleteCompanyRegulatoryInformationAsync(id);
        if (result) return Ok();

        return NotFound($"Regulatory information with ID: {id} not found.");
    }
}
