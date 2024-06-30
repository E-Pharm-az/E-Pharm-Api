using EPharm.Domain.Dtos.WarehouseDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/[controller]/{pharmaCompanyId:int}/[controller]")]
public class WarehouseController(IWarehouseService warehouseService, IPharmacyService pharmacyService) : Controller
{
    [HttpGet]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    public async Task<ActionResult<IEnumerable<GetWarehouseDto>>> GetAllCompanyWarehouses(int pharmaCompanyId)
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
        
        var result = await warehouseService.GetAllCompanyWarehousesAsync(pharmaCompanyId);
        
        if (result.Any()) return Ok(result);

        return NotFound("Warehouses not found.");
    }
    
    [HttpGet("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    public async Task<ActionResult<GetWarehouseDto>> GetWarehouseById(int pharmaCompanyId, int id)
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
        
        var result = await warehouseService.GetWarehouseByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Warehouse with ID: {id} not found.");
    }
    
    [HttpPost]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    public async Task<ActionResult<GetWarehouseDto>> CreateWarehouse(int pharmaCompanyId, CreateWarehouseDto warehouseDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");
        
        var company = await pharmacyService.GetPharmacyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
        
        if (company.Owner.Id != userId)
            return Forbid();

        try
        {
            var result = await warehouseService.CreateWarehouseAsync(pharmaCompanyId, warehouseDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error("Error creating warehouse, {Error}", ex.Message);
            return BadRequest("Error creating warehouse.");
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
