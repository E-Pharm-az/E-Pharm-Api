using AutoMapper;
using EPharm.Domain.Dtos.AttributeDtos;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;

namespace EPharm.Domain.Services.Entities;

public class AllergyService(IAllergyRepository allergyRepository, IMapper mapper) : IAllergyService
{
    public async Task<IEnumerable<GetAttributeDto>> GetAllAllergiesAsync()
    {
        var allergies = await allergyRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetAttributeDto>>(allergies);
    }

    public async Task<GetAttributeDto?> GetAllergyByIdAsync(int id)
    {
        var allergy = await allergyRepository.GetByIdAsync(id);
        return mapper.Map<GetAttributeDto>(allergy);
    }

    public async Task<GetAttributeDto> CreateAllergyAsync(CreateAttributeDto allergyDto)
    {
        try
        {
            var allergyEntity = mapper.Map<Allergy>(allergyDto);
            var allergy = await allergyRepository.InsertAsync(allergyEntity);

            return mapper.Map<GetAttributeDto>(allergy);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create allergy. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdateAllergyAsync(int id, CreateAttributeDto allergyDto)
    {
        var allergy = await allergyRepository.GetByIdAsync(id);

        if (allergy is null)
            return false;

        mapper.Map(allergyDto, allergy);
        allergyRepository.Update(allergy);

        var result = await allergyRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteAllergyAsync(int id)
    {
        var allergy = await allergyRepository.GetByIdAsync(id);

        if (allergy is null)
            return false;

        allergyRepository.Delete(allergy);
        var result = await allergyRepository.SaveChangesAsync();
        return result > 0;
    }
}
