using System.Text;

namespace EPharm.Domain.Services.CommonServices;

public static class RandomCodeGenerator
{
    private static readonly Random Random = new();

    public static string GenerateCode(int length = 6)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var result = new StringBuilder(length);
        
        for (var i = 0; i < length; i++)
        {
            result.Append(chars[Random.Next(chars.Length)]);
        }

        return result.ToString();
    }
}
