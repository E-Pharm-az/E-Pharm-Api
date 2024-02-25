namespace PharmaPortalService.Domain.Dtos.PharmaCompanyDtos;

public class GetPharmaCompanyDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string Location { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
}
