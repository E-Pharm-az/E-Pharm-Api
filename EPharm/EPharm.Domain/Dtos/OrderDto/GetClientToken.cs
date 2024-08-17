using Newtonsoft.Json;

namespace EPharm.Domain.Dtos.OrderDto;

public class GetClientToken
{
    [JsonProperty("client_token")]
    public string ClientToken { get; set; }
    
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
}
