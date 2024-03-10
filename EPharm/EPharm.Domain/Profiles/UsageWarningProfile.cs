using AutoMapper;
using EPharm.Domain.Dtos.ProductDtos.UsageWarningDto;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class UsageWarningProfile : Profile
{
    public UsageWarningProfile()
    {
        CreateMap<CreateUsageWarningDto, UsageWarning>();
        CreateMap<UsageWarning, GetUsageWarningDto>();
    }
}
