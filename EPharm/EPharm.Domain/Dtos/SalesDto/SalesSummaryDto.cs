namespace EPharm.Domain.Dtos.SalesDto;

public class SalesSummaryDto
{
    public decimal TotalSales { get; set; }
    public int TotalOrders { get; set; }
    public SalesDto[] Sales { get; set; }
}