using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class SideEffectRepository : Repository<SideEffect>, ISideEffectRepository
{
    protected SideEffectRepository(AppDbContext context) : base(context)
    {
    }
}
