namespace EPharm.Domain.Dtos.ManufacturerDto;

public class GetManufacturerDto
{
    public int Id { get; set; }
    public int PharmaCompanyId { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string Website { get; set; }
    public string Email { get; set; }
}
