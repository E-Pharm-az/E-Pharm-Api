using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.ProductDtos.RouteOfAdministrationDto;

public class CreateRouteOfAdministrationDto
{
    [Required]
    public string Description { get; set; }
}
