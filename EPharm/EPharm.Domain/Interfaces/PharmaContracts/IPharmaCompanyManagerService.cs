using EPharm.Domain.Dtos.PharmaCompanyManagerDto;

namespace EPharm.Domain.Interfaces.PharmaContracts;

public interface IPharmaCompanyManagerService
{
    public Task<IEnumerable<GetPharmaCompanyManagerDto>> GetAllPharmaCompanyManagersAsync(int pharmaCompanyId);
    public Task<GetPharmaCompanyManagerDto?> GetPharmaCompanyManagerByIdAsync(int pharmaCompanyManagerId);
    public Task<GetPharmaCompanyManagerDto> CreatePharmaCompanyManagerAsync(CreatePharmaCompanyManagerDto pharmaCompanyManagerDto);
    public Task<bool> UpdatePharmaCompanyManagerAsync(int id, CreatePharmaCompanyManagerDto pharmaCompanyManagerDto);
    public Task<bool> DeletePharmaCompanyManagerAsync(int pharmaCompanyManagerId);   
}
