using System.Text.Json.Serialization;

namespace EPharm.Domain.Models.Payment;

public class PaymentAuthResponse
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; }
}