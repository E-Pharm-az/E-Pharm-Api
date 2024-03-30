namespace EPharm.Domain.Interfaces.JwtContracts;

public interface ITokenRefreshService
{
    public string RefreshToken();
}
