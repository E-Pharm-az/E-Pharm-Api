using Newtonsoft.Json;

namespace EPharm.Domain.Dtos.PayPalDtos.Payload;

public class Breakdown
{
    [JsonProperty("item_total")]
    public ItemTotal ItemTotal { get; set; }
}
