using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class ProductActiveIngredientRepository(AppDbContext context, IActiveIngredientRepository activeIngredientRepository)
    : Repository<ProductActiveIngredient>(context), IProductActiveIngredientRepository
{
    public async Task InsertProductActiveIngredientAsync(int productId, int[] activeIngredientsIds)
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
    }
}
