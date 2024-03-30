using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class ProductDosageFormRepository(AppDbContext context, IDosageFormRepository dosageFormRepository)
    : Repository<ProductDosageForm>(context), IProductDosageFormRepository
{
    public async Task InsertProductDosageFormAsync(int productId, int[] dosageFormsIds)
    {
        foreach (var dosageFormsId in dosageFormsIds)
        {
            var dosageForm = await dosageFormRepository.GetByIdAsync(dosageFormsId);
            
            if (dosageForm is null)
                throw new ArgumentException("Dosage form not found");
            
            await Entities.AddAsync(
                new ProductDosageForm
                {
                    ProductId = productId,
                    DosageFormId = dosageFormsId
                }
            );
        }
        
        await base.SaveChangesAsync();
    }
}
