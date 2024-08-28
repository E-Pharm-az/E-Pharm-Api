using EPharm.Domain.Dtos.OrderDto;
using EPharm.Domain.Dtos.PayPalDtos;
using EPharm.Domain.Dtos.PayPalDtos.Payload;
using EPharm.Domain.Interfaces.CommonContracts;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace EPharm.Domain.Services.Common;

public class PayPalClient : IPayPalClient
{
    private readonly IConfiguration _configuration;
    private readonly RestClient _client;

    public PayPalClient(IConfiguration configuration)
    {
        _configuration = configuration;
        _client = new RestClient(_configuration["PayPalConfig:BaseUrl"]!);
    }

    public async Task<RestResponse<CreateOrderResponse>> CreatePayPalOrderAsync(PayPalOrderPayload payload)
    {
        var accessToken = await GenerateAccessTokenAsync();
        var request = new RestRequest("/v2/checkout/orders", Method.Post);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddHeader("Content-Type", "application/json");
        request.AddStringBody(JsonConvert.SerializeObject(payload), ContentType.Json);
        
        return await _client.ExecuteAsync<CreateOrderResponse>(request);
    }

    public async Task<RestResponse<CreateOrderResponse>> CapturePayPalOrderAsync(string orderId)
    {
        var accessToken = await GenerateAccessTokenAsync();
        var request = new RestRequest($"/v2/checkout/orders/{orderId}/capture", Method.Post);
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", $"Bearer {accessToken}");

        return await _client.ExecuteAsync<CreateOrderResponse>(request);
    }
    
    private async Task<string> GenerateAccessTokenAsync()
    {
        if (_configuration["PayPalConfig:ClientId"] is null || _configuration["PayPalConfig:ClientSecret"] is null)
            throw new ArgumentException("MISSING_PAYPAL_API_CREDENTIALS");

        var auth = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{_configuration["PayPalConfig:ClientId"]}:{_configuration["PayPalConfig:ClientSecret"]}"));

        var request = new RestRequest($"{_configuration["PayPalConfig:BaseUrl"]}/v1/oauth2/token", Method.Post);
        request.AddHeader("Authorization", $"Basic {auth}");
        request.AddBody("grant_type=client_credentials");

        var response = await _client.ExecuteAsync(request);
        if (response.Content is null)
            throw new InvalidOperationException("FAILED_TO_EXECUTE_PAYPAL_TOKEN_REQUEST");

        var token = JsonConvert.DeserializeObject<TokenDto>(response.Content);
        return token?.AccessToken ?? throw new Exception("INVALID_PAYPAL_TOKEN_RESPONSE");
    }
}