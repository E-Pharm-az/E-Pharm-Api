using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EPharm.Domain.Interfaces.JwtContracts;
using EPharm.Domain.Models.Jwt;
using EPharm.Infrastructure.Context.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EPharm.Domain.Services.Jwt;

public class TokenCreationService(IConfiguration configuration) : ITokenCreationService
{
    public AuthResponse CreateToken(AppIdentityUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Sub, user.FirstName),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: configuration["JwtSettings:Issuer"]!,
            audience: configuration["JwtSettings:Audience"]!,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["JwtSettings:ExpirationMinutes"])),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)), SecurityAlgorithms.HmacSha256));

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        
        return new AuthResponse
        {
            Token = tokenString,
            ValidTo = token.ValidTo.ToString(CultureInfo.InvariantCulture)
        };
    }
}
