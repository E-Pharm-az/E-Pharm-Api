namespace EPharm.Domain.Dtos.ActiveIngredientDto;

public class CreateActiveIngredientDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Quantity { get; set; }
}