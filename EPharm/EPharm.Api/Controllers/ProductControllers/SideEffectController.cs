using EPharm.Domain.Dtos.SideEffectDto;
using EPharm.Domain.Interfaces.Product;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/[controller]")]
public class SideEffectController(ISideEffectService sideEffectService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetSideEffectDto>>> GetAllSideEffects()
    {
        var result = await sideEffectService.GetAllSideEffectsAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Side effects not found.");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetSideEffectDto>> GetSideEffectById(int id)
    {
        var result = await sideEffectService.GetSideEffectByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Side effect with ID: {id} not found.");
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<GetSideEffectDto>> CreateSideEffect(CreateSideEffectDto sideEffectDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");

        try
        {
            var result = await sideEffectService.CreateSideEffectAsync(sideEffectDto);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Error("Error creating side effect, {Error}", e.Message);
            return BadRequest($"Error creating side effect, {e.Message}");
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> UpdateSideEffect(int id, CreateSideEffectDto sideEffectDto)
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
