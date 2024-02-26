using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class IndicationRepository : Repository<Indication>, IIndicationRepository
{
    protected IndicationRepository(AppDbContext context) : base(context)
    {
    }
}
