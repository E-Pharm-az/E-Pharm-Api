using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class ProductDosageFormRepository : Repository<ProductDosageForm>, IProductDosageFormRepository
{
    protected ProductDosageFormRepository(AppDbContext context) : base(context)
    {
    }
}
