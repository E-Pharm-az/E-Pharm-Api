using AutoMapper;
using EPharm.Domain.Dtos.ProductDtos;
using EPharm.Domain.Dtos.WarehouseDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Domain.Services.ProductServices;

public class ProductService(
    IUnitOfWork unitOfWork,
    IIndicationProductRepository indicationProductRepository,
    IProductRepository productRepository,
    IProductImageService productImageService,
    IProductActiveIngredientRepository productActiveIngredientRepository,
    IProductAllergyRepository productAllergyRepository,
    IProductDosageFormRepository productDosageFormRepository,
    IProductRouteOfAdministrationRepository productRouteOfAdministrationRepository,
    IProductSideEffectRepository productSideEffectRepository,
    IProductUsageWarningRepository productUsageWarningRepository,
    IWarehouseProductRepository warehouseProductRepository,
    IMapper mapper) : IProductService
{
    public async Task<IEnumerable<GetMinimalProductDto>> SearchProduct(string parameter, int page)
    {
        var allProducts = await productRepository.GetAlLProductsAsync(page, pageSize: 30);

        var filteredProducts = allProducts.Where(product =>
            product.Name.Contains(parameter, StringComparison.OrdinalIgnoreCase) ||
            product.Description.Contains(parameter, StringComparison.OrdinalIgnoreCase)
        );

        return mapper.Map<IEnumerable<GetMinimalProductDto>>(filteredProducts);
    }
    
    public async Task<IEnumerable<GetMinimalProductDto>> GetAllProductsAsync()
    {
        var products = await productRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetMinimalProductDto>>(products);
    }

    public async Task<IEnumerable<GetMinimalProductDto>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page)
    {
        var products = await productRepository.GetAllPharmaCompanyProductsAsync(pharmaCompanyId, page, pageSize: 30);
        return mapper.Map<IEnumerable<GetMinimalProductDto>>(products);
    }

    public async Task<GetFullProductDto?> GetProductByIdAsync(int productId)
    {
        var product = await productRepository.GetFullProductDetailAsync(productId);
        return mapper.Map<GetFullProductDto>(product);
    }

    public async Task<GetMinimalProductDto> CreateProductAsync(int pharmaCompanyId, CreateProductDto productDto)
    {
        try
        {
            var productEntity = mapper.Map<Product>(productDto);
            productEntity.PharmaCompanyId = pharmaCompanyId;
            
            if (productDto.Image is not null)
                productEntity.ImageUrl =  await productImageService.UploadProductImageAsync(productDto.Image);
            
            await unitOfWork.BeginTransactionAsync();
            
            var product = await productRepository.InsertAsync(productEntity);

            foreach (var stock in productDto.Stocks)
            {
                await warehouseProductRepository.InsertWarehouseProductAsync(new WarehouseProduct
                    {
                        ProductId = product.Id,
                        WarehouseId = stock.WarehouseId,
                        Quantity = stock.Quantity
                    });
            }
            
            await productActiveIngredientRepository.InsertProductActiveIngredientAsync(product.Id, productDto.ActiveIngredientsIds);
            await productAllergyRepository.InsertProductAllergyAsync(product.Id, productDto.AllergiesIds);
            await productDosageFormRepository.InsertProductDosageFormAsync(product.Id, productDto.DosageFormsIds);
            await indicationProductRepository.InsertIndicationProductAsync(product.Id, productDto.IndicationsIds);
            await productRouteOfAdministrationRepository.InsertProductRouteOfAdministrationAsync(product.Id, productDto.RouteOfAdministrationsIds);
            await productSideEffectRepository.InsertProductSideEffectAsync(product.Id, productDto.SideEffectsIds);
            await productUsageWarningRepository.InsertProductUsageWarningAsync(product.Id, productDto.UsageWarningsIds);
                
            await unitOfWork.CommitTransactionAsync();
            await unitOfWork.SaveChangesAsync();

            return mapper.Map<GetMinimalProductDto>(product);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> UpdateProductAsync(int id, CreateProductDto productDto)
    {
        var product = await productRepository.GetByIdAsync(id);

        if (product is null)
            return false;

        mapper.Map(productDto, product);
        productRepository.Update(product);

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
