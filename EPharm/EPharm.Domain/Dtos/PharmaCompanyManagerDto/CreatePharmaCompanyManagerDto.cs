namespace EPharm.Domain.Dtos.PharmaCompanyManagerDto;

public class CreatePharmaCompanyManagerDto
{
    public string ExternalId { get; set; }
    public int? PharmaCompanyId { get; set; }
    public string Email { get; set; }
}
