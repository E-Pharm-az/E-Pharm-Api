using AutoMapper;
using EPharm.Domain.Dtos.DosageFormDto;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;

namespace EPharm.Domain.Services.Entities;

public class DosageFormService(IDosageFormRepository dosageFormRepository, IMapper mapper) : IDosageFormService
{
    public async Task<IEnumerable<GetDosageFormDto>> GetAllDosageFormsAsync()
    {
        var dosageForms = await dosageFormRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetDosageFormDto>>(dosageForms);
    }

    public async Task<GetDosageFormDto?> GetDosageFormByIdAsync(int id)
    {
        var dosageForm = await dosageFormRepository.GetByIdAsync(id);
        return mapper.Map<GetDosageFormDto>(dosageForm);
    }

    public async Task<GetDosageFormDto> CreateDosageFormAsync(CreateDosageFormDto dosageFormDto)
    {
        try
        {
            var dosageFormEntity = mapper.Map<DosageForm>(dosageFormDto);
            var dosageForm = await dosageFormRepository.InsertAsync(dosageFormEntity);

            return mapper.Map<GetDosageFormDto>(dosageForm);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create dosage form. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdateDosageFormAsync(int id, CreateDosageFormDto dosageFormDto)
    {
        var dosageForm = await dosageFormRepository.GetByIdAsync(id);

        if (dosageForm is null)
            return false;

        mapper.Map(dosageFormDto, dosageForm);
        dosageFormRepository.Update(dosageForm);

        var result = await dosageFormRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteDosageFormAsync(int id)
    {
        var dosageForm = await dosageFormRepository.GetByIdAsync(id);

        if (dosageForm is null)
            return false;

        dosageFormRepository.Delete(dosageForm);
        var result = await dosageFormRepository.SaveChangesAsync();
        return result > 0;
    }
}