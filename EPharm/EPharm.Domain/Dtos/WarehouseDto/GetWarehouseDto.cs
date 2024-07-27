namespace EPharm.Domain.Dtos.WarehouseDto;

public class GetWarehouseDto
{
    public int Id { get; set; }
    public int PharmacyId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}
