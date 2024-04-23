using EPharm.Domain.Dtos.ProductDtos;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService, IPharmaCompanyService pharmaCompanyService) : ControllerBase
{
    [HttpGet("all/{page:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<IEnumerable<GetMinimalProductDto>>> GetAllProducts(int page)
    {
        try
        {
            var result = await productService.GetAllProductsAsync(page);
            if (result.Any()) return Ok(result);

            return NotFound("Products not found.");
        }
        catch (Exception ex)
        {
            Log.Error("Internal server error occured. Detail: {error}", ex);
            return StatusCode(500, "Internal server error occured.");
        }
    }

    [HttpGet("search/{parameter}/{page:int}")]
    public async Task<ActionResult<IEnumerable<GetMinimalProductDto>>> SearchProduct(string parameter, int page)
    {
        try
        {
            var result = await productService.SearchProduct(parameter, page);
            if (result.Any()) return Ok(result);

            return NotFound("Products not found.");
        }
        catch (Exception ex)
        {
            Log.Error("Internal server error occurred. Detail: {error}", ex);
            return StatusCode(500, "Internal server error occurred.");
        }
    }

    [HttpGet("pharma-company/{pharmaCompanyId:int}/[controller]/{page:int}")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager + "," + IdentityData.Admin)]
    public async Task<ActionResult<IEnumerable<GetMinimalProductDto>>> GetAllPharmaCompanyProducts(int pharmaCompanyId, int page)
    {
        try
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

            var result = await productService.GetAllPharmaCompanyProductsAsync(pharmaCompanyId, page);
            if (result.Any()) return Ok(result);

            return NotFound("Pharma company products not found.");
        }
        catch (Exception ex)
        {
            Log.Error("Internal server error occurred. Detail: {error}", ex);
            return StatusCode(500, "Internal server error occurred.");
        }
    }

    [HttpGet("{id:int}", Name = "getProductById")]
    public async Task<ActionResult<GetMinimalProductDto>> GetProductById(int id)
    {
        try
        {
            var result = await productService.GetProductByIdAsync(id);
            if (result is not null) return Ok(result);

            return NotFound($"Product with ID: {id} not found.");
        }
        catch (Exception ex)
        {
            Log.Error("Internal server error occurred. Detail: {error}", ex);
            return StatusCode(500, "Internal server error occurred.");
        }
    }
    
    [HttpPost("pharma-company/{pharmaCompanyId:int}/[controller]")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager)]
    public async Task<ActionResult<GetMinimalProductDto>> CreateProduct(int pharmaCompanyId, [FromBody] CreateProductDto productDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        try
        {
            var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);

            if (company is null)
                return NotFound("Pharmaceutical company not found.");

            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;

            if (company.OwnerId != userId)
                return Forbid();

            var result = await productService.CreateProductAsync(pharmaCompanyId, productDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error("Error creating product, {Error}", ex.Message);
            return BadRequest("Error creating a product.");
        }
    }

    [HttpPut("pharma-company/{pharmaCompanyId:int}/[controller]/{id:int}")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager + "," + IdentityData.Admin)]
    public async Task<ActionResult> UpdateProduct(int pharmaCompanyId, int id, [FromBody] CreateProductDto productDto)
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

        var result = await productService.UpdateProductAsync(id, productDto);

        if (result) return Ok("Product updated with success.");

        Log.Error("Error updating product");
        return BadRequest("Error updating product.");
    }

    [HttpDelete("pharma-company/{pharmaCompanyId:int}/[Controller]/{id:int}")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager + "," + IdentityData.Admin)]
    public async Task<ActionResult> DeleteProduct(int pharmaCompanyId, int id)
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

        var result = await productService.DeleteProductAsync(id);

        if (result) return NoContent();

        Log.Error("Error deleting product");
        return BadRequest($"Product with ID: {id} could not be deleted.");
    }
}
