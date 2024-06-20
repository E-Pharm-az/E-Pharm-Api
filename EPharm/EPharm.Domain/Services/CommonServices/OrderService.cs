using AutoMapper;
using EPharm.Domain.Dtos.OrderDto;
using EPharm.Domain.Dtos.PayPalDtos;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Domain.Models.Product;
using EPharm.Infrastructure.Context.Entities.Identity;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace EPharm.Domain.Services.CommonServices;

public class OrderService(
    IUnitOfWork unitOfWork,
    UserManager<AppIdentityUser> userManager,
    IOrderRepository orderRepository,
    IOrderProductRepository orderProductRepository,
    IProductRepository productRepository,
    IConfiguration configuration,
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

    public async Task<GetOrderDto> CreateOrderAsync(CreateOrderDto orderDto)
    {
        if (orderDto.Email is null && orderDto.PhoneNumber is null)
            throw new ArgumentException("MISSING_REQUIRED_FIELDS_FOR_ORDER");

        var orderSummary = new OrderSummary();
        var products =
            await productRepository.GetApprovedProductsByIdAsync(orderDto.Products.Select(p => p.ProductId).ToArray());

        // TODO: Performance review
        foreach (var product in products)
        {
            if (product is null)
                throw new ArgumentException("PRODUCT_NOT_FOUND");

            if (product.IsApproved is false)
                throw new ArgumentException("PRODUCT_NOT_APPROVED");

            var productStock = product.Stock.Select(s => s.Quantity).Sum();

            // Mapping the queried product to product in order
            var orderProduct = orderDto.Products.First(p => p.ProductId == product.Id);

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

        var payload = new
        {
            intent = "CAPTURE",
            purchase_units = new[]
            {
                new
                {
                    amount = new
                    {
                        currency_code = orderDto.Currency,
                        value = orderSummary.TotalPrice.ToString(), // TODO: Requires conversion performance review?
                        breakdown = new
                        {
                            item_total = new
                            {
                                currency_code = orderDto.Currency,
                                value = orderSummary.TotalPrice.ToString("0.00"),
                            },
                        }
                    },
                    items = orderSummary.Products.Select(p => new
                    {
                        name = p.Name,
                        unit_amount = new
                        {
                            currency_code = orderDto.Currency,
                            value = p.Value.ToString("0.00"),
                        },
                        quantity = p.Quantity.ToString(),
                    }).ToArray()
                }
            }
        };

        var client = new RestClient(configuration["PayPalConfig:BaseUrl"]!);
        var request = new RestRequest("/v2/checkout/orders", Method.Post);

        request.AddHeader("Authorization", $"Basic {accessToken}");
        request.AddJsonBody(payload);

        var response = await client.ExecuteAsync<CreateOrderResponse>(request);

        if (response.IsSuccessful is false)
        {
            throw new Exception("FAILED_TO_CREATE_PAYPAL_ORDER");
        }

        var orderEntity = mapper.Map<Order>(orderDto);
        orderEntity.TrackingId = response.Data.Id;
        orderEntity.Status = OrderStatus.PendingPayment;

        var user = await userManager.FindByEmailAsync(orderEntity.Email);

        if (user is not null)
            orderEntity.UserId = user.Id;

        await unitOfWork.BeginTransactionAsync();

        var order = await orderRepository.InsertAsync(orderEntity);

        var orderProductEntities = mapper.Map<IEnumerable<OrderProduct>>(orderDto.Products);

        await orderProductRepository.CreateManyOrderProductAsync(orderProductEntities);

        await unitOfWork.CommitTransactionAsync();
        await unitOfWork.SaveChangesAsync();

        return mapper.Map<GetOrderDto>(order);
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
}