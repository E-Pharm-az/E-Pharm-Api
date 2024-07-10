namespace EPharm.Domain.Dtos.PharmacyStaffDto;

public class CreatePharmacyStaffDto
{
    public string ExternalId { get; set; }
    public string Email { get; set; }
    public int? PharmacyId { get; set; }
}
