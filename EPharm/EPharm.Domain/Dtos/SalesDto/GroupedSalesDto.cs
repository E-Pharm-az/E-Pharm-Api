namespace EPharm.Domain.Dtos.SalesDto;
public class GroupedSalesDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int OrderCount { get; set; }
}
