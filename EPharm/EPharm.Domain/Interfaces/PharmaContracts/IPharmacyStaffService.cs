using EPharm.Domain.Dtos.PharmacyStaffDto;
using EPharm.Domain.Dtos.UserDto;

namespace EPharm.Domain.Interfaces.PharmaContracts;

public interface IPharmacyStaffService
{
    public Task<IEnumerable<GetPharmacyStaffDto>> GetAllPharmacyStaffAsync(int pharmaCompanyId);
    public Task<GetPharmacyStaffDto?> GetPharmacyStaffByIdAsync(int pharmaCompanyManagerId);
    public Task<GetPharmacyStaffDto?> GetPharmacyStaffByExternalIdAsync(string externalId);
    public Task<GetPharmacyStaffDto> CreatePharmacyStaffAsync(CreatePharmacyStaffDto pharmacyStaffDto);
    public Task<GetPharmacyStaffDto> CreatePharmacyStaffAsync(int pharmaCompanyId, EmailDto emailDto);
    public Task<bool> UpdatePharmacyStaffAsync(int id, CreatePharmacyStaffDto pharmacyStaffDto);
    public Task<bool> DeletePharmacyStaffAsync(int pharmaCompanyManagerId);
}