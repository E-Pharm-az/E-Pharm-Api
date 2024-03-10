using EPharm.Domain.Dtos.ProductDtos.IndicationDto;

namespace EPharm.Domain.Interfaces.Product;

public interface IIndicationService
{
    Task<List<GetIndicationDto>> GetAllIndications();
    Task<GetIndicationDto?> GetIndicationById(int id);
    Task<GetIndicationDto> CreateIndication(CreateIndicationDto newIndication);
    Task<bool> UpdateIndication(int id, CreateIndicationDto updatedIndication);
    Task<bool> DeleteIndication(int id);
}
