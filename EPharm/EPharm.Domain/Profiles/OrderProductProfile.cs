using AutoMapper;
using EPharm.Domain.Dtos.OrderProductDto;
using EPharm.Infrastructure.Entities.Junctions;

namespace EPharm.Domain.Profiles;

public class OrderProductProfile : Profile
{
    public OrderProductProfile()
    {
        CreateMap<CreateOrderProductDto, OrderProduct>();
        CreateMap<OrderProduct, GetOrderProductDto>();
    }
}