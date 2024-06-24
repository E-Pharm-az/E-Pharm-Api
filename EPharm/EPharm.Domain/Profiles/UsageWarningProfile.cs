using AutoMapper;
using EPharm.Domain.Dtos.UsageWarningDto;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class UsageWarningProfile : Profile
{
    public UsageWarningProfile()
    {
        CreateMap<CreateUsageWarningDto, UsageWarning>();
        CreateMap<UsageWarning, GetUsageWarningDto>();
        CreateMap<ProductUsageWarning, GetUsageWarningDto>().IncludeMembers(src => src.UsageWarning);
    }
}