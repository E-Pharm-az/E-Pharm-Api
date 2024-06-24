using EPharm.Domain.Interfaces.CommonContracts;

namespace EPharm.Domain.Services.Common;

public class EmailService : IEmailService
{
    private readonly Dictionary<string, string?> _emails = new();

    public async Task CompileEmail(string emailName, string emailPath)
    {
        var email = await File.ReadAllTextAsync(emailPath);
        _emails.Add(emailName, email);
    }

    public string? GetEmail(string emailName)
    {
        return _emails.GetValueOrDefault(emailName);
    }
}