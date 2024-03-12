namespace EPharm.Domain.Dtos.ActiveIngredientDto;

public class GetActiveIngredientDto
{
    public int Id { get; set; }
    public int PharmaCompanyId { get; set; }
    public string IngredientName { get; set; }
    public string IngredientDescription { get; set; } 
}