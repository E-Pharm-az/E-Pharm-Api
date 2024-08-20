using EPharm.Domain.Dtos.AttributeDtos;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface IIndicationService
{
    Task<List<GetAttributeDto>> GetAllIndicationsAsync();
    Task<GetAttributeDto?> GetIndicationByIdAsync(int id);
    Task<GetAttributeDto> CreateIndicationAsync(CreateAttributeDto newIndication);
    Task<bool> UpdateIndicationAsync(int id, CreateAttributeDto updatedIndication);
    Task<bool> DeleteIndicationAsync(int id);
}
