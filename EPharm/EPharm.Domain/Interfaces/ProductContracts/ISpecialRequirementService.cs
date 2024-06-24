using EPharm.Domain.Dtos.SpecialRequirementsDto;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface ISpecialRequirementService
{
    Task<IEnumerable<GetSpecialRequirementDto>> GetAllCompanySpecialRequirementsAsync(int pharmaCompanyId);
    Task<GetSpecialRequirementDto?> GetSpecialRequirementByIdAsync(int specialRequirementId);

    Task<GetSpecialRequirementDto> AddCompanySpecialRequirementAsync(int pharmaCompanyId,
        CreateSpecialRequirementDto specialRequirementDto);

    Task<bool> UpdateCompanySpecialRequirementAsync(int specialRequirementId,
        CreateSpecialRequirementDto specialRequirement);

    Task<bool> DeleteCompanySpecialRequirementAsync(int specialRequirementId);
}