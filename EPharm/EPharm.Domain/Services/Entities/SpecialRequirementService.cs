using AutoMapper;
using EPharm.Domain.Dtos.SpecialRequirementsDto;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;

namespace EPharm.Domain.Services.Entities;

public class SpecialRequirementService(ISpecialRequirementRepository specialRequirementRepository, IMapper mapper)
    : ISpecialRequirementService
{
    public async Task<IEnumerable<GetSpecialRequirementDto>> GetAllCompanySpecialRequirementsAsync(int pharmaCompanyId)
    {
        var specialRequirements =
            await specialRequirementRepository.GetAllCompanySpecialRequirementsAsync(pharmaCompanyId);
        return mapper.Map<IEnumerable<GetSpecialRequirementDto>>(specialRequirements);
    }

    public async Task<GetSpecialRequirementDto?> GetSpecialRequirementByIdAsync(int specialRequirementId)
    {
        var specialRequirement = await specialRequirementRepository.GetByIdAsync(specialRequirementId);
        return mapper.Map<GetSpecialRequirementDto>(specialRequirement);
    }

    public async Task<GetSpecialRequirementDto> AddCompanySpecialRequirementAsync(int pharmaCompanyId,
        CreateSpecialRequirementDto specialRequirementDto)
    {
        try
        {
            var specialRequirementEntity = mapper.Map<SpecialRequirement>(specialRequirementDto);
            specialRequirementEntity.PharmacyId = pharmaCompanyId;
            var specialRequirement = await specialRequirementRepository.InsertAsync(specialRequirementEntity);

            return mapper.Map<GetSpecialRequirementDto>(specialRequirement);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create special requirement. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdateCompanySpecialRequirementAsync(int specialRequirementId,
        CreateSpecialRequirementDto specialRequirement)
    {
        var specialRequirementEntity = await specialRequirementRepository.GetByIdAsync(specialRequirementId);

        if (specialRequirementEntity is null)
            return false;

        mapper.Map(specialRequirement, specialRequirementEntity);
        specialRequirementRepository.Update(specialRequirementEntity);

        var result = await specialRequirementRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteCompanySpecialRequirementAsync(int specialRequirementId)
    {
        var specialRequirement = await specialRequirementRepository.GetByIdAsync(specialRequirementId);

        if (specialRequirement is null)
            return false;

        specialRequirementRepository.Delete(specialRequirement);
        var result = await specialRequirementRepository.SaveChangesAsync();
        return result > 0;
    }
}