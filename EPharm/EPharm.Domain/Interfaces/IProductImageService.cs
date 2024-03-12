using EPharm.Domain.Dtos.ProductImageDto;

namespace EPharm.Domain.Interfaces;

public interface IProductImageService
{
    public Task<bool> UploadProductImageAsync(CreateProductImageDto productImageDto);
}