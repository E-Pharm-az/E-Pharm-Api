using System.Security.Claims;
using EPharm.Domain.Dtos.OrderDto;
using EPharm.Domain.Interfaces;
using EPharm.Domain.Interfaces.Pharma;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPharmApi.Controllers;

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (company.PharmaCompanyOwnerId != userId)
                return Forbid();
        }
        
        var result = await orderService.GetOrderByIdAsync(id);
        if (result is not null) return Ok(result);

        return NotFound($"Order with ID: {id} not found.");
    }
    
    [HttpGet("user/{userId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<GetOrderDto>>> GetAllUserOrders(string userId)
    {
        if (!User.IsInRole(IdentityData.Admin) && User.FindFirstValue(ClaimTypes.NameIdentifier) != userId)
            return Forbid();
        
        var result = await orderService.GetAllUserOrders(userId);
        
        if (result.Any()) return Ok(result);
        
        return NotFound("Orders not found.");
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
