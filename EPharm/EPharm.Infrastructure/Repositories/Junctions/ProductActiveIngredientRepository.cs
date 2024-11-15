using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Junctions;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.Junctions;

public class ProductActiveIngredientRepository(AppDbContext context, IActiveIngredientRepository activeIngredientRepository)
    : Repository<ProductActiveIngredient>(context), IProductActiveIngredientRepository
{
    public async Task<IEnumerable<ProductActiveIngredient>> GetAllAsync(int productId) =>
        await Entities
            .Where(x => x.ProductId == productId)
            .Include(x => x.ActiveIngredient)
            .AsNoTracking()
            .ToListAsync();

    public async Task InsertAsync(int productId, int[] activeIngredientsIds)
    {
        foreach (var activeIngredientsId in activeIngredientsIds)
        {
            var activeIngredient = await activeIngredientRepository.GetByIdAsync(activeIngredientsId);
            
            if (activeIngredient is null)
                throw new ArgumentException("Active ingredient not found");
            
            await Entities.AddAsync(
                new ProductActiveIngredient
                {
                    ProductId = productId,
                    ActiveIngredientId = activeIngredientsId
                }
            );
        }
        
        await base.SaveChangesAsync();
    }
    
}
