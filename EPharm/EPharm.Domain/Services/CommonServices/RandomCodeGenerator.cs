namespace EPharm.Domain.Services.CommonServices;

public static class RandomCodeGenerator
{
    private static readonly Random Random = new();

    public static int GenerateCode()
    {
        return Random.Next(100000, 1000000); // Generates a random 6 code.
    }
}
