namespace EPharm.Domain.Dtos.RegulatoryInformationDto;

public class GetRegulatoryInformationDto
{
    public int Id { get; set; }
    public int PharmaCompanyId { get; set; }
    public string Name { get; set; }
    public DateTime ApprovalDate { get; set; }
    public string Certification { get; set; }
}