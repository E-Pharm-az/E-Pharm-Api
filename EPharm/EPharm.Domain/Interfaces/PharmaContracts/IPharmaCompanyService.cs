using EPharm.Domain.Dtos.PharmaCompanyDtos;

namespace EPharm.Domain.Interfaces.PharmaContracts;

public interface IPharmaCompanyService
{
    public Task<IEnumerable<GetPharmaCompanyDto>> GetAllPharmaCompaniesAsync();
    public Task<GetPharmaCompanyDto?> GetPharmaCompanyByIdAsync(int pharmaCompanyId);
    public Task<GetPharmaCompanyDto> CreatePharmaCompanyAsync(CreatePharmaCompanyDto pharmaCompanyDto,
        string pharmaAdminId);
    public Task<bool> UpdatePharmaCompanyAsync(int id, CreatePharmaCompanyDto pharmaCompanyDto);
    public Task<bool> DeletePharmaCompanyAsync(int pharmaCompanyId);   
}
