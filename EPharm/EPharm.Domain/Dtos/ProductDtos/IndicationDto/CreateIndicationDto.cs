using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.ProductDtos.IndicationDto;

public class CreateIndicationDto
{
    [Required]
    public string IndicationsName { get; set; }
    public string IndicationsDescription { get; set; }
}
