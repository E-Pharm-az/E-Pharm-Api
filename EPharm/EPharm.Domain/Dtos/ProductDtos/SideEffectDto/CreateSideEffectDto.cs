using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.ProductDtos.SideEffectDto;

public class CreateSideEffectDto
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
}
