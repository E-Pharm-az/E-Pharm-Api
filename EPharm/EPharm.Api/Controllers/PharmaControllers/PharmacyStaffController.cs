using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers.PharmaControllers;

[ApiController]
[Route("api/[controller]/{pharmaCompanyId:int}")]
[Authorize(Roles = IdentityData.Admin + "," + IdentityData.PharmaCompanyAdmin)]
public class PharmacyStaffController(IPharmacyStaffService pharmacyStaffService, IPharmacyService pharmacyService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetPharmaCompanyManagerDto>>> GetAllPharmaCompanyManagers(int pharmaCompanyId)
    {
        var company = await pharmacyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            if (company.OwnerId != userId)
                return Forbid();
        }

        var result = await pharmacyStaffService.GetAllPharmaCompanyManagersAsync(pharmaCompanyId);

        if (result.Any()) return Ok(result);

        return NotFound("Pharmaceutical company managers not found.");
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetPharmaCompanyManagerDto>> GetPharmaCompanyManagerById(int pharmaCompanyId, int id)
    {
        var company = await pharmacyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            if (company.OwnerId != userId)
                return Forbid();
        }

        var result = await pharmacyStaffService.GetPharmaCompanyManagerByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Pharmaceutical company manager with ID: {id} not found.");
    }
    
    

    [HttpPost]
    [Route("register/{companyId:int}/pharma/")]
    [Authorize(Roles = IdentityData.PharmaCompanyAdmin)]
    public async Task<IActionResult> RegisterPharmaManager(int companyId, [FromBody] EmailDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await pharmacyStaffService.CreatePharmaManagerAsync(companyId, request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Error creating pharma manager, {Error}", ex.Message);
            return BadRequest("Error creating pharma manager");
        }
    }
}
