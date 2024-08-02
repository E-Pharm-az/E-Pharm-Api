using EPharm.Domain.Dtos.SpecialRequirementsDto;
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
[Route("api/special-requirements")]
public class SpecialRequirementsController(ISpecialRequirementService specialRequirementService, IPharmacyService pharmacyService) : ControllerBase
{
    [HttpGet("pharmacy")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    [RequirePharmacyId]
    public async Task<ActionResult<IEnumerable<GetSpecialRequirementDto>>> GetAllPharmacySpecialRequirements([FromQuery] int? pharmacyId = null)
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
            var result = await specialRequirementService.GetAllPharmacySpecialRequirementsAsync(pharmacyId.Value);
            if (result.Any()) return Ok(result);

            return NotFound("Special requirements not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting special requirements for pharmacy.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    public async Task<ActionResult<GetSpecialRequirementDto>> GetSpecialRequirementById(int id, [FromQuery] int? pharmacyId = null)
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
            var result = await specialRequirementService.GetSpecialRequirementByIdAsync(id);
            if (result is null)
                return NotFound($"Special requirement with ID: {id} not found.");
            
            if (result.PharmacyId != pharmacyId.Value)
                return Forbid();
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting special requirement.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    [RequirePharmacyId]
    public async Task<ActionResult<GetSpecialRequirementDto>> CreateSpecialRequirement([FromBody] CreateSpecialRequirementDto specialRequirementDto, [FromQuery] int? pharmacyId = null)
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
            var result = await specialRequirementService.AddPharmacySpecialRequirementAsync(pharmacyId.Value, specialRequirementDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while creating special requirement for pharmacy.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    public async Task<ActionResult> UpdateSpecialRequirement(int pharmaCompanyId, int id, [FromBody] CreateSpecialRequirementDto specialRequirement)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var company = await pharmacyService.GetPharmacyByIdAsync(pharmaCompanyId);

        if (company is null)
            return NotFound("Pharmaceutical company not found.");

        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            
            if (company.Owner.Id != userId)
                return Forbid();
        }

        var result = await specialRequirementService.UpdatePharmacySpecialRequirementAsync(id, specialRequirement);
        if (result) return Ok();
        
        return BadRequest("Failed to update special requirement.");
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    public async Task<ActionResult> DeleteSpecialRequirement(int pharmaCompanyId, int id)
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
        
        var result = await specialRequirementService.DeletePharmacySpecialRequirementAsync(id);
        if (result) return Ok();
        
        return BadRequest("Failed to delete special requirement.");
    }
}
