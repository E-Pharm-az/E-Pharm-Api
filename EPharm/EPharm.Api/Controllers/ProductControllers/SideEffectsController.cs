using EPharm.Domain.Dtos.AttributeDtos;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/side-effects")]
public class SideEffectsController(ISideEffectService sideEffectService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAttributeDto>>> GetAllSideEffects()
    {
        var result = await sideEffectService.GetAllSideEffectsAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Side effects not found.");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetAttributeDto>> GetSideEffectById(int id)
    {
        var result = await sideEffectService.GetSideEffectByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Side effect with ID: {id} not found.");
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<GetAttributeDto>> CreateSideEffect(CreateAttributeDto sideEffectDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");

        try
        {
            var result = await sideEffectService.CreateSideEffectAsync(sideEffectDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating side effect.");
            return BadRequest("Error creating side effect.");
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> UpdateSideEffect(int id, CreateAttributeDto sideEffectDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");
        
        var result = await sideEffectService.UpdateSideEffectAsync(id, sideEffectDto);
        if (result) return Ok();
        
        return BadRequest($"Side effect with ID: {id} could not be updated.");
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> DeleteSideEffect(int id)
    {
        var result = await sideEffectService.DeleteSideEffectAsync(id);
        if (result) return Ok();
        
        return BadRequest($"Side effect with ID: {id} could not be deleted.");
    }
}
