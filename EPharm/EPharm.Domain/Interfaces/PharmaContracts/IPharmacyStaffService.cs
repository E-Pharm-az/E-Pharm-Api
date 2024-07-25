using EPharm.Domain.Dtos.PharmacyStaffDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Infrastructure.Entities.Identity;

namespace EPharm.Domain.Interfaces.PharmaContracts;

public interface IPharmacyStaffService
{
    public Task<IEnumerable<GetPharmacyStaffDto>> GetAllAsync(int pharmaCompanyId);
    public Task<GetPharmacyStaffDto?> GetByIdAsync(int pharmaCompanyManagerId);
    public Task<GetPharmacyStaffDto?> GetByExternalIdAsync(string externalId);
    public Task<GetPharmacyStaffDto> CreateAsync(CreatePharmacyStaffDto pharmacyStaffDto);
    public Task<AppIdentityUser> CreateAsync(int pharmacyId, EmailDto emailDto);
    public Task<bool> UpdateAsync(int id, CreatePharmacyStaffDto pharmacyStaffDto);
    public Task<bool> DeleteAsync(int pharmaCompanyManagerId);
    public Task BulkInviteAsync(int pharmaCompanyId, BulkEmailDto bulkEmailDto);
}
