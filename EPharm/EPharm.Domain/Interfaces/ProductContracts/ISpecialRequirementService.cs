using EPharm.Domain.Dtos.SpecialRequirementsDto;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface ISpecialRequirementService
{
    Task<IEnumerable<GetSpecialRequirementDto>> GetAllPharmacySpecialRequirementsAsync(int pharmaCompanyId);
    Task<GetSpecialRequirementDto?> GetSpecialRequirementByIdAsync(int specialRequirementId);

    Task<GetSpecialRequirementDto> AddPharmacySpecialRequirementAsync(int pharmaCompanyId,
        CreateSpecialRequirementDto specialRequirementDto);

    Task<bool> UpdatePharmacySpecialRequirementAsync(int specialRequirementId,
        CreateSpecialRequirementDto specialRequirement);

    Task<bool> DeletePharmacySpecialRequirementAsync(int specialRequirementId);
}
