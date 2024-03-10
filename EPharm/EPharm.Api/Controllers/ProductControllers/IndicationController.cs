using EPharm.Domain.Dtos.ProductDtos.IndicationDto;
using EPharm.Domain.Interfaces.Product;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/[controller]")]
public class IndicationController(IIndicationService indicationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetIndicationDto>>> GetAllIndications()
    {
        var result = await indicationService.GetAllIndicationsAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Indications not found.");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetIndicationDto>> GetIndicationById(int id)
    {
        var result = await indicationService.GetIndicationByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Indication with ID: {id} not found.");
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<GetIndicationDto>> CreateIndication(CreateIndicationDto indicationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");

        try
        {
            var result = await indicationService.CreateIndicationAsync(indicationDto);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Error("Error creating indication, {Error}", e.Message);
            return BadRequest($"Error creating indication, {e.Message}");
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> UpdateIndication(int id, CreateIndicationDto indicationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");
        
        var result = await indicationService.UpdateIndicationAsync(id, indicationDto);
        if (result) return Ok();
        
        return BadRequest($"Indication with ID: {id} could not be updated.");
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> DeleteIndication(int id)
    {
        var result = await indicationService.DeleteIndicationAsync(id);
        if (result) return Ok();
        
        return BadRequest($"Indication with ID: {id} could not be deleted.");
    }
}
