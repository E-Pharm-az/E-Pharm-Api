using AutoMapper;
using EPharm.Domain.Dtos.ProductDtos;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Domain.Services.ProductServices;

public class ProductService(
    IUnitOfWork unitOfWork,
    IProductRepository productRepository,
    IProductImageService productImageService,
    IRegulatoryInformationService regulatoryInformationService,
    IProductActiveIngredientRepository productActiveIngredientRepository,
    IProductAllergyRepository productAllergyRepository,
    IProductDosageFormRepository productDosageFormRepository,
    IIndicationProductRepository indicationProductRepository,
    IProductRouteOfAdministrationRepository productRouteOfAdministrationRepository,
    IProductSideEffectRepository productSideEffectRepository,
    IProductUsageWarningRepository productUsageWarningRepository,
    IMapper mapper) : IProductService
{
    public async Task<IEnumerable<GetProductDto>> GetAllProductsAsync()
    {
        var products = await productRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetProductDto>>(products);
    }

    public async Task<IEnumerable<GetProductDto>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId)
    {
        var products = await productRepository.GetAllPharmaCompanyProductsAsync(pharmaCompanyId);
        return mapper.Map<IEnumerable<GetProductDto>>(products);
    }

    public async Task<GetFullProductDto?> GetProductByIdAsync(int productId)
    {
        var product = await productRepository.GetByIdAsync(productId);
        return mapper.Map<GetFullProductDto>(product);
    }

    public async Task<IEnumerable<GetProductDto>> SearchProduct(string parameter)
    {
        var allProducts = await productRepository.GetAllAsync();

        var filteredProducts = allProducts.Where(p =>
            p.Name.Contains(parameter, StringComparison.OrdinalIgnoreCase) ||
            p.Description.Contains(parameter, StringComparison.OrdinalIgnoreCase)
        );

        return mapper.Map<IEnumerable<GetProductDto>>(filteredProducts);
    }

    public async Task<GetProductDto> CreateProductAsync(int pharmaCompanyId, CreateProductDto productDto)
    {
        try
        {
            var productEntity = mapper.Map<Product>(productDto);
            
            if (productDto.ProductImage is not null)
                productEntity.ImageUrl =  await productImageService.UploadProductImageAsync(productDto.ProductImage);
            
            productEntity.PharmaCompanyId = pharmaCompanyId;

            var regulatoryInformation = await regulatoryInformationService.GetRegulatoryInformationByIdAsync(productEntity.RegulatoryInformationId);

            if (regulatoryInformation is null)
                throw new ArgumentException("Regulatory information not found");
            
            var product = await productRepository.InsertAsync(productEntity);

            await unitOfWork.BeginTransactionAsync();
            
            await productActiveIngredientRepository.InsertProductActiveIngredientAsync(product.Id, productDto.ActiveIngredientsIds);
            await productAllergyRepository.InsertProductAllergyAsync(product.Id, productDto.AllergiesIds);
            await productDosageFormRepository.InsertProductDosageFormAsync(product.Id, productDto.DosageFormsIds);
            await indicationProductRepository.InsertIndicationProductAsync(product.Id, productDto.IndicationsIds);
            await productRouteOfAdministrationRepository.InsertProductRouteOfAdministrationAsync(product.Id, productDto.RouteOfAdministrationsIds);
            await productSideEffectRepository.InsertProductSideEffectAsync(product.Id, productDto.SideEffectsIds);
            await productUsageWarningRepository.InsertProductUsageWarningAsync(product.Id, productDto.UsageWarningsIds);
                
            await unitOfWork.CommitTransactionAsync();
            await unitOfWork.SaveChangesAsync();

            return mapper.Map<GetProductDto>(product);
        }
        catch (ArgumentException ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw new ArgumentException($"Failed to create product, detail: {ex}");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create product, detail: {ex}");
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
