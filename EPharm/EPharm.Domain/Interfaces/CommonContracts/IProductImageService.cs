using Microsoft.AspNetCore.Http;

namespace EPharm.Domain.Interfaces.CommonContracts;

public interface IProductImageService
{
    public Task<string> UploadProductImageAsync(IFormFile imageData);
    public Task<bool> DeleteProductImageAsync(string imageUrl);
}
