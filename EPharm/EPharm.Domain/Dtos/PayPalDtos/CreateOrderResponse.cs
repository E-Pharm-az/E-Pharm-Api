using Newtonsoft.Json;

namespace EPharm.Domain.Dtos.PayPalDtos;

public class CreateOrderResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("status")]
    public string Status { get; set; }
}
