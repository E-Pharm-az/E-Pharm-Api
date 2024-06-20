namespace EPharm.Domain.Dtos.PayPalDtos.Payload;

public class PurchaseUnit
{
    public Amount Amount { get; set; }
    public Item[] Items { get; set; }
}