using AutoMapper;
using EPharm.Domain.Dtos.ProductDtos.ProductDtos;
using EPharm.Domain.Interfaces.Product;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Domain.Services.ProductServices;

public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
{
    public async Task<IEnumerable<GetProductDto>> GetAllProductsAsync()
    {
        var products = await productRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetProductDto>>(products);
    }

    public async Task<GetProductDto?> GetProductByIdAsync(int productId)
    {
        var product = await productRepository.GetByIdAsync(productId);
        return mapper.Map<GetProductDto>(product);
    }

    public async Task<GetProductDto> CreateProductAsync(CreateProductDto productDto)
    {
        var productEntity = mapper.Map<Product>(productDto);
        var product = productRepository.InsertAsync(productEntity);

        var result = await productRepository.SaveChangesAsync();

        if (result > 0)
            return mapper.Map<GetProductDto>(product);

        throw new InvalidOperationException("Failed to create product.");
    }

    public async Task<bool> UpdateProductAsync(CreateProductDto productDto)
    {
        var productEntity = mapper.Map<Product>(productDto);
        productRepository.Update(productEntity);

        var result = await productRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        var product = await productRepository.GetByIdAsync(productId);

        if (product is null)
            return false;

        productRepository.Delete(product);

        var result = await productRepository.SaveChangesAsync();
        return result > 0;
    }
}
