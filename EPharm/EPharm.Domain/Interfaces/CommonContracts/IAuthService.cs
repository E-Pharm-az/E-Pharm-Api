using EPharm.Domain.Dtos.AuthDto;
using EPharm.Domain.Models.Jwt;

namespace EPharm.Domain.Interfaces.CommonContracts;

public interface IAuthService
{
    Task<AuthResponse> ProcessLoginAsync(AuthRequest request, string role);
    Task ConfirmEmailAsync(ConfirmEmailDto request);
    Task<AuthResponse> RefreshTokenAsync(string accessToken, string refreshToken, string role);    
}
