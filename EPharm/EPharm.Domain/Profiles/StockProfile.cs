using AutoMapper;
using EPharm.Domain.Dtos.StockDto;
using EPharm.Infrastructure.Entities.Junctions;

namespace EPharm.Domain.Profiles;

public class StockProfile : Profile
{
    public StockProfile()
    {
        CreateMap<CreateStockDto, WarehouseProduct>();
        CreateMap<WarehouseProduct, GetStockDto>();
    }
}