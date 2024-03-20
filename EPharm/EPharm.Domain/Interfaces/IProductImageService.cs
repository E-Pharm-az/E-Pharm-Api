using Microsoft.AspNetCore.Http;

namespace EPharm.Domain.Interfaces;

public interface IProductImageService
{
    public Task<string> UploadProductImageAsync(IFormFile image);
    public Task<bool> DeleteProductImageAsync(string imageUrl);
}
