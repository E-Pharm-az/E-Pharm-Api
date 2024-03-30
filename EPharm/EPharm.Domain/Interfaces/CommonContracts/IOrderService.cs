using EPharm.Domain.Dtos.OrderDto;

namespace EPharm.Domain.Interfaces.CommonContracts;

public interface IOrderService
{
    public Task<IEnumerable<GetOrderDto>> GetAllOrders();
    public Task<IEnumerable<GetOrderDto>> GetAllUserOrders(string userId);
    public Task<GetOrderDto?> GetOrderByTrackingNumberAsync(string trackingNumber);
    public Task<GetOrderDto?> GetOrderByIdAsync(int orderId);
    public Task<GetOrderDto> CreateOrderAsync(string userId, CreateOrderDto orderDto);
    public Task<bool> UpdateOrderAsync(int id, CreateOrderDto orderDto);
    public Task<bool> DeleteOrderAsync(int orderId);
}
