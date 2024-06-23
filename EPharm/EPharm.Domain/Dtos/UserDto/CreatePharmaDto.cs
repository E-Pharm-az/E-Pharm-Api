using EPharm.Domain.Dtos.PharmaCompanyDtos;

namespace EPharm.Domain.Dtos.UserDto;

public class CreatePharmaDto
{
    public CreateUserDto UserRequest { get; set; }
    public CreatePharmaCompanyDto PharmaCompanyRequest { get; set; } 
}
