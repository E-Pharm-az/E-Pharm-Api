using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces.PharmaRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.PharmaRepositories;

public class PharmaCompanyManagerRepository(AppDbContext context)
    : Repository<PharmaCompanyManager>(context), IPharmaCompanyManagerRepository
{
    public async Task<IEnumerable<PharmaCompanyManager>> GetAllPharmaCompanyManagersAsync(int companyId) =>
        await Entities.Where(x => x.PharmaCompanyId == companyId).ToListAsync();
}
