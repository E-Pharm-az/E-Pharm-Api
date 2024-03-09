using EPharm.Domain.Dtos.ProductDtos.ActiveIngredientDto;
using EPharm.Domain.Interfaces.Pharma;
using EPharm.Domain.Interfaces.Product;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/active-ingredient")]
public class ActiveIngredientController(
    IActiveIngredientService activeIngredientService,
    IPharmaCompanyService pharmaCompanyService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetActiveIngredientDto>>> GetAllActiveIngredients()
    {
        var result = await activeIngredientService.GetAllActiveIngredientsAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Active ingredients not found.");
    }
    
    [Authorize(Roles = IdentityData.PharmaCompanyManager)]
    [HttpGet("pharma-company/{pharmaCompanyId:int}/active-ingredients")]
    public async Task<ActionResult<IEnumerable<GetActiveIngredientDto>>> GetAllCompanyActiveIngredients(int pharmaCompanyId)
    {
        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");

        var userId = User.FindFirst(JwtRegisteredClaimNames.Jti);
        
        if (company.PharmaCompanyOwnerId != userId.Value)
            return Forbid(); 
        
        var result = await activeIngredientService.GetAllCompanyActiveIngredientsAsync(pharmaCompanyId);
        if (result.Any()) return Ok(result);

        return NotFound("Active ingredients not found.");
    }

    [HttpGet("{id:int}", Name = "getActiveIngredientById")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager)]
    public async Task<ActionResult<GetActiveIngredientDto>> GetActiveIngredientById(int id)
    {
        var result = await activeIngredientService.GetActiveIngredientByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Active ingredient with ID: {id} not found.");
    }

    [HttpPost("pharma-company/{pharmaCompanyId:int}/active-ingredients")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager)]
    public async Task<ActionResult<GetActiveIngredientDto>> CreateActiveIngredient(int pharmaCompanyId, [FromBody] CreateActiveIngredientDto activeIngredientDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);
    
        if (company is null)
            return NotFound("Pharmaceutical company not found.");

        var userId = User.FindFirst(JwtRegisteredClaimNames.Jti);
    
        if (company.PharmaCompanyOwnerId != userId.Value)
            return Forbid();

        try
        {
            var activeIngredient = await activeIngredientService.CreateActiveIngredientAsync(pharmaCompanyId, activeIngredientDto);
            return CreatedAtRoute("getActiveIngredientById", new {id = activeIngredient.Id}, activeIngredient);
        }
        catch (Exception ex)
        {
            Log.Error("Error creating active ingredient, {Error}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("pharma-company/{pharmaCompanyId:int}/active-ingredients/{id:int}")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager)]
    public async Task<ActionResult> UpdateActiveIngredient(int pharmaCompanyId, int id, [FromBody] CreateActiveIngredientDto activeIngredientDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");
        
        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti);
            if (company.PharmaCompanyOwnerId != userId.Value)
                return Forbid();
        } 
        
        var result = await activeIngredientService.UpdateActiveIngredientAsync(id, activeIngredientDto);

        if (result) return Ok("Active ingredient updated with success.");

        Log.Error("Error updating active ingredient");
        return BadRequest("Error updating active ingredient.");
    }

    [HttpDelete("pharma-company/{pharmaCompanyId:int}/active-ingredients/{id:int}")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager)]
    public async Task<ActionResult> DeleteActiveIngredient(int pharmaCompanyId, int id)
    {
        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti);
            if (company.PharmaCompanyOwnerId != userId.Value)
                return Forbid();
        }  
        
        var result = await activeIngredientService.DeleteActiveIngredientAsync(id);

        if (result) return NoContent();

        Log.Error("Error deleting active ingredient");
        return BadRequest("Error deleting active ingredient.");
    }
}
