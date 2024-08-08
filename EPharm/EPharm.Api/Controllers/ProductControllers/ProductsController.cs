using EPharm.Domain.Dtos.ProductDtos;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Domain.Models.Identity;
using EPharmApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers.ProductControllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(
    IProductService productService,
    IPharmacyService pharmacyService
) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<IEnumerable<GetProductDto>>> GetAllProducts([FromQuery] int page)
    {
        try
        {
            var result = await productService.GetAllProductsAsync(page);
            if (result.Any()) return Ok(result);

            return NotFound("Products not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Internal server error occured");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }
    
    [HttpGet("{id:int}", Name = "getProductById")]
    public async Task<ActionResult<GetDetailProductDto>> GetProductById(int id)
    {
        try
        {
            var product = await productService.GetProductByIdAsync(id);
            
            if (product is null)
                return NotFound($"Product with ID: {id} not found.");

            if (product.IsApproved)
                return Ok(product);
            
            var user = HttpContext.User;

            if (user.IsInRole(IdentityData.Admin))
                return Ok(product);
        
            var pharmacyIdClaim = HttpContext.User.FindFirst("PharmacyId");
            if (pharmacyIdClaim == null || !int.TryParse(pharmacyIdClaim.Value, out var pharmacyId))
                return BadRequest("Invalid or missing PharmacyId");

            if (product.Pharmacy.Id != pharmacyId)
                return Forbid();

            return Ok(product);

        }
        catch (Exception ex)
        {
            Log.Error(ex, "Internal server error occurred.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<GetProductDto>>> SearchProduct([FromQuery] string query, [FromQuery] int page)
    {
        try
        {
            var result = await productService.SearchProduct(query, page);
            if (result.Any()) return Ok(result);

            return NotFound("Products not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Internal server error occurred");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpGet("pharmacy")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    [RequirePharmacyId]
    public async Task<ActionResult<IEnumerable<GetProductDto>>> GetAllPharmacyProducts([FromQuery] int page, [FromQuery] int? pharmacyId = null)
    {
        if (User.IsInRole(IdentityData.Admin))
        {
            if (pharmacyId is null)
                return BadRequest("PharmacyId is required.");
            
            var pharmacy = await pharmacyService.GetPharmacyByIdAsync(pharmacyId.Value);

            if (pharmacy is null)
                return NotFound("Pharmacy not found.");
        }
        else
        {
            pharmacyId = (int)HttpContext.Items["PharmacyId"]!;
        }
        
        try
        {
            var result = await productService.GetAllPharmaCompanyProductsAsync(pharmacyId.Value, page);
            if (result.Any()) return Ok(result);

            return NotFound("Pharma company products not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Internal server error occurred.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpPost("approve/{productId:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> ApproveProduct(int productId)
    {
        var adminId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
        
        try
        {
            await productService.ApproveProductAsync(adminId, productId);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error approving product with id: {Id}, by admin with id: {AdminId}.", productId, adminId);
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }
    
    [HttpPost]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    [RequirePharmacyId]
    public async Task<ActionResult<GetProductDto>> CreateProduct([FromForm] CreateProductDto productDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");
        
        var pharmacyId = (int)HttpContext.Items["PharmacyId"]!;

        try
        {
            var result = await productService.CreateProductAsync(pharmacyId, productDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating product.");
            return BadRequest("Error creating a product.");
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff)]
    [RequirePharmacyId]
    public async Task<ActionResult> UpdateProduct(int id, [FromBody] CreateProductDto productDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");
        
        var pharmacyId = (int)HttpContext.Items["PharmacyId"]!;

        var result = await productService.UpdateProductAsync(pharmacyId, id, productDto);

        if (result) return Ok("Product updated with success.");

        Log.Error("Error updating product");
        return BadRequest("Error updating product.");
    }

    [HttpDelete("{productId:int}")]
    [Authorize(Roles = IdentityData.PharmacyStaff + "," + IdentityData.Admin)]
    public async Task<ActionResult> DeleteProduct(int productId, [FromQuery] int? pharmacyId = null)
    {
        if (User.IsInRole(IdentityData.Admin))
        {
            if (pharmacyId is null)
                return BadRequest("PharmacyId is required.");
        }
        else
        {
            pharmacyId = (int)HttpContext.Items["PharmacyId"]!;
        }
        
        var result = await productService.DeleteProductAsync(pharmacyId.Value, productId);

        if (result) return NoContent();

        Log.Error("Error deleting product ID: {ProductId}", productId);
        return BadRequest($"Product with ID: {productId} could not be deleted.");
    }
}
