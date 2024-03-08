using System.Security.Claims;
using EPharm.Domain.Models.Jwt;
using Microsoft.AspNetCore.Identity;

namespace EPharm.Domain.Interfaces.Jwt;

public interface ITokenService
{
    public AuthResponse CreateToken(IdentityUser user, List<string> roles);
    public string RefreshToken();
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token); 
}
