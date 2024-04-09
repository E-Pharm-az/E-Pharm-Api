using AutoMapper;
using EPharm.Domain.Dtos.StockDto;
using EPharm.Infrastructure.Context.Entities.Junctions;

namespace EPharm.Domain.Profiles;

public class StockProfile : Profile
{
    public StockProfile()
    {
        CreateMap<CreateStockDto, WarehouseProduct>();
        CreateMap<WarehouseProduct, GetStockDto>();
    }
}
