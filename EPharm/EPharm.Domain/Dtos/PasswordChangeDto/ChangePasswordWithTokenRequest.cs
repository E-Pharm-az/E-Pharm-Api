namespace EPharm.Domain.Dtos.PasswordChangeDto;

public class ChangePasswordWithTokenRequest
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; } 
}
