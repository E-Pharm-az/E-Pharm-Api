using EPharm.Domain.Dtos.DosageFormDto;
using EPharm.Domain.Interfaces.Product;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/[controller]")]
public class DosageFormController(IDosageFormService dosageFormService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetDosageFormDto>>> GetAllDosageForms()
    {
        var result = await dosageFormService.GetAllDosageFormsAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Dosage forms not found.");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetDosageFormDto>> GetDosageFormById(int id)
    {
        var result = await dosageFormService.GetDosageFormByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Dosage form with ID: {id} not found.");
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<GetDosageFormDto>> CreateDosageForm(CreateDosageFormDto dosageFormDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");

        try
        {
            var result = await dosageFormService.CreateDosageFormAsync(dosageFormDto);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Error("Error creating dosage form, {Error}", e.Message);
            return BadRequest($"Error creating dosage form, {e.Message}");
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> UpdateDosageForm(int id, CreateDosageFormDto dosageFormDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");
        
        var result = await dosageFormService.UpdateDosageFormAsync(id, dosageFormDto);
        if (result) return Ok();
        
        return BadRequest($"Dosage form with ID: {id} could not be updated.");
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> DeleteDosageForm(int id)
    {
        var result = await dosageFormService.DeleteDosageFormAsync(id);
        if (result) return Ok();
        
        return BadRequest($"Dosage form with ID: {id} could not be deleted.");
    }
}
