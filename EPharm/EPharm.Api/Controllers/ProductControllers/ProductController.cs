using EPharm.Domain.Dtos.ProductDtos;
using EPharm.Domain.Interfaces.Pharma;
using EPharm.Domain.Interfaces.Product;
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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetProductDto>>> GetAllProducts()
    {
        var result = await productService.GetAllProductsAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Products not found.");
    }

    [HttpGet("search/{parameter}")]
    public async Task<ActionResult<IEnumerable<GetProductDto>>> SearchProduct(string parameter)
    {
        var result = await productService.SearchProduct(parameter);
        if (result.Any()) return Ok(result);

        return NotFound("Products not found.");
    }

    [HttpGet("pharma-company/{pharmaCompanyId:int}/[controller]")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager + "," + IdentityData.Admin)]
    public async Task<ActionResult<IEnumerable<GetProductDto>>> GetAllPharmaCompanyProducts(int pharmaCompanyId)
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

        var result = await productService.GetAllPharmaCompanyProductsAsync(pharmaCompanyId);
        if (result.Any()) return Ok(result);

        return NotFound("Pharma company products not found.");
    }

    [HttpGet("{id:int}", Name = "getProductById")]
    public async Task<ActionResult<GetProductDto>> GetProductById(int id)
    {
        var result = await productService.GetProductByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Product with ID: {id} not found.");
    }

    [HttpPost("pharma-company/{pharmaCompanyId:int}/[controller]")]
    [Authorize(Roles = IdentityData.PharmaCompanyManager)]
    public async Task<ActionResult<GetProductDto>> CreateProduct(int pharmaCompanyId,
        [FromBody] CreateProductDto productDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var company = await pharmaCompanyService.GetPharmaCompanyByIdAsync(pharmaCompanyId);

        if (company is null)
            return NotFound("Pharmaceutical company not found.");

        var userId = User.FindFirst(JwtRegisteredClaimNames.Jti);

        if (company.PharmaCompanyOwnerId != userId.Value)
            return Forbid();

        try
        {
            var result = await productService.CreateProductAsync(pharmaCompanyId, productDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error("Error creating product, {Error}", ex.Message);
            return BadRequest(ex.Message);
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
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti);
            if (company.PharmaCompanyOwnerId != userId.Value)
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
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti);
            if (company.PharmaCompanyOwnerId != userId.Value)
                return Forbid();
        }

        var result = await productService.DeleteProductAsync(id);

        if (result) return NoContent();

        Log.Error("Error deleting product");
        return BadRequest($"Product with ID: {id} could not be deleted.");
    }
}
