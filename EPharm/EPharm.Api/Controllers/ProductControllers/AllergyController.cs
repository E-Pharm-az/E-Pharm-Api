using EPharm.Domain.Dtos.ProductDtos.AllergyDto;
using EPharm.Domain.Interfaces.Product;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/[controller]")]
public class AllergyController(IAllergyService allergyService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAllergyDto>>> GetAllAllergies()
    {
        var result = await allergyService.GetAllAllergiesAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Allergies not found.");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetAllergyDto>> GetAllergyById(int id)
    {
        var result = await allergyService.GetAllergyByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Allergy with ID: {id} not found.");
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<GetAllergyDto>> CreateAllergy(CreateAllergyDto allergyDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");

        try
        {
            var result = await allergyService.CreateAllergyAsync(allergyDto);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Error("Error creating allergy, {Error}", e.Message);
            return BadRequest($"Error creating allergy, {e.Message}");
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> UpdateAllergy(int id, CreateAllergyDto allergyDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");
        
        var result = await allergyService.UpdateAllergyAsync(id, allergyDto);
        if (result) return Ok();
        
        return BadRequest($"Allergy with ID: {id} could not be updated.");
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> DeleteAllergy(int id)
    {
        var result = await allergyService.DeleteAllergyAsync(id);
        if (result) return Ok();

        return BadRequest($"Allergy with ID: {id} could not be deleted.");
    }
}    
