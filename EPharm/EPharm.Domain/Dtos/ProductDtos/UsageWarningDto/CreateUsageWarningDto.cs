using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.ProductDtos.UsageWarningDto;

public class CreateUsageWarningDto
{
    [Required]
    public string Description { get; set; }
}
