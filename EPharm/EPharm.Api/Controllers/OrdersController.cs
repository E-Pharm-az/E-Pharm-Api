using EPharm.Domain.Dtos.OrderDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using EPharmApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IOrderService orderService, IPharmacyService pharmacyService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = IdentityData.Admin)]
    public async Task<ActionResult<IEnumerable<GetOrderDto>>> GetAllOrders()
    {
        var result = await orderService.GetAllOrders();
        if (result.Any()) return Ok(result);

        return NotFound("Orders not found.");
    }

    [HttpGet("pharmacy")]
    [Authorize(Roles = IdentityData.Admin + "," + IdentityData.PharmacyStaff)]
    [RequirePharmacyId]
    public async Task<ActionResult<IEnumerable<GetOrderPharmacyDto>>> GetPharmacyOrders(int? pharmacyId)
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
        var result = await orderService.GetAllPharmacyOrders(pharmacyId.Value);
        if (result.Any()) return Ok(result);

        return NotFound("Orders not found.");
    }
    
    [HttpGet("user/{userId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<GetOrderDto>>> GetAllUserOrders(string userId)
    {
        var currentUserId = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;

        if (!User.IsInRole(IdentityData.Admin) && currentUserId != userId)
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
        catch (Exception ex) when (ex.Message == "PRODUCT_NOT_FOUND")
        {
            return NotFound(new { Error = "One or more products in the order were not found." });
        }
        catch (Exception ex) when (ex.Message == "STOCK_NOT_ENOUGH")
        {
            return BadRequest(new { Error = "Insufficient stock for one or more products." });
        }
        catch (Exception ex) when (ex.Message == "FAILED_TO_CREATE_PAYPAL_ORDER")
        {
            return BadRequest(new { Error = "Failed to create PayPal order." });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while creating order.");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpPost("{orderId}/capture")]
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
            Log.Error(ex, "An error occurred while capturing order.");
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
