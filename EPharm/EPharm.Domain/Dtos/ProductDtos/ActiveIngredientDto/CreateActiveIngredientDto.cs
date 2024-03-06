namespace EPharm.Domain.Dtos.ProductDtos.ActiveIngredientDto;

public class CreateActiveIngredientDto
{
    public int PharmaCompanyId { get; set; }
    public string IngredientName { get; set; }
    public string IngredientDescription { get; set; }
}
