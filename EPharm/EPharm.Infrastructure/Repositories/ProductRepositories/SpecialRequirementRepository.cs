using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class SpecialRequirementRepository(AppDbContext context)
    : Repository<SpecialRequirement>(context), ISpecialRequirementRepository
{
    public async Task<IEnumerable<SpecialRequirement>> GetAllCompanySpecialRequirementsAsync(int pharmaCompanyId) =>
        await Entities.Where(x => x.PharmaCompanyId == pharmaCompanyId).ToListAsync();
}
