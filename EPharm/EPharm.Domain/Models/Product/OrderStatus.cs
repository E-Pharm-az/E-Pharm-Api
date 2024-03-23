namespace EPharm.Domain.Models.Product;

public static class OrderStatus
{
    public const string PendingPayment = "Pending Payment";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string Shipped = "Shipped";
    public const string InOffice = "In Office";
    public const string Delivered = "Delivered";
    public const string Canceled = "Canceled";
}
