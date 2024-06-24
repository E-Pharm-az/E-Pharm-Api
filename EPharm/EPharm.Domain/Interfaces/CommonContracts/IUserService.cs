using EPharm.Domain.Dtos.AuthDto;
using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Infrastructure.Entities.Identity;

namespace EPharm.Domain.Interfaces.CommonContracts;

public interface IUserService
{
    public Task<IEnumerable<GetUserDto>> GetAllUsersAsync();
    public Task<GetUserDto?> GetUserByIdAsync(string userId);
    public Task CreateCustomerAsync(EmailDto emailDto);
    public Task InitializeUserAsync(InitializeUserDto initializeUserDto);
    public Task<GetUserDto> CreateAdminAsync(EmailDto emailDto);
    public Task<bool> UpdateUserAsync(string id, EmailDto emailDto);
    public Task InitiatePasswordChange(InitiatePasswordChangeRequest passwordChangeRequest, string url);
    public Task<AppIdentityUser> CreateUserAsync(EmailDto emailDto, string[] identityRole);
    public Task ChangePassword(ChangePasswordWithTokenRequest passwordWithTokenRequest);
    public Task SendEmailConfirmationAsync(AppIdentityUser user);
    public Task<bool> DeleteUserAsync(string userId);
}