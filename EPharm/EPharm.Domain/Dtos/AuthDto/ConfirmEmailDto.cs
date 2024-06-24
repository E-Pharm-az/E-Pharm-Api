namespace EPharm.Domain.Dtos.AuthDto;

public class ConfirmEmailDto
{
    public string Email { get; set; }
    public int Code { get; set; }
}