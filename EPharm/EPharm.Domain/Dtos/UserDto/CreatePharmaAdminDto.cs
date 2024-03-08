using System.ComponentModel.DataAnnotations;
using EPharm.Domain.Dtos.PharmaCompanyDtos;

namespace EPharm.Domain.Dtos.UserDto;

public class CreatePharmaAdminDto
{
    [Required]
    public CreateUserDto UserRequest { get; set; }
    
    [Required]
    public CreatePharmaCompanyDto PharmaCompanyRequest { get; set; } 
}
