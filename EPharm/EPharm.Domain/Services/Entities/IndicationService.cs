using AutoMapper;
using EPharm.Domain.Dtos.AttributeDtos;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;

namespace EPharm.Domain.Services.Entities;

public class IndicationService(IIndicationRepository indicationRepository, IMapper mapper) : IIndicationService
{
    public async Task<List<GetAttributeDto>> GetAllIndicationsAsync()
    {
        var indications = await indicationRepository.GetAllAsync();
        return mapper.Map<List<GetAttributeDto>>(indications);
    }

    public async Task<GetAttributeDto?> GetIndicationByIdAsync(int id)
    {
        var indication = await indicationRepository.GetByIdAsync(id);
        return mapper.Map<GetAttributeDto>(indication);
    }

    public async Task<GetAttributeDto> CreateIndicationAsync(CreateAttributeDto newIndication)
    {
        try
        {
            var indicationEntity = mapper.Map<Indication>(newIndication);
            var indication = await indicationRepository.InsertAsync(indicationEntity);

            return mapper.Map<GetAttributeDto>(indication);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create indication. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdateIndicationAsync(int id, CreateAttributeDto updatedIndication)
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
