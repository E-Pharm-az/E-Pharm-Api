using EPharm.Domain.Dtos.RouteOfAdministrationDto;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/roa")]
public class RouteOfAdministrationsController(IRouteOfAdministrationService routeOfAdministrationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetRouteOfAdministrationDto>>> GetAllRouteOfAdministrations()
    {
        var result = await routeOfAdministrationService.GetAllRouteOfAdministrationsAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Route of administrations not found.");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetRouteOfAdministrationDto>> GetRouteOfAdministrationById(int id)
    {
        var result = await routeOfAdministrationService.GetRouteOfAdministrationByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Route of administration with ID: {id} not found.");
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<GetRouteOfAdministrationDto>> CreateRouteOfAdministration(CreateRouteOfAdministrationDto routeOfAdministrationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");

        try
        {
            var result = await routeOfAdministrationService.CreateRouteOfAdministrationAsync(routeOfAdministrationDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while creating route of administration.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> UpdateRouteOfAdministration(int id, CreateRouteOfAdministrationDto routeOfAdministrationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");
        
        var result = await routeOfAdministrationService.UpdateRouteOfAdministrationAsync(id, routeOfAdministrationDto);
        if (result) return Ok();
        
        return BadRequest($"Route of administration with ID: {id} could not be updated.");
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> DeleteRouteOfAdministration(int id)
    {
        var result = await routeOfAdministrationService.DeleteRouteOfAdministrationAsync(id);
        if (result) return Ok();

        return BadRequest($"Route of administration with ID: {id} could not be deleted.");
    }
}
