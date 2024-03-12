using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.RegulatoryInformationDto;

public class CreateRegulatoryInformationDto
{
    [Required]
    public string RegulatoryStandards { get; set; }
    [Required]
    public DateTime ApprovalDate { get; set; }
    [Required]
    public string Certification { get; set; }  
}
