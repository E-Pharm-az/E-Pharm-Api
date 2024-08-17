using Newtonsoft.Json;

namespace EPharm.Domain.Dtos.PayPalDtos.Payload;

public class PayPalOrderPayload
{
    [JsonProperty("intent")]
    public string Intent { get; set; }
    
    [JsonProperty("purchase_units")]
    public PurchaseUnit[] PurchaseUnits { get; set; } = [];
}
