using EPharm.Domain.Interfaces;
using EPharm.Domain.Interfaces.Pharma;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]/pharma-company/{pharmaCompanyId:int}/image/{imageId}")]
[Authorize(Roles = IdentityData.Admin + "," + IdentityData.PharmaCompanyManager)]
public class ProductImageController(IPharmaCompanyService pharmaCompanyService, IProductImageService productImageService) : ControllerBase
{
    [HttpDelete]
    public async Task<ActionResult> DeleteProductImage(int pharmaCompanyId, string imageId)
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
        
        var result = await productImageService.DeleteProductImageAsync(imageId);
        if (result) return Ok();

        return BadRequest($"Product image with ID: {imageId} could not be deleted.");
    }
}
