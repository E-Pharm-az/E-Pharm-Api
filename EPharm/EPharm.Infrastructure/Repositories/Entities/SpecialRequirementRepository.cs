using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.Entities;

public class SpecialRequirementRepository(AppDbContext context)
    : Repository<SpecialRequirement>(context), ISpecialRequirementRepository
{
    public async Task<IEnumerable<SpecialRequirement>> GetAllCompanySpecialRequirementsAsync(int pharmaCompanyId) =>
        await Entities.Where(x => x.PharmaCompanyId == pharmaCompanyId).ToListAsync();
}
