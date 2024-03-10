using AutoMapper;
using EPharm.Domain.Dtos.OrderDto;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, GetOrderDto>();
        CreateMap<CreateOrderDto, Order>();
    }
}
