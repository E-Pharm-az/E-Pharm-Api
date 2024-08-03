using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces.Pharma;
using EPharm.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.Pharma;

public class PharmacyStaffRepository(AppDbContext context)
    : Repository<PharmacyStaff>(context), IPharmacyStaffRepository
{
    public async Task<IEnumerable<PharmacyStaff>> GetAllAsync(int pharmacyId) =>
        await Entities.Where(x => x.PharmacyId == pharmacyId).AsNoTracking().ToListAsync();

    public async Task<PharmacyStaff?> GetByExternalIdAsync(string externalId) =>
        await Entities.AsNoTracking().SingleOrDefaultAsync(x => x.ExternalId == externalId);
}
