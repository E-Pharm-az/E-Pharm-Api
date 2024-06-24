using AutoMapper;
using EPharm.Domain.Dtos.WarehouseDto;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class WarehouseProfile : Profile
{
    public WarehouseProfile()
    {
        CreateMap<Warehouse, GetWarehouseDto>();
        CreateMap<CreateWarehouseDto, Warehouse>();
    }
}