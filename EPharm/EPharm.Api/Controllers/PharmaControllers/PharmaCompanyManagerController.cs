using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Domain.Interfaces.Pharma;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EPharmApi.Controllers.PharmaControllers;

[ApiController]
[Route("api/[controller]/{pharmaCompanyId:int}")]
[Authorize(Roles = IdentityData.Admin + "," + IdentityData.PharmaCompanyAdmin)]
public class PharmaCompanyManagerController(IPharmaCompanyManagerService pharmaCompanyManagerService, IPharmaCompanyService pharmaCompanyService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetPharmaCompanyManagerDto>>> GetAllPharmaCompanyManagers(int pharmaCompanyId)
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

        var result = await pharmaCompanyManagerService.GetAllPharmaCompanyManagersAsync(pharmaCompanyId);

        if (result.Any()) return Ok(result);

        return NotFound("Pharmaceutical company managers not found.");
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetPharmaCompanyManagerDto>> GetPharmaCompanyManagerById(int pharmaCompanyId, int id)
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

        var result = await pharmaCompanyManagerService.GetPharmaCompanyManagerByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Pharmaceutical company manager with ID: {id} not found.");
    }
}
