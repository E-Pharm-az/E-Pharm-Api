using EPharm.Domain.Dtos.ManufacturerDto;
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
public class ManufacturersController(IManufacturerService manufacturerService, IPharmacyService pharmacyService) : ControllerBase
{
    // TODO: Add get all manufacturers without duplicates method (PUBLIC)
    
    [HttpGet("pharmacy")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    [RequirePharmacyId]
    public async Task<ActionResult<IEnumerable<GetManufacturerDto>>> GetAllPharmacyManufacturers([FromQuery] int? pharmacyId = null)
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
            var result = await manufacturerService.GetAllCompanyManufacturersAsync(pharmacyId.Value);

            if (result.Any())
                return Ok(result);

            return NotFound("Manufacturers not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting manufacturers for pharmacy.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    [RequirePharmacyId]
    public async Task<ActionResult<GetManufacturerDto>> GetManufacturerById(int id, [FromQuery] int? pharmacyId = null)
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
            var result = await manufacturerService.GetManufacturerByIdAsync(id);
            if (result is null)
                return NotFound("Manufacturer not found.");
            
            if (result.PharmacyId != pharmacyId.Value)
                return Forbid();
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting manufacturer.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    [RequirePharmacyId]
    public async Task<ActionResult<GetManufacturerDto>> CreateManufacturer([FromBody] CreateManufacturerDto manufacturerDto, [FromQuery] int? pharmacyId = null)
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
            var result = await manufacturerService.CreateManufacturerAsync(pharmacyId.Value, manufacturerDto);
            return Ok(result);
        }
        catch(Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while creating manufacturer.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    public async Task<ActionResult> UpdateManufacturer(int pharmaCompanyId, int id, [FromBody] CreateManufacturerDto manufacturerDto)
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

        var result = await manufacturerService.UpdateManufacturerAsync(id, manufacturerDto);
        
        if (result) return Ok("Manufacturer updated successfully.");

        Log.Error("Error updating manufacturer with ID: {Id}", id);
        return NotFound($"Error updating manufacturer with ID: {id}");
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    public async Task<ActionResult> DeleteManufacturer(int pharmaCompanyId, int id)
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
        
        var result = await manufacturerService.DeleteManufacturerAsync(id);
        if (result) return NoContent();

        return NotFound($"Manufacturer with ID: {id} not found.");
    }
}
