using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;
using PharmaPortalService.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.JunctionsRepositories;

public class ProductDosageFormRepository : Repository<ProductDosageForm>, IProductDosageFormRepository
{
    protected ProductDosageFormRepository(AppDbContext context) : base(context)
    {
    }
}
