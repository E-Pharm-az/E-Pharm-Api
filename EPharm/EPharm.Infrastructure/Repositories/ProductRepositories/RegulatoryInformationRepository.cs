using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class RegulatoryInformationRepository(AppDbContext context)
    : Repository<RegulatoryInformation>(context), IRegulatoryInformationRepository
{
    public async Task<IEnumerable<RegulatoryInformation>> GetAllCompanyRegulatoryInformationAsync(int pharmaCompanyId) =>
        await Entities.Where(x => x.PharmaCompanyId == pharmaCompanyId).ToListAsync();
}
