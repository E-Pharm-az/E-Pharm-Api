using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Domain.Dtos.UserDto;

namespace EPharm.Domain.Interfaces.PharmaContracts;

public interface IPharmacyStaffService
{
    public Task<IEnumerable<GetPharmaCompanyManagerDto>> GetAllPharmaCompanyManagersAsync(int pharmaCompanyId);
    public Task<GetPharmaCompanyManagerDto?> GetPharmaCompanyManagerByIdAsync(int pharmaCompanyManagerId);
    public Task<GetPharmaCompanyManagerDto?> GetPharmaCompanyManagerByExternalIdAsync(string externalId);
    public Task<GetPharmaCompanyManagerDto> CreatePharmaCompanyManagerAsync(CreatePharmaCompanyManagerDto pharmaCompanyManagerDto);
    public Task<GetPharmaCompanyManagerDto> CreatePharmaManagerAsync(int pharmaCompanyId, EmailDto emailDto);
    public Task<bool> UpdatePharmaCompanyManagerAsync(int id, CreatePharmaCompanyManagerDto pharmaCompanyManagerDto);
    public Task<bool> DeletePharmaCompanyManagerAsync(int pharmaCompanyManagerId);   
}
