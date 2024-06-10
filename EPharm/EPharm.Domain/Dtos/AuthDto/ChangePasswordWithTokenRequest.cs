namespace EPharm.Domain.Dtos.AuthDto;

public class ChangePasswordWithTokenRequest
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; } 
}
