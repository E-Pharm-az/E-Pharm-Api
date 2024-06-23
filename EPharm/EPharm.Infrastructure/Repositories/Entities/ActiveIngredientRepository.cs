using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.Entities;

public class ActiveIngredientRepository(AppDbContext context)
    : Repository<ActiveIngredient>(context), IActiveIngredientRepository
{
    public async Task<IEnumerable<ActiveIngredient>> GetAllCompanyActiveIngredientsAsync(int pharmaCompanyId) =>
        await Entities.Where(x => x.PharmaCompanyId == pharmaCompanyId).ToListAsync();
}
