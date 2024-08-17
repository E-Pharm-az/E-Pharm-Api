using Newtonsoft.Json;

namespace EPharm.Domain.Dtos.PayPalDtos;

public class TokenDto
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }
}
