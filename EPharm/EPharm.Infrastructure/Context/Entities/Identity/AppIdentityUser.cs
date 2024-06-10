using Microsoft.AspNetCore.Identity;

namespace EPharm.Infrastructure.Context.Entities.Identity;

public class AppIdentityUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Address { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public int Code { get; set; }
    public int CodeVerificationFailedAttempts { get; set; }
    public DateTime CodeExpiryTime { get; set; }
    public DateTime LockoutEnd { get; set; }
}
