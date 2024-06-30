using EPharm.Domain.Dtos.ActiveIngredientDto;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/active-ingredient")]
public class ActiveIngredientController(IActiveIngredientService activeIngredientService, IPharmacyService pharmacyService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetActiveIngredientDto>>> GetAllActiveIngredients()
    {
        var result = await activeIngredientService.GetAllActiveIngredientsAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Active ingredients not found.");
    }
    
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    [HttpGet("pharma-company/{pharmaCompanyId:int}/active-ingredients")]
    public async Task<ActionResult<IEnumerable<GetActiveIngredientDto>>> GetAllCompanyActiveIngredients(int pharmaCompanyId)
    {
        var company = await pharmacyService.GetPharmacyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");

        var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
        
        if (company.Owner.Id != userId)
            return Forbid(); 
        
        var result = await activeIngredientService.GetAllCompanyActiveIngredientsAsync(pharmaCompanyId);
        
        if (result.Any()) return Ok(result);

        return NotFound("Active ingredients not found.");
    }

    [HttpGet("{id:int}", Name = "getActiveIngredientById")]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    public async Task<ActionResult<GetActiveIngredientDto>> GetActiveIngredientById(int id)
    {
        var result = await activeIngredientService.GetActiveIngredientByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Active ingredient with ID: {id} not found.");
    }

    [HttpPost("pharma-company/{pharmaCompanyId:int}/active-ingredients")]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    public async Task<ActionResult<GetActiveIngredientDto>> CreateActiveIngredient(int pharmaCompanyId, [FromBody] CreateActiveIngredientDto activeIngredientDto)
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
            var activeIngredient = await activeIngredientService.CreateActiveIngredientAsync(pharmaCompanyId, activeIngredientDto);
            return CreatedAtRoute("getActiveIngredientById", new {id = activeIngredient.Id}, activeIngredient);
        }
        catch (Exception ex)
        {
            Log.Error("Error creating active ingredient, {Error}", ex.Message);
            return BadRequest("Error creating active ingredient.");
        }
    }

    [HttpPut("pharma-company/{pharmaCompanyId:int}/active-ingredients/{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    public async Task<ActionResult> UpdateActiveIngredient(int pharmaCompanyId, int id, [FromBody] CreateActiveIngredientDto activeIngredientDto)
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
        
        var result = await activeIngredientService.UpdateActiveIngredientAsync(id, activeIngredientDto);

        if (result) return Ok("Active ingredient updated with success.");

        Log.Error("Error updating active ingredient");
        return BadRequest("Error updating active ingredient.");
    }

    [HttpDelete("pharma-company/{pharmaCompanyId:int}/active-ingredients/{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    public async Task<ActionResult> DeleteActiveIngredient(int pharmaCompanyId, int id)
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
        
        var result = await activeIngredientService.DeleteActiveIngredientAsync(id);

        if (result) return NoContent();

        Log.Error("Error deleting active ingredient");
        return BadRequest("Error deleting active ingredient.");
    }
}
