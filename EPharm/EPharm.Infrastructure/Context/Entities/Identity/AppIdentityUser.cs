using Microsoft.AspNetCore.Identity;

namespace EPharm.Infrastructure.Context.Entities.Identity;

public class AppIdentityUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Fin { get; set; }
    
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
