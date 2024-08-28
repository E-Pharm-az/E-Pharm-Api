using EPharm.Domain.Dtos.PayPalDtos;
using EPharm.Domain.Dtos.PayPalDtos.Payload;
using RestSharp;

namespace EPharm.Domain.Interfaces.CommonContracts;

public interface IPayPalClient
{
    public Task<RestResponse<CreateOrderResponse>> CreatePayPalOrderAsync(PayPalOrderPayload payload);
    public Task<RestResponse<CreateOrderResponse>> CapturePayPalOrderAsync(string orderId);    
}