using AutoMapper;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Infrastructure.Context.Entities.Identity;

namespace EPharm.Domain.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserDto, AppIdentityUser>();
        CreateMap<InitializeUserDto, AppIdentityUser>();
        CreateMap<AppIdentityUser, GetUserDto>();
    }
}
