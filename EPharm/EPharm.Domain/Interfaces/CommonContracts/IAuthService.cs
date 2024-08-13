using EPharm.Domain.Dtos.AuthDto;
using EPharm.Domain.Models.Jwt;

namespace EPharm.Domain.Interfaces.CommonContracts;

public interface IAuthService
{
    public Task<AuthResponse> ProcessLoginAsync(AuthRequest request, string role);
    public Task<AuthResponse> RefreshTokenAsync(string accessToken, string refreshToken, string role);    
}
