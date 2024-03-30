using EPharm.Domain.Dtos.RegulatoryInformationDto;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface IRegulatoryInformationService
{
    Task<IEnumerable<GetRegulatoryInformationDto>> GetAllCompanyRegulatoryInformationAsync(int pharmaCompanyId);
    Task<GetRegulatoryInformationDto?> GetRegulatoryInformationByIdAsync(int regulatoryInformationId);
    Task<GetRegulatoryInformationDto> AddCompanyRegulatoryInformationAsync(int pharmaCompanyId, CreateRegulatoryInformationDto regulatoryInformationDto);
    Task<bool> UpdateCompanyRegulatoryInformationAsync(int regulatoryInformationId, CreateRegulatoryInformationDto regulatoryInformationDto);
    Task<bool> DeleteCompanyRegulatoryInformationAsync(int regulatoryInformationId);
}
