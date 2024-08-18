using AutoMapper;
using EPharm.Domain.Dtos.OrderDto;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<CreateOrderDto, Order>();
        CreateMap<Order, GetOrderDto>();
        CreateMap<Order, GetOrderPharmacyDto>();

        CreateMap<OrderProduct, GetOrderProductDto>();
    }
}
