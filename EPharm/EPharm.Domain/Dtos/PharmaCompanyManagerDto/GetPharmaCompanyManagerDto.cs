namespace EPharm.Domain.Dtos.PharmaCompanyManagerDto;

public class GetPharmaCompanyManagerDto
{
    public int Id { get; set; }
    public string ExternalId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int PharmaCompanyId { get; set; }
}
