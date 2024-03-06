using AutoMapper;
using EPharm.Domain.Dtos.ProductDtos.ActiveIngredientDto;
using EPharm.Domain.Interfaces.Product;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Domain.Services.ProductServices;

public class ActiveIngredientService(IActiveIngredientRepository activeIngredientRepository, IMapper mapper)
    : IActiveIngredientService
{
    public async Task<List<GetActiveIngredientDto>> GetAllActiveIngredientsAsync()
    {
        var activeIngredients = await activeIngredientRepository.GetAllAsync();
        return mapper.Map<List<GetActiveIngredientDto>>(activeIngredients);
    }

    public async Task<GetActiveIngredientDto?> GetActiveIngredientByIdAsync(int id)
    {
        var activeIngredient = await activeIngredientRepository.GetByIdAsync(id);
        return mapper.Map<GetActiveIngredientDto>(activeIngredient);
    }

    public async Task<GetActiveIngredientDto> CreateActiveIngredientAsync(
        CreateActiveIngredientDto createActiveIngredientDto)
    {
        var activeIngredientEntity = mapper.Map<ActiveIngredient>(createActiveIngredientDto);
        var activeIngredient = activeIngredientRepository.InsertAsync(activeIngredientEntity);

        var result = await activeIngredientRepository.SaveChangesAsync();

        if (result > 0)
            return mapper.Map<GetActiveIngredientDto>(activeIngredient);

        throw new InvalidOperationException("Failed to create active ingredient.");
    }

    public async Task<bool> UpdateActiveIngredientAsync(int id, CreateActiveIngredientDto createActiveIngredientDto)
    {
        var activeIngredient = await activeIngredientRepository.GetByIdAsync(id);

        if (activeIngredient is null)
            return false;

        var activeIngredientEntity = mapper.Map<ActiveIngredient>(createActiveIngredientDto);
        mapper.Map(activeIngredientEntity, activeIngredient);
        
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
