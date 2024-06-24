namespace EPharm.Domain.Interfaces.CommonContracts;

public interface IEmailService
{
    public Task CompileEmail(string emailName, string emailPath);
    public string? GetEmail(string emailName);
}