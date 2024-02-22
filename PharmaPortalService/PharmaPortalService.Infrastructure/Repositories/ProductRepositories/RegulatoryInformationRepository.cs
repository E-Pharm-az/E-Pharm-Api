using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;
using PharmaPortalService.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.ProductRepositories;

public class RegulatoryInformationRepository : Repository<RegulatoryInformation>, IRegulatoryInformationRepository
{
    protected RegulatoryInformationRepository(AppDbContext context) : base(context)
    {
    }
}
