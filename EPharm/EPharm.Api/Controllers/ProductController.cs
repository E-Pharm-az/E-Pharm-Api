using EPharm.Domain.Dtos.ProductDtos.ProductDtos;
using EPharm.Domain.Interfaces.Product;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("{productId:int}", Name = "getProductById")]
    public async Task<ActionResult<GetProductDto>> GetProductById(int productId)
    {
        var result = await productService.GetProductByIdAsync(productId);
        if (result is not null) return Ok(result);

        return NotFound($"Product with ID: {productId} not found.");
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
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateProduct([FromBody] CreateProductDto productDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid.");

        var result = await productService.UpdateProductAsync(productDto);

        if (result) return Ok("Product updated with success.");

        return BadRequest("Error updating product.");
    }

    [HttpDelete("{productId:int}")]
    public async Task<ActionResult> DeleteProduct(int productId)
    {
        var result = await productService.DeleteProductAsync(productId);

        if (result) return Ok($"Product with ID: {productId} deleted with success.");

        return BadRequest($"Product with ID: {productId} could not be deleted.");
    }
}
