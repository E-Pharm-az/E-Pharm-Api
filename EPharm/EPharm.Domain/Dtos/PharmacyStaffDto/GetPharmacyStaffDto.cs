namespace EPharm.Domain.Dtos.PharmacyStaffDto;

public class GetPharmacyStaffDto
{
    public int Id { get; set; }
    public string ExternalId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int PharmaCompanyId { get; set; }
}