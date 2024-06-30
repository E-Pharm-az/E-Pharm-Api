using EPharm.Domain.Dtos.PharmacyStaffDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers.PharmaControllers;

[ApiController]
[Route("api/[controller]/{pharmacyId:int}")]
[Authorize(Roles = IdentityData.Admin + "," + IdentityData.PharmacyAdmin)]
public class PharmacyStaffController(IPharmacyStaffService pharmacyStaffService, IPharmacyService pharmacyService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetPharmacyStaffDto>>> GetAllPharmacyStaff(int pharmacyId)
    {
        var company = await pharmacyService.GetPharmacyByIdAsync(pharmacyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            if (company.Owner.Id != userId)
                return Forbid();
        }

        var result = await pharmacyStaffService.GetAllPharmacyStaffAsync(pharmacyId);

        if (result.Any()) return Ok(result);

        return NotFound("Pharmaceutical company managers not found.");
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetPharmacyStaffDto>> GetPharmaCompanyManagerById(int pharmacyId, int id)
    {
        var company = await pharmacyService.GetPharmacyByIdAsync(pharmacyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            if (company.Owner.Id != userId)
                return Forbid();
        }

        var result = await pharmacyStaffService.GetPharmacyStaffByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Pharmaceutical company manager with ID: {id} not found.");
    }
    
    

    [HttpPost]
    [Route("register/{companyId:int}/pharma/")]
    [Authorize(Roles = IdentityData.PharmacyAdmin)]
    public async Task<IActionResult> RegisterPharmaManager(int companyId, [FromBody] EmailDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await pharmacyStaffService.CreatePharmacyStaffAsync(companyId, request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Error creating pharma manager, {Error}", ex.Message);
            return BadRequest("Error creating pharma manager");
        }
    }
}
