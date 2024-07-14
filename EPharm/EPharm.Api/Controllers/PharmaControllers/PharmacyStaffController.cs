using EPharm.Domain.Dtos.PharmacyStaffDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using EPharmApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers.PharmaControllers;

[ApiController]
[Route("api/[controller]")]
public class PharmacyStaffController(IPharmacyStaffService pharmacyStaffService) : ControllerBase
{
    [HttpGet("{pharmacyId:int}")]
    [Authorize(Roles = IdentityData.PharmacyAdmin)]
    [PharmacyOwner]
    public async Task<ActionResult<IEnumerable<GetPharmacyStaffDto>>> GetAllPharmacyStaff(int pharmacyId)
    {
        var result = await pharmacyStaffService.GetAllAsync(pharmacyId);
        if (result.Any()) return Ok(result);

        return NotFound("Pharmaceutical company managers not found.");
    }
    
    // TODO
    [HttpGet("{pharmacyId:int}/{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyAdmin)]
    [PharmacyOwner]
    public async Task<ActionResult<GetPharmacyStaffDto>> GetPharmacyStaffById(int pharmacyId, int id)
    {
        var result = await pharmacyStaffService.GetByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Pharmaceutical company manager with ID: {id} not found.");
    }

    [HttpPost]
    [Route("{pharmacyId:int}/invite")]
    [Authorize(Roles = IdentityData.PharmacyAdmin)]
    [PharmacyOwner]
    public async Task<IActionResult> InvitePharmacyStaff(int pharmacyId, [FromBody] BulkEmailDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            await pharmacyStaffService.BulkInviteAsync(pharmacyId, request);
            return Ok(new { Message = "Pharmacy staff invited successfully." });
        }
        catch (ApplicationException ex) when (ex.Message == "FAILED_TO_SEND_INVITATION_EMAIL")
        {
            return StatusCode(500, new { Error = "Failed to send invitation email. Please try again later." });
        }
        catch (ApplicationException ex) when (ex.Message.StartsWith("Failed to invite staff member:"))
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while inviting pharmacy staff");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }    
    
    [HttpPost]
    [Route("register/{companyId:int}/pharma/")]
    [Authorize(Roles = IdentityData.PharmacyAdmin)]
    [PharmacyOwner]
    public async Task<IActionResult> RegisterPharmacyManager(int companyId, [FromBody] EmailDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await pharmacyStaffService.CreateAsync(companyId, request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Error creating pharma manager, {Error}", ex.Message);
            return BadRequest("Error creating pharma manager");
        }
    }
}
