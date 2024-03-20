using EPharm.Domain.Models.Jwt;
using EPharm.Infrastructure.Context.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace EPharm.Domain.Interfaces.Jwt;

public interface ITokenCreationService
{
    public AuthResponse CreateToken(AppIdentityUser user, IList<string> roles);
}
