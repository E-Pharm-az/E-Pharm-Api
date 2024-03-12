using EPharm.Domain.Dtos.OrderDto;
using EPharm.Domain.Interfaces;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;
using Stripe;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]/create-payment-intent")]
[Authorize]
public class CheckoutController(IUnitOfWork unitOfWork, IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] CreateOrderDto orderDto)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync();
            
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti);
            var order = await orderService.CreateOrderAsync(userId.Value, orderDto);
            Console.WriteLine("Order total: " + order.TotalPrice);

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
