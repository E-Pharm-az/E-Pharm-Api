namespace EPharm.Domain.Dtos.PayPalDtos;

public class TokenDto
{
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }
}