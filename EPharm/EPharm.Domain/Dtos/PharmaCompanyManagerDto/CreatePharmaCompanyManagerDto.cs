namespace EPharm.Domain.Dtos.PharmaCompanyManagerDto;

public class CreatePharmaCompanyManagerDto
{
    public string ExternalId { get; set; }
    public int PharmaCompanyId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
