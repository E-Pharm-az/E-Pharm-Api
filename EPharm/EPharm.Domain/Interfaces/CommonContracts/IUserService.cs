using EPharm.Domain.Dtos.PasswordChangeDto;
using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Domain.Dtos.UserDto;

namespace EPharm.Domain.Interfaces.CommonContracts;

public interface IUserService
{
    public Task<IEnumerable<GetUserDto>> GetAllUsersAsync();
    public Task<GetUserDto?> GetUserByIdAsync(string userId);
    public Task<GetUserDto> CreateCustomerAsync(CreateUserDto createUserDto);
    public Task<GetPharmaCompanyManagerDto> CreatePharmaManagerAsync(int pharmaCompanyId, CreateUserDto createUserDto);
    public Task<GetPharmaCompanyManagerDto> CreatePharmaAdminAsync(CreateUserDto createUserDto,
        CreatePharmaCompanyDto createPharmaCompanyDto);
    public Task<GetUserDto> CreateAdminAsync(CreateUserDto createUserDto);
    public Task<bool> UpdateUserAsync(string id, CreateUserDto createUserDto);
    public Task InitiatePasswordChange(InitiatePasswordChangeRequest passwordChangeRequest, string url);
    public Task ChangePassword(ChangePasswordWithTokenRequest passwordWithTokenRequest);
    public Task<bool> DeleteUserAsync(string userId);
}
