using EPharm.Domain.Dtos.UserDto;

namespace EPharm.Domain.Models.Jwt;

public class AuthResponse
{
    public TokenResponse TokenResponse { get; set; }
    public GetUserDto UserResponse { get; set; }
}