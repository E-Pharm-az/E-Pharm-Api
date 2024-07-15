using System.Security.Claims;
using EPharm.Domain.Dtos.OrderDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController(IOrderService orderService, IPharmacyService pharmacyService) : ControllerBase
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
    [Authorize(Roles = IdentityData.Admin + "," + IdentityData.PharmacyStaff)]
    public async Task<ActionResult<GetOrderDto>> GetOrderById(int pharmaCompanyId, int id)
    {
        var company = await pharmacyService.GetPharmacyByIdAsync(pharmaCompanyId);
        
        if (company is null)
            return NotFound("Pharmaceutical company not found.");
        
        if (!User.IsInRole(IdentityData.Admin))
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (company.Owner.Id != userId)
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

    [HttpPost]
    public async Task<ActionResult<GetOrderDto>> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { Error = "Model not valid" });

        try
        {
            var result = await orderService.CreateOrderAsync(orderDto);
            return Ok(result);
        }
        catch (ArgumentException ex) when (ex.Message == "MISSING_EMAIL_FOR_ORDER")
        {
            return BadRequest(new { Error = "Email is required for the order." });
        }
        catch (ArgumentException ex) when (ex.Message == "PRODUCT_NOT_FOUND")
        {
            return NotFound(new { Error = "One or more products in the order were not found." });
        }
        catch (ArgumentException ex) when (ex.Message == "STOCK_NOT_ENOUGH")
        {
            return BadRequest(new { Error = "Insufficient stock for one or more products." });
        }
        catch (ArgumentException ex) when (ex.Message == "FAILED_TO_CREATE_PAYPAL_ORDER")
        {
            return BadRequest(new { Error = "Failed to create PayPal order." });
        }
        catch (Exception ex)
        {
            Log.Error("An error occurred while creating order. Details: {@ex}", ex);
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpPost("{orderId}")]
    public async Task<IActionResult> CaptureOrder(string orderId)
    {
        try
        {
            await orderService.CaptureOrderAsync(orderId);
            return Ok(new { Message = "Order captured successfully." });
        }
        catch (ArgumentException ex) when (ex.Message == "ORDER_NOT_FOUND")
        {
            return NotFound(new { Error = "Order not found." });
        }
        catch (ArgumentException ex) when (ex.Message == "PRODUCT_NOT_FOUND")
        {
            return NotFound(new { Error = "Product not found." });
        }
        catch (ArgumentException ex) when (ex.Message == "STOCK_NOT_ENOUGH")
        {
            return BadRequest(new { Error = "Insufficient stock for one or more products." });
        }
        catch (ArgumentException ex) when (ex.Message == "FAILED_TO_CAPTURE_PAYPAL_ORDER")
        {
            return BadRequest(new { Error = "Failed to capture PayPal order." });
        }
        catch (Exception ex)
        {
            Log.Error("An error occurred while capturing order. Details: {@ex}", ex);
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
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
