using EPharm.Domain.Dtos.PharmaCompanyDtos;

namespace EPharm.Domain.Interfaces;

public interface IPharmaCompanyService
{
    Task<IEnumerable<GetPharmaCompanyDto>> GetAllPharmaCompaniesAsync();
    Task<GetPharmaCompanyDto?> GetPharmaCompanyByIdAsync(int pharmaCompanyId);
    Task<GetPharmaCompanyDto> CreatePharmaCompanyAsync(CreatePharmaCompanyDto pharmaCompanyDto);
    Task<bool> UpdatePharmaCompanyAsync(CreatePharmaCompanyDto pharmaCompanyDto);
    Task<bool> DeletePharmaCompanyAsync(int pharmaCompanyId);   
}
