using EPharm.Domain.Dtos.OrderDto;
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
[Authorize]
public class OrderController(IOrderService orderService, IPharmaCompanyService pharmaCompanyService) : Controller
{
    [HttpGet]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<IEnumerable<GetOrderDto>>> GetAllOrders()
    {
        var result = await orderService.GetAllOrders();
        if (result.Any()) return Ok(result);

        return NotFound("Orders not found.");
    }

    [HttpGet("pharma-company/{pharmaCompanyId:int}/{id:int}")]
    [Authorize(Roles = IdentityData.Admin + "," + IdentityData.PharmaCompanyManager)]
    public async Task<ActionResult<GetOrderDto>> GetOrderById(int pharmaCompanyId, int id)
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
        
        var result = await orderService.GetOrderByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Order with ID: {id} not found.");
    }

    [HttpPost]
    public async Task<ActionResult<GetOrderDto>> CreateOrder(CreateOrderDto orderDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");

        try
        {
            var result = await orderService.CreateOrderAsync(orderDto);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Error("Error creating order, {Error}", e.Message);
            return BadRequest($"Error creating order, {e.Message}");
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> UpdateOrder(int id, CreateOrderDto orderDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model not valid");
        
        var result = await orderService.UpdateOrderAsync(id, orderDto);
        if (result) return Ok();
        
        return BadRequest($"Order with ID: {id} could not be updated.");
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        var result = await orderService.DeleteOrderAsync(id);
        if (result) return Ok();
        
        return BadRequest($"Order with ID: {id} could not be deleted.");
    }
}
