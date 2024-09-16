namespace EPharm.Domain.Models.Product;

public static class OrderStatus
{
    public const string Pending = "PENDING";
    public const string Confirmed = "CONFIRMED";
    public const string Rejected = "REJECTED";
    public const string Shipped = "SHIPPED";
    public const string Delivered = "DELIVERED";
    public const string Canceled = "CANCELED";
    public const string Returned = "RETURNED";
}
