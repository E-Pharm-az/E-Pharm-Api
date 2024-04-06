using System.ComponentModel.DataAnnotations;
using EPharm.Domain.Dtos.PharmaCompanyDtos;

namespace EPharm.Domain.Dtos.UserDto;

public class CreatePharmaAdminDto
{
    public CreateUserDto UserRequest { get; set; }
    
    public CreatePharmaCompanyDto PharmaCompanyRequest { get; set; } 
}
