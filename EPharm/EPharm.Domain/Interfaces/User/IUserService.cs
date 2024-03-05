using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Domain.Dtos.UserDto;

namespace EPharm.Domain.Interfaces.User;

public interface IUserService
{
    public Task<IEnumerable<GetUserDto>> GetAllUsersAsync();
    public Task<GetUserDto?> GetUserByIdAsync(string userId);
    public Task<GetUserDto> CreateCustomerAsync(CreateUserDto createUser);
    public Task<GetPharmaCompanyManagerDto> CreatePharmaManagerAsync(int pharmaCompanyId, CreateUserDto createUserDto);
    public Task<GetPharmaCompanyManagerDto> CreatePharmaAdminAsync(CreateUserDto createUserDto, CreatePharmaCompanyDto createPharmaCompanyDto);
    public Task<GetUserDto> CreateAdminAsync(CreateUserDto createUserDto);
    public Task<bool> UpdateUserAsync(GetUserDto getUserDto);
    public Task<bool> DeleteUserAsync(string userId);
}
