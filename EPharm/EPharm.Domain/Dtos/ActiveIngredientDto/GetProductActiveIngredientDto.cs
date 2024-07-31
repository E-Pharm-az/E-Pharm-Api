namespace EPharm.Domain.Dtos.ActiveIngredientDto;

public class GetProductActiveIngredientDto : GetActiveIngredientDto
{
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
}