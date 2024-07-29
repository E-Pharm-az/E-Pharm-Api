using EPharm.Domain.Dtos.WarehouseDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using EPharmApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController(IWarehouseService warehouseService, IPharmacyService pharmacyService) : ControllerBase
{
    [HttpGet("pharmacy")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    [RequirePharmacyId]
    public async Task<ActionResult<IEnumerable<GetWarehouseDto>>> GetAllCompanyWarehouses([FromQuery] int? pharmacyId = null)
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
            var result = await warehouseService.GetAllCompanyWarehousesAsync(pharmacyId.Value);
            if (result.Any()) return Ok(result);

            return NotFound("Warehouses not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting warehouses for pharmacy.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }
    
    [HttpGet("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    [RequirePharmacyId]
    public async Task<ActionResult<GetWarehouseDto>> GetWarehouseById(int id, [FromQuery] int? pharmacyId = null)
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
            var result = await warehouseService.GetWarehouseByIdAsync(id);
            if (result is null)
                return NotFound($"Warehouse with ID: {id} not found.");
            
            if (result.PharmacyId != pharmacyId.Value)
                return Forbid();
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting warehouse.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }
    
    [HttpPost]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    [RequirePharmacyId]
    public async Task<ActionResult<GetWarehouseDto>> CreateWarehouse([FromBody] CreateWarehouseDto warehouseDto, [FromQuery] int? pharmacyId = null)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");
        
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
            var result = await warehouseService.CreateWarehouseAsync(pharmacyId.Value, warehouseDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while creating warehouse.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }
    
    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    public async Task<ActionResult> UpdateWarehouse(int pharmaCompanyId, int id, CreateWarehouseDto warehouseDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");
        
        var company = await pharmacyService.GetPharmacyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
        
        if (company.Owner.Id != userId)
            return Forbid();
        
        var result = await warehouseService.UpdateWarehouseAsync(id, warehouseDto);
        if (result) return Ok();
        
        return BadRequest($"Warehouse with ID: {id} could not be updated.");
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    public async Task<ActionResult> DeleteWarehouse(int pharmaCompanyId, int id)
    {
        var company = await pharmacyService.GetPharmacyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
         
        if (company.Owner.Id != userId)
            return Forbid();
        
        var result = await warehouseService.DeleteWarehouseAsync(id);
        if (result) return Ok();

        return BadRequest($"Warehouse with ID: {id} could not be deleted.");
    }
}
