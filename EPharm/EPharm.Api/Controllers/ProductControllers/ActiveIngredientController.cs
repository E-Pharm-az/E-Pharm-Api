using EPharm.Domain.Dtos.ActiveIngredientDto;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Domain.Models.Identity;
using EPharmApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/active-ingredient")]
public class ActiveIngredientController(IActiveIngredientService activeIngredientService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetActiveIngredientDto>>> GetAllActiveIngredients()
    {
        var result = await activeIngredientService.GetAllActiveIngredientsAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Active ingredients not found.");
    }
    
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    [HttpGet("pharmacy")]
    [RequirePharmacyId]
    public async Task<ActionResult<IEnumerable<GetActiveIngredientDto>>> GetAllPharmacyActiveIngredients()
    {
        var pharmacyId = (int)HttpContext.Items["PharmacyId"]!;

        try
        {
            var result = await activeIngredientService.GetAllCompanyActiveIngredientsAsync(pharmacyId);
            if (result.Any()) return Ok(result);

            return NotFound("Active ingredients not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting active ingredients for pharmacy.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpGet("{id:int}", Name = "getActiveIngredientById")]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    public async Task<ActionResult<GetActiveIngredientDto>> GetActiveIngredientById(int id)
    {
        var result = await activeIngredientService.GetActiveIngredientByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Active ingredient with ID: {id} not found.");
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    public async Task<ActionResult<GetActiveIngredientDto>> CreateActiveIngredient([FromBody] CreateActiveIngredientDto activeIngredientDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");
        
        var pharmacyId = (int)HttpContext.Items["PharmacyId"]!;

        try
        {
            var activeIngredient = await activeIngredientService.CreateActiveIngredientAsync(pharmacyId, activeIngredientDto);
            return CreatedAtRoute("getActiveIngredientById", new {id = activeIngredient.Id}, activeIngredient);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while creating active ingredient.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> UpdateActiveIngredient(int id, [FromBody] CreateActiveIngredientDto activeIngredientDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");
        
        var result = await activeIngredientService.UpdateActiveIngredientAsync(id, activeIngredientDto);
        if (result) return Ok();
        
        Log.Error("An unexpected error occurred while updating active ingredient.");
        return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> DeleteActiveIngredient(int id)
    {
        var result = await activeIngredientService.DeleteActiveIngredientAsync(id);
        if (result) return NoContent();

        Log.Error("An unexpected error occurred while deleting active ingredient.");
        return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
    }
}
