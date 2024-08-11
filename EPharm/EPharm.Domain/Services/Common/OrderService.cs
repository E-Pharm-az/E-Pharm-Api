using System.Globalization;
using AutoMapper;
using EPharm.Domain.Dtos.OrderDto;
using EPharm.Domain.Dtos.PayPalDtos;
using EPharm.Domain.Dtos.PayPalDtos.Payload;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Models.Product;
using EPharm.Infrastructure.Entities.Identity;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;
using EPharm.Infrastructure.Interfaces.Junctions;
using EPharm.Infrastructure.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace EPharm.Domain.Services.Common;

public class OrderService(
    IUnitOfWork unitOfWork,
    UserManager<AppIdentityUser> userManager,
    IUserService userService,
    IOrderRepository orderRepository,
    IOrderProductRepository orderProductRepository,
    IProductRepository productRepository,
    IConfiguration configuration,
    IMapper mapper) : IOrderService
{
    public async Task<IEnumerable<GetOrderDto>> GetAllOrders()
    {
        var orders = await orderRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetOrderDto>>(orders);
    }
    
    public async Task<IEnumerable<GetOrderProductDto>> GetAllPharmacyOrders(int pharmacyId)
    {
        var orders = await orderRepository.GetAllPharmacyOrdersAsync(pharmacyId);
        return mapper.Map<IEnumerable<GetOrderProductDto>>(orders);
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

    // TODO: Cache user preferences for order delivery address.
    public async Task<GetOrderDto> CreateOrderAsync(CreateOrderDto orderDto)
    {
        if (!string.IsNullOrEmpty(orderDto.Email))
            throw new ArgumentException("MISSING_EMAIL_FOR_ORDER");

        var orderSummary = new OrderSummary();
        var productIds = orderDto.Products.Select(p => p.ProductId).ToArray();
        var products = await productRepository.GetApprovedProductsByIdAsync(productIds);

        var orderProducts = orderDto.Products.ToDictionary(p => p.ProductId);

        foreach (var product in products)
        {
            if (product is null)
                throw new ArgumentException("PRODUCT_NOT_FOUND");

            var productStock = product.Stock.Sum(s => s.Quantity);

            orderProducts.TryGetValue(product.Id, out var orderProduct);

            if (productStock < orderProduct.Quantity)
                throw new ArgumentException("STOCK_NOT_ENOUGH");

            orderSummary.TotalPrice += product.Price * orderProduct.Quantity;
            orderSummary.Products.Add(new ProductSummary
            {
                Name = product.Name,
                Value = product.Price,
                Quantity = orderProduct.Quantity
            });
        }

        var accessToken = await GenerateAccessTokenAsync();
        var payload = CreatePayPalPayload(orderDto.Currency, orderSummary);

        var response = await CreatePayPalOrderAsync(accessToken, payload);

        if (!response.IsSuccessful)
            throw new ArgumentException("FAILED_TO_CREATE_PAYPAL_ORDER");

        var orderEntity = mapper.Map<Order>(orderDto);

        orderEntity.TrackingId = response.Data.Id;
        orderEntity.Status = OrderStatus.PendingPayment;

        var user = await userManager.FindByEmailAsync(orderDto.Email);

        if (user is null)
        {
            await userService.CreateCustomerAsync(new EmailDto { Email = orderDto.Email, });
        }
        else
        {
            orderEntity.UserId = user.Id;
        }

        try
        {
            await unitOfWork.BeginTransactionAsync();

            var order = await orderRepository.InsertAsync(orderEntity);
            var orderProductEntities = mapper.Map<IEnumerable<OrderProduct>>(orderDto.Products);

            await orderProductRepository.CreateManyOrderProductAsync(orderProductEntities);

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

    public async Task CaptureOrderAsync(string orderId)
    {
        var order = await orderRepository.GetOrderByTrackingNumberAsync(orderId);

        if (order is null)
            throw new ArgumentException("ORDER_NOT_FOUND");

        var products =
            await productRepository.GetApprovedProductsByIdAsync(order.OrderProducts.Select(p => p.ProductId)
                .ToArray());

        var orderProducts = order.OrderProducts.ToDictionary(p => p.ProductId);

        // Validate that order product is still in purchasable state
        try
        {
            await unitOfWork.BeginTransactionAsync();

            foreach (var product in products)
            {
                if (product is null)
                    throw new ArgumentException("PRODUCT_NOT_FOUND");

                orderProducts.TryGetValue(product.Id, out var orderProduct);

                if (product.Stock.Sum(s => s.Quantity) < orderProduct.Quantity)
                    throw new ArgumentException("STOCK_NOT_ENOUGH");


                // TODO: Take use of the google api key to determine the most optimal warehouse
                // If product is located in a single warehouse, then we directly order it from there;
                if (product.Stock.Count == 1)
                {
                    orderProduct.WarehouseId = product.Stock.First().WarehouseId;
                    product.Stock.First().Quantity -= orderProduct.Quantity;
                    productRepository.Update(product);
                    orderProductRepository.Update(orderProduct);
                }
            }

            var accessToken = await GenerateAccessTokenAsync();

            var client = new RestClient(configuration["PayPalConfig:BaseUrl"]!);
            var request = new RestRequest($"/v2/checkout/orders/${orderId}/capture", Method.Post);

            request.AddHeader("Authorization", $"Bearer {accessToken}");

            var response = await client.ExecuteAsync<CreateOrderResponse>(request);

            if (!response.IsSuccessful)
            {
                Console.WriteLine(response.ErrorMessage);
                throw new ArgumentException("FAILED_TO_CAPTURE_PAYPAL_ORDER");
            }

            order.Status = OrderStatus.Paid;
            orderRepository.Update(order);

            await unitOfWork.CommitTransactionAsync();
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
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

    private async Task<string> GenerateAccessTokenAsync()
    {
        if (configuration["PayPalConfig:ClientId"] is null || configuration["PayPalConfig:ClientSecret"] is null)
            throw new ArgumentException("MISSING_PAYPAL_API_CREDENTIALS");

        var auth = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes(
                $"{configuration["PayPalConfig:ClientId"]}:{configuration["PayPalConfig:ClientSecret"]}")
        );

        var client = new RestClient(configuration["PayPalConfig:BaseUrl"]!);
        var request = new RestRequest($"{configuration["PayPalConfig:BaseUrl"]}/v1/oauth2/token", Method.Post);

        request.AddHeader("Authorization", $"Basic {auth}");
        request.AddBody("grant_type=client_credentials");

        var response = await client.ExecuteAsync(request);
        if (response.Content is null)
            throw new InvalidOperationException("FAILED_TO_EXECUTE_PAYPAL_TOKEN_REQUEST");

        var token = JsonConvert.DeserializeObject<TokenDto>(response.Content);
        if (token is null)
            throw new Exception("INVALID_PAYPAL_TOKEN_RESPONSE");

        return token.AccessToken;
    }

    private PayPalOrderPayload CreatePayPalPayload(string currency, OrderSummary orderSummary)
    {
        var payload = new PayPalOrderPayload
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

        return payload;
    }

    private async Task<RestResponse<CreateOrderResponse>> CreatePayPalOrderAsync(string accessToken,
        PayPalOrderPayload payload)
    {
        var client = new RestClient(configuration["PayPalConfig:BaseUrl"]!);
        var request = new RestRequest("/v2/checkout/orders", Method.Post);

        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddJsonBody(payload);

        return await client.ExecuteAsync<CreateOrderResponse>(request);
    }
}
