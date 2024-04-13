using EPharm.Domain.Dtos.SpecialRequirementsDto;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/[controller]/{pharmaCompanyId:int}/[controller]")]
public class SpecialRequirementController(ISpecialRequirementService specialRequirementService, IPharmaCompanyService pharmaCompanyService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = IdentityData.PharmaCompanyManager + "," + IdentityData.Admin)]
    public async Task<ActionResult<IEnumerable<GetSpecialRequirementDto>>> GetAllCompanySpecialRequirements(
        int pharmaCompanyId)
    {
        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);

        if (company is null)
            return NotFound("Pharmaceutical company not found.");

        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            
            if (company.OwnerId != userId)
                return Forbid();
        }

        var result = await specialRequirementService.GetAllCompanySpecialRequirementsAsync(pharmaCompanyId);
        if (result.Any()) return Ok(result);

        return NotFound("Special requirements not found.");
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager + "," + IdentityData.Admin)]
    public async Task<ActionResult<GetSpecialRequirementDto>> GetSpecialRequirementById(int pharmaCompanyId, int id)
    {
        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);

        if (company is null)
            return NotFound("Pharmaceutical company not found.");

        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            
            if (company.OwnerId != userId)
                return Forbid();
        }

        var result = await specialRequirementService.GetSpecialRequirementByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Special requirement with ID: {id} not found.");
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.PharmaCompanyManager)]
    public async Task<ActionResult<GetSpecialRequirementDto>> CreateSpecialRequirement(int pharmaCompanyId, [FromBody] CreateSpecialRequirementDto specialRequirementDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);

        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
        
        if (company.OwnerId != userId)
            return Forbid();

        try
        {
            var result = await specialRequirementService.AddCompanySpecialRequirementAsync(pharmaCompanyId, specialRequirementDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error("Error creating special requirement, {Error}", ex.Message);
            return BadRequest("Error creating special requirement.");
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager + "," + IdentityData.Admin)]
    public async Task<ActionResult> UpdateSpecialRequirement(int pharmaCompanyId, int id, [FromBody] CreateSpecialRequirementDto specialRequirement)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);

        if (company is null)
            return NotFound("Pharmaceutical company not found.");

        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            
            if (company.OwnerId != userId)
                return Forbid();
        }

        var result = await specialRequirementService.UpdateCompanySpecialRequirementAsync(id, specialRequirement);
        if (result) return Ok();
        
        return BadRequest("Failed to update special requirement.");
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager + "," + IdentityData.Admin)]
    public async Task<ActionResult> DeleteSpecialRequirement(int pharmaCompanyId, int id)
    {
        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);

        if (company is null)
            return NotFound("Pharmaceutical company not found.");

        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            
            if (company.OwnerId != userId)
                return Forbid();
        }
        
        var result = await specialRequirementService.DeleteCompanySpecialRequirementAsync(id);
        if (result) return Ok();
        
        return BadRequest("Failed to delete special requirement.");
    }
}
