using EPharm.Domain.Dtos.IndicationDto;

namespace EPharm.Domain.Interfaces.Product;

public interface IIndicationService
{
    Task<List<GetIndicationDto>> GetAllIndicationsAsync();
    Task<GetIndicationDto?> GetIndicationByIdAsync(int id);
    Task<GetIndicationDto> CreateIndicationAsync(CreateIndicationDto newIndication);
    Task<bool> UpdateIndicationAsync(int id, CreateIndicationDto updatedIndication);
    Task<bool> DeleteIndicationAsync(int id);
}
