namespace EPharm.Domain.Dtos.UserDto;

public class InitializeUserDto
{
    public string Email { get; set; }
    public int Code { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Address { get; set; }
    public string Password { get; set; }
}