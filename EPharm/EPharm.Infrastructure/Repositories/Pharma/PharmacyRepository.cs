using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces.Pharma;
using EPharm.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.Pharma;

public class PharmacyRepository(AppDbContext context) : Repository<Pharmacy>(context), IPharmacyRepository
{
    public override async Task<Pharmacy?> GetByIdAsync(int id) =>
        await Entities
            .Include(p => p.PharmacyStaff)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Pharmacy?> GetByOwnerId(string id) =>
        await Entities
            .Where(p => p.OwnerId == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    
    public override async Task<IEnumerable<Pharmacy>> GetAllAsync() =>
        await Entities
            .AsNoTracking()
            .Include(p => p.PharmacyStaff)
            .ToListAsync();
}