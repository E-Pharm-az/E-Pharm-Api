using AutoMapper;
using EPharm.Domain.Dtos.ActiveIngredientDto;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;

namespace EPharm.Domain.Services.ProductServices;

public class ActiveIngredientService(IActiveIngredientRepository activeIngredientRepository, IMapper mapper)
    : IActiveIngredientService
{
    public async Task<IEnumerable<GetActiveIngredientDto>> GetAllActiveIngredientsAsync()
    {
        var activeIngredients = await activeIngredientRepository.GetAllAsync();
        return mapper.Map<List<GetActiveIngredientDto>>(activeIngredients);
    }

    public async Task<IEnumerable<GetActiveIngredientDto>> GetAllCompanyActiveIngredientsAsync(int pharmaCompanyId)
    {
        var activeIngredients = await activeIngredientRepository.GetAllCompanyActiveIngredientsAsync(pharmaCompanyId);
        return mapper.Map<List<GetActiveIngredientDto>>(activeIngredients);
    }

    public async Task<GetActiveIngredientDto?> GetActiveIngredientByIdAsync(int id)
    {
        var activeIngredient = await activeIngredientRepository.GetByIdAsync(id);
        return mapper.Map<GetActiveIngredientDto>(activeIngredient);
    }

    public async Task<GetActiveIngredientDto> CreateActiveIngredientAsync(int pharmaCompanyId, CreateActiveIngredientDto createActiveIngredientDto)
    {
        try
        {
            var activeIngredientEntity = mapper.Map<ActiveIngredient>(createActiveIngredientDto);
            activeIngredientEntity.PharmaCompanyId = pharmaCompanyId;
            var activeIngredient = await activeIngredientRepository.InsertAsync(activeIngredientEntity);

            return mapper.Map<GetActiveIngredientDto>(activeIngredient);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create active ingredient. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdateActiveIngredientAsync(int id, CreateActiveIngredientDto createActiveIngredientDto)
    {
        var activeIngredient = await activeIngredientRepository.GetByIdAsync(id);

        if (activeIngredient is null)
            return false;

        mapper.Map(createActiveIngredientDto, activeIngredient);
        activeIngredientRepository.Update(activeIngredient);

        var result = await activeIngredientRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteActiveIngredientAsync(int id)
    {
        var activeIngredient = await activeIngredientRepository.GetByIdAsync(id);

        if (activeIngredient is null)
            return false;

        activeIngredientRepository.Delete(activeIngredient);

        var result = await activeIngredientRepository.SaveChangesAsync();
        return result > 0;
    }
}
