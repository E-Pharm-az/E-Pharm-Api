using EPharm.Domain.Dtos.WarehouseDto;

namespace EPharm.Domain.Dtos.StockDto;

public class GetStockDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public GetWarehouseDto Warehouse { get; set; }
    public int Quantity { get; set; }
}
