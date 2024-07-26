using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Junctions;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;

namespace EPharm.Infrastructure.Repositories.Junctions;

public class ProductDosageFormRepository(AppDbContext context, IDosageFormRepository dosageFormRepository)
    : Repository<ProductDosageForm>(context), IProductDosageFormRepository
{
    public async Task InsertAsync(int productId, int[] dosageFormsIds)
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
