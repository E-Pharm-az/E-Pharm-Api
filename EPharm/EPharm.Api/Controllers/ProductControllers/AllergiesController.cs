using EPharm.Domain.Dtos.AttributeDtos;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/[controller]")]
public class AllergiesController(IAllergyService allergyService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAttributeDto>>> GetAllAllergies()
    {
        var result = await allergyService.GetAllAllergiesAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Allergies not found.");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetAttributeDto>> GetAllergyById(int id)
    {
        var result = await allergyService.GetAllergyByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Allergy with ID: {id} not found.");
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<GetAttributeDto>> CreateAllergy(CreateAttributeDto allergyDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");

        try
        {
            var result = await allergyService.CreateAllergyAsync(allergyDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating allergy.");
            return BadRequest("Error creating allergy.");
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> UpdateAllergy(int id, CreateAttributeDto allergyDto)
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
