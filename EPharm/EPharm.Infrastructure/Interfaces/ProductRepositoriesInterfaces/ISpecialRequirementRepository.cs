using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

public interface ISpecialRequirementRepository : IRepository<SpecialRequirement>
{
    Task<IEnumerable<SpecialRequirement>> GetAllCompanySpecialRequirementsAsync(int pharmaCompanyId);
}
