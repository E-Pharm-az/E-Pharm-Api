using EPharm.Domain.Dtos.PharmacyDtos;

namespace EPharm.Domain.Dtos.UserDto;

public class CreatePharmaDto
{
    public CreateUserDto UserRequest { get; set; }
    public CreatePharmacyDto PharmacyRequest { get; set; }
}