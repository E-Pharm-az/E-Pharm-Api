namespace EPharm.Domain.Dtos.PharmacyDtos;

public class CreatePharmacyDto
{
    public string Name { get; set; }
    public string TIN { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public string Address { get; set; }
}
