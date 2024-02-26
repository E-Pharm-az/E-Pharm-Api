using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class SpecialRequirementRepository : Repository<SpecialRequirement>, ISpecialRequirementRepository
{
    protected SpecialRequirementRepository(AppDbContext context) : base(context)
    {
    }
}
