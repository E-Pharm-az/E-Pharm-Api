using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.ManufacturerDto;

public class CreateManufacturerDto
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string Website { get; set; }
    public string Email { get; set; }
}
