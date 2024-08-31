namespace EPharm.Domain.Models.Product;

public static class OrderStatus
{
    public const string PendingPayment = "PENDING_PAYMENT";
    public const string Paid = "PAID";
    public const string Approved = "APPROVED";
    public const string Rejected = "REJECTED";
    public const string Shipped = "SHIPPED";
    public const string InOffice = "IN_OFFICE";
    public const string Delivered = "DELIVERED";
    public const string Canceled = "CANCELED";
    public const string Returned = "RETURNED";
}
