using EPharm.Domain.Dtos.ProductDtos.SpecialRequirementsDto;

namespace EPharm.Domain.Interfaces.Product;

public interface ISpecialRequirementService
{
    Task<IEnumerable<GetSpecialRequirementDto>> GetAllCompanySpecialRequirementsAsync(int pharmaCompanyId);
    Task<GetSpecialRequirementDto?> GetSpecialRequirementByIdAsync(int specialRequirementId);
    Task<GetSpecialRequirementDto> AddCompanySpecialRequirementAsync(int pharmaCompanyId, CreateSpecialRequirementDto specialRequirementDto);
    Task<bool> UpdateCompanySpecialRequirementAsync(int specialRequirementId, CreateSpecialRequirementDto specialRequirement);
    Task<bool> DeleteCompanySpecialRequirementAsync(int specialRequirementId);
}
