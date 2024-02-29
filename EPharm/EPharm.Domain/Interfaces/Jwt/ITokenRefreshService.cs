namespace EPharm.Domain.Interfaces.Jwt;

public interface ITokenRefreshService
{
    public string RefreshToken();
}
