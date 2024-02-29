namespace EPharm.Domain.Models.Jwt;

public class AuthRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
