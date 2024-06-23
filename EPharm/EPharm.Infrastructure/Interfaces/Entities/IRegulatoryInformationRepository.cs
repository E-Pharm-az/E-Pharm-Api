using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Entities;

public interface IRegulatoryInformationRepository : IRepository<RegulatoryInformation>
{
    Task<IEnumerable<RegulatoryInformation>> GetAllCompanyRegulatoryInformationAsync(int pharmaCompanyId);
}
