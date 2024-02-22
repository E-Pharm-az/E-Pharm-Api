using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;
using PharmaPortalService.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.JunctionsRepositories;

public class IndicationProductRepository : Repository<IndicationProduct>, IIndicationProductRepository
{
    protected IndicationProductRepository(AppDbContext context) : base(context)
    {
    }
}
