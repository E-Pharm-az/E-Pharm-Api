using EPharm.Domain.Dtos.ProductDtos.ActiveIngredientDto;
using EPharm.Domain.Interfaces.Product;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = IdentityData.PharmaCompanyManager)]
public class ActiveIngredientController(IActiveIngredientService activeIngredientService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetActiveIngredientDto>>> GetAllActiveIngredients()
    {
        var result = await activeIngredientService.GetAllActiveIngredientsAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Active ingredients not found.");
    }
    
    [HttpGet("{id:int}", Name = "getActiveIngredientById")]
    public async Task<ActionResult<GetActiveIngredientDto>> GetActiveIngredientById(int id)
    {
        var result = await activeIngredientService.GetActiveIngredientByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Active ingredient with ID: {id} not found.");
    }
    
    [HttpPost]
    public async Task<ActionResult<GetActiveIngredientDto>> CreateActiveIngredient([FromBody] CreateActiveIngredientDto activeIngredientDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        try
        {
            var activeIngredient = await activeIngredientService.CreateActiveIngredientAsync(activeIngredientDto);
            return CreatedAtRoute("getActiveIngredientById", new { activeIngredientId = activeIngredient.Id }, activeIngredient);
        }
        catch (Exception ex)
        {
            Log.Error("Error creating active ingredient, {Error}", ex.Message);
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateActiveIngredient(int id, [FromBody] CreateActiveIngredientDto activeIngredientDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var result = await activeIngredientService.UpdateActiveIngredientAsync(id, activeIngredientDto);

        if (result) return Ok("Active ingredient updated with success.");
        
        Log.Error("Error updating active ingredient");
        return BadRequest("Error updating active ingredient.");
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteActiveIngredient(int id)
    {
        var result = await activeIngredientService.DeleteActiveIngredientAsync(id);

        if (result) return Ok("Active ingredient deleted with success.");
        
        Log.Error("Error deleting active ingredient");
        return BadRequest("Error deleting active ingredient.");
    }
}
