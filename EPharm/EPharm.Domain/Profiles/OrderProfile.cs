using AutoMapper;
using EPharm.Domain.Dtos.OrderDto;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<CreateOrderDto, Order>();
        CreateMap<Order, GetOrderDto>().ForMember(dest => dest.ProductIds,
            opt => opt.MapFrom(src => src.OrderProducts.Select(op => op.ProductId).ToArray()));
    }
}
