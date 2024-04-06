using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.SideEffectDto;

public class CreateSideEffectDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
}
