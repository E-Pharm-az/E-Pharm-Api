namespace EPharm.Domain.Dtos.RegulatoryInformationDto;

public class GetRegulatoryInformationDto
{
    public int Id { get; set; }
    public int PharmaCompanyId { get; set; }
    public string RegulatoryStandards { get; set; }
    public DateTime ApprovalDate { get; set; }
    public string Certification { get; set; } 
}
