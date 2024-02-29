using EPharm.Domain.Dtos.UserDto;

namespace EPharm.Domain.Interfaces.User;

public interface IUserService
{
    public Task<UserRegistrationDto> CreateUser(UserRegistrationDto user);
}
