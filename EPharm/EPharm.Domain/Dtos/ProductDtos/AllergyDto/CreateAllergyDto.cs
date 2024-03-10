using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.ProductDtos.AllergyDto;

public class CreateAllergyDto
{
    [Required]
    public string Description { get; set; }
}
