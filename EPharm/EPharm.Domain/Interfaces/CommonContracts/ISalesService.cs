using EPharm.Domain.Dtos.SalesDto;

namespace EPharm.Domain.Interfaces.CommonContracts;

public interface ISalesService
{
    public Task<SalesSummaryDto> GetSalesAsync(int pharmacyId, DateTime? startDate, DateTime? endDate, int? frequency);
}
