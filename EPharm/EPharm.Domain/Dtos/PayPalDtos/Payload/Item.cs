using Newtonsoft.Json;

namespace EPharm.Domain.Dtos.PayPalDtos.Payload;

public class Item
{
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("unit_amount")]
    public UnitAmount UnitAmount { get; set; } = new();
    
    [JsonProperty("quantity")]
    public string Quantity { get; set; }
}
