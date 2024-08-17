using Newtonsoft.Json;

namespace EPharm.Domain.Dtos.PayPalDtos.Payload;

public class UnitAmount
{
    [JsonProperty("currency_code")]
    public string CurrencyCode { get; set; }
    
    [JsonProperty("value")]
    public string Value { get; set; }
}
