namespace EPharm.Domain.Models.Jwt;

public class AuthResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string ValidTo { get; set; }
}
