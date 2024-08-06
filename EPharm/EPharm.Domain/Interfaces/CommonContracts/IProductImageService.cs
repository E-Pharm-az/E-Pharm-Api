using Microsoft.AspNetCore.Http;

namespace EPharm.Domain.Interfaces.CommonContracts;

public interface IProductImageService
{
    public Task<string> UploadProductImageAsync(byte[] imageData);
    public Task<bool> DeleteProductImageAsync(string imageUrl);
}
