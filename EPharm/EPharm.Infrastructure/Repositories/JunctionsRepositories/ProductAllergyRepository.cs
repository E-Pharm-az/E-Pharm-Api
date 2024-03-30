using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class ProductAllergyRepository(AppDbContext context, IAllergyRepository allergyRepository)
    : Repository<ProductAllergy>(context), IProductAllergyRepository
{
    public async Task InsertProductAllergyAsync(int productId, int[] allergiesIds)
    {
        foreach (var allergiesId in allergiesIds)
        {
            var allergy = await allergyRepository.GetByIdAsync(allergiesId);
            
            if (allergy is null)
                throw new ArgumentException("Allergy not found");
            
            await Entities.AddAsync(
                new ProductAllergy
                {
                    ProductId = productId,
                    AllergyId = allergiesId
                }
            );
        }
        
        await base.SaveChangesAsync();
    }
}
