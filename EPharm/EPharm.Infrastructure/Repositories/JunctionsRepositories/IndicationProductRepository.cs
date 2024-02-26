using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class IndicationProductRepository : Repository<IndicationProduct>, IIndicationProductRepository
{
    protected IndicationProductRepository(AppDbContext context) : base(context)
    {
    }
}
