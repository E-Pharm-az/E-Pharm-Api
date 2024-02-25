using PharmaPortalService.Domain.Dtos.PharmaCompanyDtos;

namespace PharmaPortalService.Domain.Interfaces;

public interface IPharmaCompanyService
{
    Task<IEnumerable<GetPharmaCompanyDto>> GetAllPharmaCompaniesAsync();
    Task<GetPharmaCompanyDto?> GetPharmaCompanyByIdAsync(int pharmaCompanyId);
    Task<GetPharmaCompanyDto> CreatePharmaCompanyAsync(CreatePharmaCompanyDto pharmaCompanyDto);
    Task<bool> UpdatePharmaCompanyAsync(CreatePharmaCompanyDto pharmaCompanyDto);
    Task<bool> DeletePharmaCompanyAsync(int pharmaCompanyId);   
}
