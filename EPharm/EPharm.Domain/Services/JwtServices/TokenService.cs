using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EPharm.Domain.Interfaces.Jwt;
using EPharm.Domain.Models.Jwt;
using EPharm.Infrastructure.Context.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EPharm.Domain.Services.JwtServices;

public class TokenService(
    ITokenCreationService tokenCreationService,
    ITokenRefreshService tokenRefreshService,
    IConfiguration configuration) : ITokenService
{
    public AuthResponse CreateToken(AppIdentityUser user, List<string> roles)
    {
        return tokenCreationService.CreateToken(user, roles);
    }

    public string RefreshToken()
    {
        return tokenRefreshService.RefreshToken();
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}
