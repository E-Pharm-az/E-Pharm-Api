using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.DosageFormDto;

public class CreateDosageFormDto
{
    [Required]
    public string DosageFormName { get; set; }
}
