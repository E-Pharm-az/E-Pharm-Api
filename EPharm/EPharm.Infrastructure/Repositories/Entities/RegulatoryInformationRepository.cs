using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.Entities;

public class RegulatoryInformationRepository(AppDbContext context)
    : Repository<RegulatoryInformation>(context), IRegulatoryInformationRepository
{
    public async Task<IEnumerable<RegulatoryInformation>> GetAllCompanyRegulatoryInformationAsync(int pharmaCompanyId) =>
        await Entities.Where(x => x.PharmacyId == pharmaCompanyId).ToListAsync();
}
