using System.Security.Cryptography;
using EPharm.Domain.Interfaces.JwtContracts;

namespace EPharm.Domain.Services.Jwt;

public class TokenRefreshService : ITokenRefreshService
{
    public string RefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}