using AutoMapper;
using EPharm.Domain.Dtos.AttributeDtos;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class AttributeProfile : Profile
{
    public AttributeProfile()
    {
        CreateMap<CreateAttributeDto, Attribute>();
        CreateMap<Attribute, GetAttributeDto>();
        
        CreateMap<CreateAttributeDto, DosageForm>();
        CreateMap<Allergy, GetAttributeDto>();
        CreateMap<ProductAllergy, GetAttributeDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Allergy.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Allergy.Name));
        
        CreateMap<CreateAttributeDto, DosageForm>();
        CreateMap<DosageForm, GetAttributeDto>();
        CreateMap<ProductDosageForm, GetAttributeDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DosageForm.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DosageForm.Name));

        CreateMap<CreateAttributeDto, Indication>();
        CreateMap<Indication, GetAttributeDto>();
        CreateMap<IndicationProduct, GetAttributeDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Indication.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Indication.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Indication.Description)); 
 
        CreateMap<CreateAttributeDto, RouteOfAdministration>();
        CreateMap<RouteOfAdministration, GetAttributeDto>();
        CreateMap<ProductRouteOfAdministration, GetAttributeDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RouteOfAdministration.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RouteOfAdministration.Name));
        
        CreateMap<CreateAttributeDto, SideEffect>();
        CreateMap<SideEffect, GetAttributeDto>();
        CreateMap<ProductSideEffect, GetAttributeDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SideEffect.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SideEffect.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SideEffect.Description));
        
        CreateMap<GetAttributeDto, UsageWarning>();
        CreateMap<UsageWarning, GetAttributeDto>();
        CreateMap<ProductUsageWarning, GetAttributeDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Product))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name));
    }
}
