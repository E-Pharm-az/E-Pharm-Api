using AutoMapper;
using EPharm.Domain.Dtos.OrderDto;
using EPharm.Domain.Interfaces.Product;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Domain.Services.ProductServices;

public class OrderService(IOrderRepository orderRepository, IOrderProductRepository orderProductRepository,IMapper mapper) : IOrderService
{
    public async Task<IEnumerable<GetOrderDto>> GetAllOrders()
    {
        var orders = await orderRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetOrderDto>>(orders);
    }
    
    public async Task<IEnumerable<GetOrderDto>> GetAllPharmaCompanyOrdersAsync(int pharmaCompanyId)
    {
        var orders = await orderRepository.GetAllCompanyOrdersAsync(pharmaCompanyId);
        return mapper.Map<IEnumerable<GetOrderDto>>(orders);
    }

    public async Task<IEnumerable<GetOrderDto>> GetAllWarehouseOrders(int warehouseId)
    {
        var orders = await orderRepository.GetAllWarehouseOrdersAsync(warehouseId);
        return mapper.Map<IEnumerable<GetOrderDto>>(orders);
    }

    public async Task<IEnumerable<GetOrderDto>> GetAllUserOrders(string userId)
    {
        var orders = await orderRepository.GetAllUserOrdersAsync(userId);
        return mapper.Map<IEnumerable<GetOrderDto>>(orders);
    }

    public async Task<GetOrderDto?> GetOrderByTrackingNumberAsync(string trackingNumber)
    {
        var order = await orderRepository.GetOrderByTrackingNumberAsync(trackingNumber);
        return mapper.Map<GetOrderDto?>(order);
    }

    public async Task<GetOrderDto?> GetOrderByIdAsync(int orderId)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        return mapper.Map<GetOrderDto?>(order);
    }

    public async Task<GetOrderDto> CreateOrderAsync(CreateOrderDto orderDto)
    {
        try 
        {
            var orderEntity = mapper.Map<Order>(orderDto);
            orderEntity.TrackingId = Guid.NewGuid().ToString();
            var order = await orderRepository.InsertAsync(orderEntity);
            
            await orderProductRepository.InsertOrderProductAsync(order.Id, orderDto.ProductIds);
            
            return mapper.Map<GetOrderDto>(order);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create order. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdateOrderAsync(int id, CreateOrderDto orderDto)
    {
        var orderEntity = await orderRepository.GetByIdAsync(id);
        
        if (orderEntity is null)
            return false;
        
        mapper.Map(orderDto, orderEntity);
        orderRepository.Update(orderEntity);
        
        var result = await orderRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        
        if (order is null)
            return false;
        
        orderRepository.Delete(order);
        
        var result = await orderRepository.SaveChangesAsync();
        return result > 0;
    }
}
