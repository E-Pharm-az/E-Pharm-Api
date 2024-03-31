using System.Security.Claims;
using EPharm.Domain.Models.Jwt;
using EPharm.Infrastructure.Context.Entities.Identity;

namespace EPharm.Domain.Interfaces.JwtContracts;

public interface ITokenService
{
    public AuthResponse CreateToken(AppIdentityUser user, List<string> roles);
    public string RefreshToken();
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token); 
}