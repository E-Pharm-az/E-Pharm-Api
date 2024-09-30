using EPharm.Domain.Dtos.SalesDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;

namespace EPharm.Domain.Services.Common;

public class SalesService(IOrderRepository orderRepository) : ISalesService
{
    public async Task<SalesSummaryDto> GetSalesAsync(int pharmacyId, DateTime? startDate, DateTime? endDate, int? frequencyDay = 1)
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(-30);
        var end = endDate ?? DateTime.UtcNow;
        var frequency = frequencyDay ?? 1;

        var sales = await orderRepository.GetAllOrdersByDate(start, end,
            query => query.Where(o => o.OrderProducts.Any(op => op.PharmacyId == pharmacyId)));

        var groupedSales = GroupSalesByFrequency(sales, start, end, frequency);

        return new SalesSummaryDto
        {
            Sales = groupedSales.ToArray(),
            TotalSales = groupedSales.Sum(g => g.Sales),
            TotalOrders = groupedSales.Sum(g => g.Orders)
        };
    }

    private List<SalesDto> GroupSalesByFrequency(IEnumerable<Order> sales, DateTime startDate, DateTime endDate, int frequencyDay)
    {
        var groupedSales = new List<SalesDto>();
        var currentDate = startDate.Date;

        while (currentDate <= endDate)
        {
            var endGroupDate = currentDate.AddDays(frequencyDay - 1);
            var salesInPeriod = sales.Where(s => s.CreatedAt.Date >= currentDate && s.CreatedAt.Date <= endGroupDate).ToList();

            groupedSales.Add(new SalesDto
            {
                Date = currentDate.ToString("yyyy-MM-dd"),
                Sales = salesInPeriod.Sum(s => s.TotalPrice),
                Orders = salesInPeriod.Count
            });

            currentDate = endGroupDate.AddDays(1);
        }

        return groupedSales;
    }
}
