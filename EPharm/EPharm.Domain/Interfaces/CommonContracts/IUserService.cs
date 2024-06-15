using EPharm.Domain.Dtos.AuthDto;
using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Infrastructure.Context.Entities.Identity;

namespace EPharm.Domain.Interfaces.CommonContracts;

public interface IUserService
{
    public Task<IEnumerable<GetUserDto>> GetAllUsersAsync();
    public Task<GetUserDto?> GetUserByIdAsync(string userId);
    public Task InitializeUserAsync(InitializeUserDto initializeUserDto);
    public Task<GetUserDto> CreateCustomerAsync(RegisterUserDto registerUserDto);
    public Task<GetPharmaCompanyManagerDto> CreatePharmaManagerAsync(int pharmaCompanyId, RegisterUserDto registerUserDto);
    public Task<GetPharmaCompanyManagerDto> CreatePharmaAdminAsync(RegisterUserDto registerUserDto,
        CreatePharmaCompanyDto createPharmaCompanyDto);
    public Task<GetUserDto> CreateAdminAsync(RegisterUserDto registerUserDto);
    public Task<bool> UpdateUserAsync(string id, RegisterUserDto registerUserDto);
    public Task InitiatePasswordChange(InitiatePasswordChangeRequest passwordChangeRequest, string url);
    public Task ChangePassword(ChangePasswordWithTokenRequest passwordWithTokenRequest);
    public Task SendEmailConfirmationAsync(AppIdentityUser user);
    public Task<bool> DeleteUserAsync(string userId);
}
