using EPharm.Domain.Dtos.RegulatoryInformationDto;
using EPharm.Domain.Interfaces.Pharma;
using EPharm.Domain.Interfaces.Product;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/[controller]/{pharmaCompanyId:int}/[controller]")]
public class RegulatoryInformationController(IRegulatoryInformationService regulatoryInformationService, IPharmaCompanyService pharmaCompanyService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = IdentityData.PharmaCompanyManager + "," + IdentityData.Admin)]
    public async Task<ActionResult<IEnumerable<GetRegulatoryInformationDto>>> GetAllCompanyRegulatoryInformation(int pharmaCompanyId)
    {
        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti);
            if (company.PharmaCompanyOwnerId != userId.Value)
                return Forbid();
        }
        
        var result = await regulatoryInformationService.GetAllCompanyRegulatoryInformationAsync(pharmaCompanyId);
        if (result.Any()) return Ok(result);

        return NotFound("Regulatory information not found.");
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager + "," + IdentityData.Admin)]
    public async Task<ActionResult<GetRegulatoryInformationDto>> GetRegulatoryInformationById(int pharmaCompanyId, int id)
    {
        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti);
            if (company.PharmaCompanyOwnerId != userId.Value)
                return Forbid();
        }
        
        var result = await regulatoryInformationService.GetRegulatoryInformationByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Regulatory information with ID: {id} not found.");
    }

    [HttpPost]
    [Authorize(Roles = IdentityData.PharmaCompanyManager)]
    public async Task<ActionResult<GetRegulatoryInformationDto>> CreateRegulatoryInformation(
        int pharmaCompanyId, [FromBody] CreateRegulatoryInformationDto regulatoryInformationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);

        if (company is null)
            return NotFound("Pharmaceutical company not found.");

        try
        {
            var result = await regulatoryInformationService.AddCompanyRegulatoryInformationAsync(pharmaCompanyId, regulatoryInformationDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager + "," + IdentityData.Admin)]
    public async Task<ActionResult> UpdateRegulatoryInformation(int pharmaCompanyId, int id, [FromBody] CreateRegulatoryInformationDto regulatoryInformation)
    {
        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti);
            if (company.PharmaCompanyOwnerId != userId.Value)
                return Forbid();
        }
        
        var result = await regulatoryInformationService.UpdateCompanyRegulatoryInformationAsync(id, regulatoryInformation);
        if (result) return Ok();

        return NotFound($"Regulatory information with ID: {id} not found.");
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager + "," + IdentityData.Admin)]
    public async Task<ActionResult> DeleteRegulatoryInformation(int pharmaCompanyId, int id)
    {
        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti);
            if (company.PharmaCompanyOwnerId != userId.Value)
                return Forbid();
        }
        
        var result = await regulatoryInformationService.DeleteCompanyRegulatoryInformationAsync(id);
        if (result) return Ok();

        return NotFound($"Regulatory information with ID: {id} not found.");
    }
}