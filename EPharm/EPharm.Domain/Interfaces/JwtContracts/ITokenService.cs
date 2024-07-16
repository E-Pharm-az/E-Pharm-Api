using System.Security.Claims;
using EPharm.Domain.Models.Jwt;
using EPharm.Infrastructure.Entities.Identity;

namespace EPharm.Domain.Interfaces.JwtContracts;

public interface ITokenService
{
    public AuthResponse CreateToken(AppIdentityUser user, List<string> roles, int? pharmacyId = null);
    public string RefreshToken();
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
}
