using Newtonsoft.Json;

namespace EPharm.Domain.Dtos.PayPalDtos.Payload;

public class PurchaseUnit
{
    [JsonProperty("amount")]
    public Amount Amount { get; set; } = new();

    [JsonProperty("items")]
    public Item[] Items { get; set; } = [];
}
