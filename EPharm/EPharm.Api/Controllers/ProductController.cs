using EPharm.Domain.Dtos.ProductDtos.ProductDtos;
using EPharm.Domain.Interfaces.Product;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = IdentityData.PharmaCompanyManager)]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetProductDto>>> GetAllProducts()
    {
        var result = await productService.GetAllProductsAsync();
        if (result.Any()) return Ok(result);

        return NotFound("Products not found.");
    }

    [HttpGet("{id:int}", Name = "getProductById")]
    public async Task<ActionResult<GetProductDto>> GetProductById(int id)
    {
        var result = await productService.GetProductByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Product with ID: {id} not found.");
    }

    [HttpPost]
    public async Task<ActionResult<GetProductDto>> CreateProduct([FromBody] CreateProductDto productDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        try
        {
            var product = await productService.CreateProductAsync(productDto);
            return CreatedAtRoute("getProductById", new { productId = product.Id }, product);
        }
        catch (Exception ex)
        {
            Log.Error("Error creating product, {Error}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, [FromBody] CreateProductDto productDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var result = await productService.UpdateProductAsync(id, productDto);

        if (result) return Ok("Product updated with success.");

        Log.Error("Error updating product");
        return BadRequest("Error updating product.");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var result = await productService.DeleteProductAsync(id);

        if (result) return Ok($"Product with ID: {id} deleted with success.");
        
        Log.Error("Error deleting product");
        return BadRequest($"Product with ID: {id} could not be deleted.");
    }
}
