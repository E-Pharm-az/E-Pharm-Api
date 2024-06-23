namespace EPharm.Domain.Dtos.PayPalDtos.Payload;

public class Item
{
    public string Name { get; set; }
    public UnitAmount UnitAmount { get; set; }
    public string Quantity { get; set; }
}