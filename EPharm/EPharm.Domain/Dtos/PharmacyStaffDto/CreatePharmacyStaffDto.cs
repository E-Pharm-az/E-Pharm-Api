namespace EPharm.Domain.Dtos.PharmacyStaffDto;

public class CreatePharmacyStaffDto
{
    public string ExternalId { get; set; }
    public int? PharmaCompanyId { get; set; }
    public string Email { get; set; }
}