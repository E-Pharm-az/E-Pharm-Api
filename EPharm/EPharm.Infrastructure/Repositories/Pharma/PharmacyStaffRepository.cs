using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces.PharmaRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.Pharmacy;

public class PharmacyStaffRepository(AppDbContext context)
    : Repository<PharmacyStaff>(context), IPharmacyStaffRepository
{
    public async Task<IEnumerable<PharmacyStaff>> GetAllPharmaCompanyManagersAsync(int companyId) =>
        await Entities.Where(x => x.PharmaCompanyId == companyId).AsNoTracking().ToListAsync();


    public async Task<PharmacyStaff?> GetPharmaCompanyManagerByExternalIdAsync(string externalId) =>
        await Entities.AsNoTracking().SingleOrDefaultAsync(x => x.ExternalId == externalId);
}
