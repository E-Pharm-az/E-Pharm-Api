using EPharm.Domain.Models.Jwt;
using Microsoft.AspNetCore.Identity;

namespace EPharm.Domain.Interfaces.Jwt;

public interface ITokenCreationService
{
    public AuthResponse CreateToken(IdentityUser user);
}
