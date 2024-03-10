using EPharm.Domain.Dtos.ProductDtos.RegulatoryInformationDto;

namespace EPharm.Domain.Interfaces.Product;

public interface IRegulatoryInformationService
{
    Task<IEnumerable<GetRegulatoryInformationDto>> GetAllCompanyRegulatoryInformationAsync(int pharmaCompanyId);
    Task<GetRegulatoryInformationDto?> GetRegulatoryInformationByIdAsync(int regulatoryInformationId);
    Task<GetRegulatoryInformationDto> AddCompanyRegulatoryInformationAsync(int pharmaCompanyId, CreateRegulatoryInformationDto regulatoryInformationDto);
    Task<bool> UpdateCompanyRegulatoryInformationAsync(int regulatoryInformationId, CreateRegulatoryInformationDto regulatoryInformationDto);
    Task<bool> DeleteCompanyRegulatoryInformationAsync(int regulatoryInformationId);
}
