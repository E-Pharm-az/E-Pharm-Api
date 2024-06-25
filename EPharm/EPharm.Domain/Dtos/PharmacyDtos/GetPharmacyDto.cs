namespace EPharm.Domain.Dtos.PharmacyDtos;

public class GetPharmacyDto
{
    public int Id { get; set; }
    public string OwnerId { get; set; }
    public string TIN { get; set; }
    public string Name { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public string Address { get; set; }
}