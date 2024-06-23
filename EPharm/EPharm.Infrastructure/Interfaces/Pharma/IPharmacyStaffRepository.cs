using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Pharma;

public interface IPharmacyStaffRepository : IRepository<PharmacyStaff>
{
    public Task<IEnumerable<PharmacyStaff>> GetAllPharmaCompanyManagersAsync(int companyId);
    public Task<PharmacyStaff?> GetPharmaCompanyManagerByExternalIdAsync(string externalId);
}
