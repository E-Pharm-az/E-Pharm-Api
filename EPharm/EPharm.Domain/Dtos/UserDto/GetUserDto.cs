namespace EPharm.Domain.Dtos.UserDto;

public class GetUserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Address { get; set; }
    public string? District { get; set; }
    public string? City { get; set; }
    public int? Zip { get; set; }
    public DateTime CreatedAt { get; set; }
}
