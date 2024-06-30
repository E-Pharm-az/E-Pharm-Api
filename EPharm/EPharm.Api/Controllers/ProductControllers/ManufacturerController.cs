using EPharm.Domain.Dtos.ManufacturerDto;
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
public class ManufacturerController(IManufacturerService manufacturerService, IPharmacyService pharmacyService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    public async Task<ActionResult<IEnumerable<GetManufacturerDto>>> GetAllCompanyManufacturers(int pharmaCompanyId)
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
        
        var result = await manufacturerService.GetAllCompanyManufacturersAsync(pharmaCompanyId);
        
        if (result.Any())
            return Ok(result);

        return NotFound("Manufacturers not found.");
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    public async Task<ActionResult<GetManufacturerDto>> GetManufacturerById(int pharmaCompanyId, int id)
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
        
        var result = await manufacturerService.GetManufacturerByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Manufacturer with ID: {id} not found.");
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    public async Task<ActionResult<GetManufacturerDto>> CreateManufacturer(int pharmaCompanyId, [FromBody] CreateManufacturerDto manufacturerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");
        
        var company = await pharmacyService.GetPharmacyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
        
        if (company.Owner.Id != userId)
            return Forbid();

        try
        {
            var result = await manufacturerService.CreateManufacturerAsync(pharmaCompanyId, manufacturerDto);
            return Ok(result);
        }
        catch(Exception ex)
        {
            Log.Error("Error creating manufacturer, {Error}", ex.Message);
            return BadRequest("Error creating manufacturer.");
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
