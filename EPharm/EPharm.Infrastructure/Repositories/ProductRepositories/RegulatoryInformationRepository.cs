using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class RegulatoryInformationRepository : Repository<RegulatoryInformation>, IRegulatoryInformationRepository
{
    protected RegulatoryInformationRepository(AppDbContext context) : base(context)
    {
    }
}
