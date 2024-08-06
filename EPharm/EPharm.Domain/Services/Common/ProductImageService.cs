using EPharm.Domain.Interfaces.CommonContracts;
using Imagekit.Sdk;
using Microsoft.Extensions.Configuration;

namespace EPharm.Domain.Services.Common;

public class ProductImageService(IConfiguration configuration) : IProductImageService
{
    private readonly ImagekitClient _imagekitClient = new (configuration["ImageKit:PublicKey"], configuration["ImageKit:PrivateKey"], configuration["ImageKit:UrlEndPoint"]);
    
    public async Task<string> UploadProductImageAsync(byte[] imageData)
    {
        if (imageData == null || imageData.Length == 0)
            throw new ArgumentException("Image data is null or empty", nameof(imageData));

        using var memoryStream = new MemoryStream(imageData);

        var ob = new FileCreateRequest
        {
            file = imageData,
            fileName = Guid.NewGuid().ToString()
        };

        var result = await _imagekitClient.UploadAsync(ob);
        return result.url;
    }

    public async Task<bool> DeleteProductImageAsync(string imageUrl)
    {
        throw new NotImplementedException();
    }
}
