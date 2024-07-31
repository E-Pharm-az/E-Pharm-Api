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
public class ActiveIngredientsController(IActiveIngredientsService activeIngredientsService, IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetActiveIngredientDto>>> GetAllActiveIngredients()
    {
        try
        {
            var result = await activeIngredientsService.GetAllActiveIngredientsAsync();
            if (result.Any()) return Ok(result);

            return NotFound("Active ingredients not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting active ingredients by id.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }
    
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    [RequirePharmacyId]
    [HttpGet("pharmacy")]
    public async Task<ActionResult<IEnumerable<GetActiveIngredientDto>>> GetAllPharmacyActiveIngredients()
    {
        var pharmacyId = (int)HttpContext.Items["PharmacyId"]!;

        try
        {
            var result = await activeIngredientsService.GetAllCompanyActiveIngredientsAsync(pharmacyId);
            if (result.Any()) return Ok(result);

            return NotFound("Active ingredients not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting active ingredients for pharmacy.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }
    
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    [RequirePharmacyId]
    [HttpGet("product/{id:int}")]
    public async Task<ActionResult<IEnumerable<GetProductActiveIngredientDto>>> GetAllProductActiveIngredients(int id)
    {
        var pharmacyId = (int)HttpContext.Items["PharmacyId"]!;
        
        try
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product is null)
                return NotFound("Product not found.");
            
            if (product.PharmacyId != pharmacyId)
                return Forbid();
            
            var result = await activeIngredientsService.GetAllCompanyActiveIngredientsAsync(id);
            if (result.Any()) return Ok(result);

            return NotFound("Active ingredients not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting active ingredients for pharmacy.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
 
    }

    [Authorize(Roles = IdentityData.PharmacyStaff)]
    [HttpGet("{id:int}", Name = "getActiveIngredientById")]
    public async Task<ActionResult<GetActiveIngredientDto>> GetActiveIngredientById(int id)
    {
        var result = await activeIngredientsService.GetActiveIngredientByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Active ingredient with ID: {id} not found.");
    }

    [Authorize(Roles = IdentityData.PharmacyStaff)]
    [RequirePharmacyId]
    [HttpPost]
    public async Task<ActionResult<GetActiveIngredientDto>> CreateActiveIngredient([FromBody] CreateActiveIngredientDto activeIngredientDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");
        
        var pharmacyId = (int)HttpContext.Items["PharmacyId"]!;

        try
        {
            var activeIngredient = await activeIngredientsService.CreateActiveIngredientAsync(pharmacyId, activeIngredientDto);
            return CreatedAtRoute("getActiveIngredientById", new {id = activeIngredient.Id}, activeIngredient);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while creating active ingredient.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [Authorize(Roles = IdentityData.Admin)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateActiveIngredient(int id, [FromBody] CreateActiveIngredientDto activeIngredientDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");
        
        var result = await activeIngredientsService.UpdateActiveIngredientAsync(id, activeIngredientDto);
        if (result) return Ok();
        
        Log.Error("An unexpected error occurred while updating active ingredient.");
        return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
    }

    [Authorize(Roles = IdentityData.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteActiveIngredient(int id)
    {
        var result = await activeIngredientsService.DeleteActiveIngredientAsync(id);
        if (result) return NoContent();

        Log.Error("An unexpected error occurred while deleting active ingredient.");
        return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
    }
}
