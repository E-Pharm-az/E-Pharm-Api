using AutoMapper;
using EPharm.Domain.Dtos.DosageFormDto;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class DosageFormProfile : Profile
{
    public DosageFormProfile()
    {
        CreateMap<CreateDosageFormDto, DosageForm>();
        CreateMap<DosageForm, GetDosageFormDto>();
        CreateMap<ProductDosageForm, GetDosageFormDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DosageForm.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DosageForm.Name));
    }
}