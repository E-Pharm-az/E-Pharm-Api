using AutoMapper;
using EPharm.Domain.Dtos.ManufacturerDto;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class ManufacturerProfile : Profile
{
    public ManufacturerProfile()
    {
        CreateMap<Manufacturer, GetManufacturerDto>();
        CreateMap<CreateManufacturerDto, Manufacturer>();
    }
}
