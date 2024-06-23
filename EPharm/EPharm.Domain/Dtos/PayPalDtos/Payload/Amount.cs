namespace EPharm.Domain.Dtos.PayPalDtos.Payload;

public class Amount
{
    public string CurrencyCode { get; set; }
    public string Value { get; set; }
    public Breakdown Breakdown { get; set; }
}