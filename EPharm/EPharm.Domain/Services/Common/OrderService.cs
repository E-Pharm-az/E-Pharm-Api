using System.Globalization;
using AutoMapper;
using EPharm.Domain.Dtos.OrderDto;
using EPharm.Domain.Dtos.PayPalDtos;
using EPharm.Domain.Dtos.PayPalDtos.Payload;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Models.Product;
using EPharm.Infrastructure.Entities.Identity;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;
using EPharm.Infrastructure.Interfaces.Junctions;
using EPharm.Infrastructure.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;

namespace EPharm.Domain.Services.Common;

public class OrderService(
    IUnitOfWork unitOfWork,
    UserManager<AppIdentityUser> userManager,
    IOrderRepository orderRepository,
    IOrderProductRepository orderProductRepository,
    IProductRepository productRepository,
    IMapper mapper,
    IPayPalClient payPalClient)
    : IOrderService
{

    public async Task<IEnumerable<GetOrderDto>> GetAllOrders()
        => mapper.Map<IEnumerable<GetOrderDto>>(await orderRepository.GetAllAsync());

    public async Task<IEnumerable<GetOrderPharmacyDto>> GetAllPharmacyOrders(int pharmacyId)
        => mapper.Map<IEnumerable<GetOrderPharmacyDto>>(await orderRepository.GetAllPharmacyOrdersAsync(pharmacyId));

    public async Task<IEnumerable<GetOrderDto>> GetAllUserOrders(string userId)
        => mapper.Map<IEnumerable<GetOrderDto>>(await orderRepository.GetAllUserOrdersAsync(userId));

    public async Task<GetOrderDto?> GetOrderByTrackingNumberAsync(string trackingNumber)
        => mapper.Map<GetOrderDto?>(await orderRepository.GetOrderByTrackingNumberAsync(trackingNumber));

    public async Task<GetOrderDto?> GetOrderByIdAsync(int orderId)
        => mapper.Map<GetOrderDto?>(await orderRepository.GetByIdAsync(orderId));

    public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderDto orderDto)
    {
        var (orderSummary, products) = await ValidateAndSummarizeOrder(orderDto);

        var payload = CreatePayPalPayload(orderDto.Currency, orderSummary);
        var response = await payPalClient.CreatePayPalOrderAsync(payload);

        if (!response.IsSuccessful)
            throw new Exception("FAILED_TO_CREATE_PAYPAL_ORDER");

        return await SaveOrder(orderDto, response.Data, products);
    }

    public async Task CaptureOrderAsync(string orderId)
    {
        var order = await orderRepository.GetOrderByTrackingNumberAsync(orderId)
            ?? throw new ArgumentException("ORDER_NOT_FOUND");

        var products = await productRepository.GetApprovedProductsByIdAsync(order.OrderProducts.Select(p => p.ProductId).ToArray());
        var orderProducts = order.OrderProducts.ToDictionary(p => p.ProductId);

        await unitOfWork.ExecuteTransactionAsync(async () =>
        {
            await UpdateProductStock(products, orderProducts);

            var response = await payPalClient.CapturePayPalOrderAsync(orderId);

            if (!response.IsSuccessful)
                throw new ArgumentException("FAILED_TO_CAPTURE_PAYPAL_ORDER");

            order.Status = OrderStatus.Paid;
            orderRepository.Update(order);
        });
    }

    public async Task<bool> UpdateOrderAsync(int id, CreateOrderDto orderDto)
    {
        var orderEntity = await orderRepository.GetByIdAsync(id);

        if (orderEntity is null)
            return false;

        mapper.Map(orderDto, orderEntity);
        orderRepository.Update(orderEntity);

        return await orderRepository.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var order = await orderRepository.GetByIdAsync(orderId);

        if (order is null)
            return false;

        orderRepository.Delete(order);

        return await orderRepository.SaveChangesAsync() > 0;
    }

    private async Task<(OrderSummary, IEnumerable<Product>)> ValidateAndSummarizeOrder(CreateOrderDto orderDto)
    {
        var orderSummary = new OrderSummary();
        var productIds = orderDto.Products.Select(p => p.ProductId).ToArray();
        var products = await productRepository.GetApprovedProductsByIdAsync(productIds);

        var orderProducts = orderDto.Products.ToDictionary(p => p.ProductId);

        foreach (var product in products)
        {
            if (product is null)
                throw new Exception("PRODUCT_NOT_FOUND");

            var productStock = product.Stock.Sum(s => s.Quantity);

            if (!orderProducts.TryGetValue(product.Id, out var orderProduct) || productStock < orderProduct.Quantity)
                throw new Exception("STOCK_NOT_ENOUGH");

            orderSummary.TotalPrice += product.Price * orderProduct.Quantity;
            orderSummary.Products.Add(new ProductSummary
            {
                Name = product.Name,
                Value = product.Price,
                Quantity = orderProduct.Quantity
            });
        }

        return (orderSummary, products);
    }

    private async Task<CreateOrderResponse> SaveOrder(CreateOrderDto orderDto, CreateOrderResponse payPalResponse, IEnumerable<Product> products)
    {
        var orderEntity = mapper.Map<Order>(orderDto);
        orderEntity.TrackingId = payPalResponse.Id;
        orderEntity.Status = OrderStatus.PendingPayment;

        var user = await userManager.FindByIdAsync(orderDto.UserId) ?? throw new Exception("USER_NOT_FOUND");

        await unitOfWork.ExecuteTransactionAsync(async () =>
        {
            var order = await orderRepository.InsertAsync(orderEntity);

            var orderProductEntities = mapper.Map<IEnumerable<OrderProduct>>(orderDto.Products)
                .Select(op =>
                {
                    var product = products.First(p => p.Id == op.ProductId);
                    op.OrderId = order.Id;
                    op.PharmacyId = product.PharmacyId;
                    op.TotalPrice = product.Price * op.Quantity;
                    return op;
                }).ToList();

            await orderProductRepository.CreateManyOrderProductAsync(orderProductEntities);

            order.TotalPrice = orderProductEntities.Sum(op => op.TotalPrice);
            orderRepository.Update(order);
            await orderRepository.SaveChangesAsync();
        });

        return payPalResponse;
    }

    private async Task UpdateProductStock(IEnumerable<Product> products, Dictionary<int, OrderProduct> orderProducts)
    {
        foreach (var product in products)
        {
            if (product is null)
                throw new ArgumentException("PRODUCT_NOT_FOUND");

            if (!orderProducts.TryGetValue(product.Id, out var orderProduct))
                continue;

            if (product.Stock.Sum(s => s.Quantity) < orderProduct.Quantity)
                throw new ArgumentException("STOCK_NOT_ENOUGH");

            if (product.Stock.Count == 1)
            {
                var stock = product.Stock.First();
                orderProduct.WarehouseId = stock.WarehouseId;
                stock.Quantity -= orderProduct.Quantity;
                productRepository.Update(product);
                orderProductRepository.Update(orderProduct);
            }
        }
    }

    private PayPalOrderPayload CreatePayPalPayload(string currency, OrderSummary orderSummary)
    {
        return new PayPalOrderPayload
        {
            Intent = "CAPTURE",
            PurchaseUnits =
            [
                new PurchaseUnit
                {
                    Amount = new Amount
                    {
                        CurrencyCode = currency,
                        Value = orderSummary.TotalPrice.ToString("0.00", CultureInfo.InvariantCulture),
                        Breakdown = new Breakdown
                        {
                            ItemTotal = new ItemTotal
                            {
                                CurrencyCode = currency,
                                Value = orderSummary.TotalPrice.ToString("0.00", CultureInfo.InvariantCulture),
                            }
                        }
                    },
                    Items = orderSummary.Products.Select(p => new Item
                    {
                        Name = p.Name,
                        UnitAmount = new UnitAmount
                        {
                            CurrencyCode = currency,
                            Value = p.Value.ToString("0.00", CultureInfo.InvariantCulture)
                        },
                        Quantity = p.Quantity.ToString()
                    }).ToArray()
                }
            ]
        };
    }
}