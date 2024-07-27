using EPharm.Domain.Dtos.UsageWarningDto;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/[controller]")]
public class UsageWarningController(IUsageWarningService usageWarningService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetUsageWarningDto>>> GetAllUsageWarnings()
    {
        var result = await usageWarningService.GetAllUsageWarningsAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Usage warnings not found.");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetUsageWarningDto>> GetUsageWarningById(int id)
    {
        var result = await usageWarningService.GetUsageWarningByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Usage warning with ID: {id} not found.");
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<GetUsageWarningDto>> CreateUsageWarning(CreateUsageWarningDto usageWarningDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");

        try
        {
            var result = await usageWarningService.CreateUsageWarningAsync(usageWarningDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while creating usage warning.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> UpdateUsageWarning(int id, CreateUsageWarningDto usageWarningDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");
        
        var result = await usageWarningService.UpdateUsageWarningAsync(id, usageWarningDto);
        if (result) return Ok();
        
        return BadRequest($"Usage warning with ID: {id} could not be updated.");
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> DeleteUsageWarning(int id)
    {
        var result = await usageWarningService.DeleteUsageWarningAsync(id);
        if (result) return Ok();
        
        return BadRequest($"Usage warning with ID: {id} could not be deleted.");
    }
}
