using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.UsageWarningDto;

public class CreateUsageWarningDto
{
    [Required]
    public string Description { get; set; }
}
