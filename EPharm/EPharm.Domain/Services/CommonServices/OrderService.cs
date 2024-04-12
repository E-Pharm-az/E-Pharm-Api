using AutoMapper;
using EPharm.Domain.Dtos.OrderDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Models.Product;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Domain.Services.CommonServices;

public class OrderService(
    IUnitOfWork unitOfWork,
    IOrderRepository orderRepository,
    IOrderProductRepository orderProductRepository,
    IMapper mapper) : IOrderService
{
    public async Task<IEnumerable<GetOrderDto>> GetAllOrders()
    {
        var orders = await orderRepository.GetAllOrdersAsync();
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
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        return mapper.Map<GetOrderDto?>(order);
    }

    public async Task<GetOrderDto> InitializeOrderAsync(string? userId, CreateOrderDto orderDto)
    {
        try 
        {
            // User should either be authenticated, or provide enough user info to handle shipment
            // TODO: Move this logic to fluent validation
            if (userId is null && ((orderDto.Email is null && orderDto.PhoneNumber is null) || orderDto.FullName is null))
                throw new ArgumentException("One or more required fields are null.");
            
            var orderEntity = mapper.Map<Order>(orderDto);
            orderEntity.TrackingId = Guid.NewGuid().ToString();
            orderEntity.Status = OrderStatus.PendingPayment;
            orderEntity.UserId = userId;

            await unitOfWork.BeginTransactionAsync();
            
            var order = await orderRepository.InsertAsync(orderEntity);
            var orderProductEntity = mapper.Map<OrderProduct>(order);
            
            foreach (var op in orderDto.Products)
            {
                var orderProduct = await orderProductRepository.InsertOrderProductAsync(orderProductEntity);
                order.TotalPrice += orderProduct.Price * op.Quantity;
            }

            await unitOfWork.CommitTransactionAsync();
            await unitOfWork.SaveChangesAsync();
            
            return mapper.Map<GetOrderDto>(order);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> CaptureOrderAsync(int orderId)
    {
        // TODO: Implement the stock logic, where on order the stock should go down, and if there is not stock, the order should not go through
        throw new NotImplementedException();
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
