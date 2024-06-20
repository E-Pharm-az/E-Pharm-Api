namespace EPharm.Domain.Dtos.PayPalDtos.Payload;

public class PayPalOrderPayload
{
    public string Intent { get; set; }
    public PurchaseUnit[] PurchaseUnits { get; set; }
}