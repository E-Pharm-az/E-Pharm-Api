using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EPharm.Domain.Interfaces.Jwt;
using EPharm.Domain.Models.Identity;
using EPharm.Domain.Models.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EPharm.Domain.Services.JwtServices;

public class TokenCreationService(IConfiguration configuration) : ITokenCreationService
{
    public AuthResponse CreateToken(IdentityUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["JwtSettings:Issuer"],
            audience: configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["JwtSettings:ExpirationMinutes"])),
            signingCredentials: signingCredentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthResponse
        {
            Token = tokenString,
            ValidTo = token.ValidTo.ToString(CultureInfo.InvariantCulture)
        };
    }
}
