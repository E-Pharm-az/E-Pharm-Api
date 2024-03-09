using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.ProductDtos.ManufacturerDto;

public class CreateManufacturerDto
{
    [Required]
    public string ManufacturerName { get; set; }
    [Required]
    public string Country { get; set; }
    [Required]
    public string Website { get; set; }
    [Required]
    public string Email { get; set; }
}
