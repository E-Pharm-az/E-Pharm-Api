using EPharm.Infrastructure.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Pharma;

public interface IPharmacyStaffRepository : IRepository<PharmacyStaff>
{
    public Task<IEnumerable<PharmacyStaff>> GetAllAsync(int pharmacyId);
    public Task<PharmacyStaff?> GetByExternalIdAsync(string externalId);
}
