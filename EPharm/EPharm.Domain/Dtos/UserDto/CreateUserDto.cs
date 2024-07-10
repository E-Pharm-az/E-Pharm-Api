namespace EPharm.Domain.Dtos.UserDto;

public class CreateUserDto : EmailDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
}
