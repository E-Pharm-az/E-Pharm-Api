using EPharm.Domain.Dtos.UserDto;

namespace EPharm.Domain.Dtos.PharmacyDtos;

public class GetPharmacyDto
{
    public int Id { get; set; }
    public GetUserDto Owner { get; set; }
    public string TIN { get; set; }
    public string Name { get; set; }
    public string OwnerEmail { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Website { get; set; }
    public string Address { get; set; }
    public bool IsActive { get; set; }
}
