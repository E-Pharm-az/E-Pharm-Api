using EPharm.Domain.Dtos.RouteOfAdministrationDto;
using EPharm.Domain.Interfaces.Product;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/[controller]")]
public class RouteOfAdministrationController(IRouteOfAdministrationService routeOfAdministrationService) : ControllerBase
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
        catch (Exception e)
        {
            Log.Error("Error creating route of administration, {Error}", e.Message);
            return BadRequest($"Error creating route of administration, {e.Message}");
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
