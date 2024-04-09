using System.Globalization;
using System.Security.Claims;
using EPharm.Domain.Dtos.OrderDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Models.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Serilog;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CheckoutController(IConfiguration configuration, IOrderService orderService) : ControllerBase
{
    public async Task<PaymentAuthResponse> GenerateAccessTokenAsync()
    {
        var client = new RestClient(configuration["PayPalConfig:Base"]!);
        var request = new RestRequest("/v1/oauth2/token", Method.Post);

        var auth = $"{configuration["PayPalConfig:ClientId"]!}:{configuration["PayPalConfig:ClientSecret"]!}";
        var encodedAuth = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));

        request.AddHeader("Authorization", $"Basic {encodedAuth}");
        request.AddParameter("grant_type", "client_credentials");

        var response = await client.ExecuteAsync<PaymentAuthResponse>(request);
        return response.Data!;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        try
        {
            var accessToken = await GenerateAccessTokenAsync();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    
            var order = await orderService.CreateOrderAsync(userId, orderDto);
            
            var client = new RestClient(configuration["PayPalConfig:Base"]!);
            var request = new RestRequest("/v2/checkout/orders", Method.Post);
            
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        amount = new
                        {
                            currency_code = "AZN",
                            value = (order.TotalPrice / 100.0).ToString("0.00", CultureInfo.InvariantCulture) 
                        }
                    }
                }
            });
            
            await client.ExecuteAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Error creating order. Error: {Error}", ex.Message);
            return BadRequest($"Error creating order.");
        }
    }

    [HttpPost("{orderId}/capture-order")]
    public async Task<IActionResult> CaptureOrder(string orderId)
    {
        try
        {
            var accessToken = await GenerateAccessTokenAsync();

            var client = new RestClient(configuration["PayPalConfig:Base"]!);
            var request = new RestRequest($"/v2/checkout/orders/{orderId}/capture", Method.Post);

            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("Content-Type", "application/json");

            await client.ExecuteAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Error processing order. Error: {Error}", ex.Message);
            return BadRequest($"Error processing order.");
        }
    }
}
