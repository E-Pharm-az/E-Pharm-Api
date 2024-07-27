namespace EPharm.Domain.Dtos.ActiveIngredientDto;

public class GetActiveIngredientDto
{
    public int Id { get; set; }
    public int PharmacyId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
