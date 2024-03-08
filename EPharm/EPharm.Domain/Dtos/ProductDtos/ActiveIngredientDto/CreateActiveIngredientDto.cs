using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.ProductDtos.ActiveIngredientDto;

public class CreateActiveIngredientDto
{
    [Required]
    public int PharmaCompanyId { get; set; }
    [Required]
    public string IngredientName { get; set; }
    [Required]
    public string IngredientDescription { get; set; }
}
