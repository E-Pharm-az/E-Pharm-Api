using EPharm.Domain.Models.Jwt;
using EPharm.Infrastructure.Context.Entities.Identity;

namespace EPharm.Domain.Interfaces.JwtContracts;

public interface ITokenCreationService
{
    public AuthResponse CreateToken(AppIdentityUser user, IList<string> roles);
}