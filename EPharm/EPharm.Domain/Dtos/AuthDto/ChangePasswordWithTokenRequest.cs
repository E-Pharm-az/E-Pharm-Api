namespace EPharm.Domain.Dtos.AuthDto;

public class ChangePasswordWithTokenRequest
{
    public string Email { get; set; }
    public int Code { get; set; }
    public string Password { get; set; }
}