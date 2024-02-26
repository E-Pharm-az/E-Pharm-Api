using EPharm.Domain.Dtos.PharmaCompanyManagerDto;

namespace EPharm.Domain.Interfaces;

public interface IPharmaCompanyManagerService
{
    Task<IEnumerable<GetPharmaCompanyManagerDto>> GetAllPharmaCompanyManagersAsync();
    Task<GetPharmaCompanyManagerDto?> GetPharmaCompanyManagerByIdAsync(int pharmaCompanyManagerId);
    Task<GetPharmaCompanyManagerDto> CreatePharmaCompanyManagerAsync(CreatePharmaCompanyManagerDto pharmaCompanyManagerDto);
    Task<bool> UpdatePharmaCompanyManagerAsync(CreatePharmaCompanyManagerDto pharmaCompanyManagerDto);
    Task<bool> DeletePharmaCompanyManagerAsync(int pharmaCompanyManagerId);   
}
