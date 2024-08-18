using EPharm.Domain.Dtos.OrderDto;
using EPharm.Domain.Dtos.PayPalDtos;

namespace EPharm.Domain.Interfaces.CommonContracts;

public interface IOrderService
{
    public Task<IEnumerable<GetOrderDto>> GetAllOrders();
    public Task<IEnumerable<GetOrderPharmacyDto>> GetAllPharmacyOrders(int pharmacyId);
    public Task<IEnumerable<GetOrderDto>> GetAllUserOrders(string userId);
    public Task<GetOrderDto?> GetOrderByTrackingNumberAsync(string trackingNumber);
    public Task<GetOrderDto?> GetOrderByIdAsync(int orderId);
    public Task<string> GenerateClientTokenAsync();
    public Task<CreateOrderResponse> CreateOrderAsync(CreateOrderDto orderDto);
    public Task CaptureOrderAsync(string orderId);
    public Task<bool> UpdateOrderAsync(int id, CreateOrderDto orderDto);
    public Task<bool> DeleteOrderAsync(int orderId);
}
