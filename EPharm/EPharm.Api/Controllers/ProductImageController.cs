using System.Security.Claims;
using EPharm.Domain.Interfaces;
using EPharm.Domain.Interfaces.Pharma;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (company.PharmaCompanyOwnerId != userId)
                return Forbid();
        } 
        
        var result = await productImageService.DeleteProductImageAsync(imageId);
        if (result) return Ok();

        return BadRequest($"Product image with ID: {imageId} could not be deleted.");
    }
}
