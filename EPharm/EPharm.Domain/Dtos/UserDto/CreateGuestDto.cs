namespace EPharm.Domain.Dtos.UserDto;

public class CreateGuestDto : EmailDto
{
    public string PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
