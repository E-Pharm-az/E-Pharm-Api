using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.PharmaRepositoriesInterfaces;

public interface IPharmaCompanyManagerRepository : IRepository<PharmaCompanyManager>
{
    public Task<IEnumerable<PharmaCompanyManager>> GetAllPharmaCompanyManagersAsync(int companyId);
}
