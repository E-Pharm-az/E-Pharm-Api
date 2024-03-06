using EPharm.Domain.Dtos.PharmaCompanyManagerDto;

namespace EPharm.Domain.Interfaces.Pharma;

public interface IPharmaCompanyManagerService
{
    public Task<IEnumerable<GetPharmaCompanyManagerDto>> GetAllPharmaCompanyManagersAsync();
    public Task<GetPharmaCompanyManagerDto?> GetPharmaCompanyManagerByIdAsync(int pharmaCompanyManagerId);
    public Task<GetPharmaCompanyManagerDto> CreatePharmaCompanyManagerAsync(CreatePharmaCompanyManagerDto pharmaCompanyManagerDto);
    public Task<bool> UpdatePharmaCompanyManagerAsync(int id, CreatePharmaCompanyManagerDto pharmaCompanyManagerDto);
    public Task<bool> DeletePharmaCompanyManagerAsync(int pharmaCompanyManagerId);   
}
