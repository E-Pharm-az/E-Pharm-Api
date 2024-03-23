using System.Security.Claims;
using EPharm.Domain.Dtos.OrderDto;
using EPharm.Domain.Interfaces;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Stripe;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CheckoutController(IConfiguration configuration, IUnitOfWork unitOfWork, IOrderService orderService) : ControllerBase
{
    [HttpGet("config")]
    public async Task<IActionResult> GetConfig()
    {
        var publishableKey = configuration["StripeConfig:PublishableKey"];
        return Ok(new { publishableKey });
    }
    
    [HttpPost("create-payment-intent")]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] CreateOrderDto orderDto)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await orderService.CreateOrderAsync(userId, orderDto);

            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = order.TotalPrice,
                Currency = "usd",
                AutomaticPaymentMethods = new()
                {
                    Enabled = true
                }
            });

            await unitOfWork.CommitTransactionAsync();

            return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            Log.Error("Error creating payment intent. Error: {Error}", ex.Message);
            return BadRequest($"Error creating payment intent. Error message: {ex.Message}");
        }
    }
}
