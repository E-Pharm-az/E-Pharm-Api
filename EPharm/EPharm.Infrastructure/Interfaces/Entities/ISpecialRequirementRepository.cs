using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Entities;

public interface ISpecialRequirementRepository : IRepository<SpecialRequirement>
{
    Task<IEnumerable<SpecialRequirement>> GetAllCompanySpecialRequirementsAsync(int pharmaCompanyId);
}
