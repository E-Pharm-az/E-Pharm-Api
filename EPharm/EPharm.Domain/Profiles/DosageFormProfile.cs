using AutoMapper;
using EPharm.Domain.Dtos.DosageFormDto;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class DosageFormProfile : Profile
{
    public DosageFormProfile()
    {
        CreateMap<CreateDosageFormDto, DosageForm>();
        CreateMap<DosageForm, GetDosageFormDto>();
    }
}
