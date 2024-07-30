using AutoMapper;
using EPharm.Domain.Dtos.ProductDtos;
using EPharm.Domain.Dtos.WarehouseDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Infrastructure.Entities.Identity;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;
using EPharm.Infrastructure.Interfaces.Junctions;
using EPharm.Infrastructure.Interfaces.Entities;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;

namespace EPharm.Domain.Services.Entities;

public class ProductService(
    IUnitOfWork unitOfWork,
    UserManager<AppIdentityUser> userManager,
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
    public async Task<IEnumerable<GetMinimalProductDto>> GetAllProductsAsync(int page)
    {
        var products = await productRepository.GetAlLProductsAsync(page, pageSize: 30);
        return mapper.Map<IEnumerable<GetMinimalProductDto>>(products);
    }

    public async Task<IEnumerable<GetMinimalProductDto>> SearchProduct(string parameter, int page)
    {
        var allProducts = await productRepository.GetAlLApprovedProductsAsync(page, pageSize: 30);

        var filteredProducts = allProducts.Where(product =>
            product.Name.Contains(parameter, StringComparison.OrdinalIgnoreCase) ||
            product.Description.Contains(parameter, StringComparison.OrdinalIgnoreCase)
        );

        return mapper.Map<IEnumerable<GetMinimalProductDto>>(filteredProducts);
    }

    public async Task<IEnumerable<GetMinimalProductDto>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page)
    {
        var products = await productRepository.GetAllPharmaCompanyProductsAsync(pharmaCompanyId, page, pageSize: 30);
        return mapper.Map<IEnumerable<GetMinimalProductDto>>(products);
    }

    public async Task<GetFullProductDto?> GetProductByIdAsync(int productId)
    {
        var product = await productRepository.GetFullByIdAsync(productId);
        return mapper.Map<GetFullProductDto>(product);
    }

    public async Task ApproveProductAsync(string adminId, int productId)
    {
        var user = await userManager.FindByIdAsync(adminId);
        ArgumentNullException.ThrowIfNull(user);

        var product = await productRepository.GetByIdAsync(productId);
        ArgumentNullException.ThrowIfNull(product);

        product.IsApproved = true;
        product.ApprovedByAdminId = adminId;
        
        productRepository.Update(product);
        await productRepository.SaveChangesAsync();
    }

    public async Task<GetMinimalProductDto> CreateProductAsync(int pharmaCompanyId, CreateProductDto productDto)
    {
        try
        {
            var productEntity = mapper.Map<Product>(productDto);
            productEntity.PharmacyId = pharmaCompanyId;

            if (productDto.Image is { Length: > 0 })
                productEntity.ImageUrl = await productImageService.UploadProductImageAsync(productDto.Image);

            productEntity.ImageUrl = productDto.ImageURL;

            await unitOfWork.BeginTransactionAsync();

            var product = await productRepository.InsertAsync(productEntity);

            foreach (var stock in productDto.Stocks)
            {
                await warehouseProductRepository.InsertAsync(new WarehouseProduct
                {
                    ProductId = product.Id,
                    WarehouseId = stock.WarehouseId,
                    Quantity = stock.Quantity
                });
            }

            await productActiveIngredientRepository.InsertAsync(product.Id, productDto.ActiveIngredientsIds);
            if (productDto.AllergiesIds is not null)
                await productAllergyRepository.InsertAsync(product.Id, productDto.AllergiesIds);
            
            await productDosageFormRepository.InsertAsync(product.Id, productDto.DosageFormsIds);

            if (productDto.IndicationsIds is not null)
                await indicationProductRepository.InsertAsync(product.Id, productDto.IndicationsIds);
            
            await productRouteOfAdministrationRepository.InsertAsync(product.Id, productDto.RouteOfAdministrationsIds);
            
            if (productDto.SideEffectsIds is not null)
                await productSideEffectRepository.InsertAsync(product.Id, productDto.SideEffectsIds);
            
            if (productDto.UsageWarningsIds is not null)
                await productUsageWarningRepository.InsertAsync(product.Id, productDto.UsageWarningsIds);

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

    public async Task<bool> UpdateProductAsync(int pharmacyId, int id, CreateProductDto productDto)
    {
        var product = await productRepository.GetByIdAsync(id);

        if (product is null)
            return false;
        
        if (product.PharmacyId != pharmacyId)
            return false;

        mapper.Map(productDto, product);
        product.IsApproved = false;
        productRepository.Update(product);

        var result = await productRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteProductAsync(int pharmacyId, int productId)
    {
        var product = await productRepository.GetByIdAsync(productId);

        if (product is null)
            return false;
        
        if (product.PharmacyId != pharmacyId)
            return false;

        productRepository.Delete(product);

        var result = await productRepository.SaveChangesAsync();
        return result > 0;
    }
}
