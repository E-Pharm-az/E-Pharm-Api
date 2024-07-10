namespace EPharm.Domain.Dtos.PharmacyStaffDto;

public class GetPharmacyStaffDto
{
    public int Id { get; set; }
    public string ExternalId { get; set; }
    public string Email { get; set; }
    public int PharmacyId { get; set; }
}
