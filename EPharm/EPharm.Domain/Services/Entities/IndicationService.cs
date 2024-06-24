using AutoMapper;
using EPharm.Domain.Dtos.IndicationDto;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;

namespace EPharm.Domain.Services.Entities;

public class IndicationService(IIndicationRepository indicationRepository, IMapper mapper) : IIndicationService
{
    public async Task<List<GetIndicationDto>> GetAllIndicationsAsync()
    {
        var indications = await indicationRepository.GetAllAsync();
        return mapper.Map<List<GetIndicationDto>>(indications);
    }

    public async Task<GetIndicationDto?> GetIndicationByIdAsync(int id)
    {
        var indication = await indicationRepository.GetByIdAsync(id);
        return mapper.Map<GetIndicationDto>(indication);
    }

    public async Task<GetIndicationDto> CreateIndicationAsync(CreateIndicationDto newIndication)
    {
        try
        {
            var indicationEntity = mapper.Map<Indication>(newIndication);
            var indication = await indicationRepository.InsertAsync(indicationEntity);

            return mapper.Map<GetIndicationDto>(indication);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create indication. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdateIndicationAsync(int id, CreateIndicationDto updatedIndication)
    {
        var indication = await indicationRepository.GetByIdAsync(id);

        if (indication is null)
            return false;

        mapper.Map(updatedIndication, indication);
        indicationRepository.Update(indication);

        var result = await indicationRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteIndicationAsync(int id)
    {
        var indication = await indicationRepository.GetByIdAsync(id);

        if (indication is null)
            return false;

        indicationRepository.Delete(indication);
        var result = await indicationRepository.SaveChangesAsync();
        return result > 0;
    }
}