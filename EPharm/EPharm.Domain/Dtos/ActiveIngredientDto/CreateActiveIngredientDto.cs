using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.ActiveIngredientDto;

public class CreateActiveIngredientDto
{
    [Required]
    public string IngredientName { get; set; }
    [Required]
    public string IngredientDescription { get; set; }
}
