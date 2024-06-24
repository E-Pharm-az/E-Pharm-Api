using AutoMapper;
using EPharm.Domain.Dtos.RegulatoryInformationDto;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;

namespace EPharm.Domain.Services.Entities;

public class RegulatoryInformationService(
    IRegulatoryInformationRepository regulatoryInformationRepository,
    IMapper mapper) : IRegulatoryInformationService
{
    public async Task<IEnumerable<GetRegulatoryInformationDto>> GetAllCompanyRegulatoryInformationAsync(
        int pharmaCompanyId)
    {
        var regulatoryInformation =
            await regulatoryInformationRepository.GetAllCompanyRegulatoryInformationAsync(pharmaCompanyId);
        return mapper.Map<IEnumerable<GetRegulatoryInformationDto>>(regulatoryInformation);
    }

    public async Task<GetRegulatoryInformationDto?> GetRegulatoryInformationByIdAsync(int regulatoryInformationId)
    {
        var regulatoryInformation = await regulatoryInformationRepository.GetByIdAsync(regulatoryInformationId);
        return mapper.Map<GetRegulatoryInformationDto>(regulatoryInformation);
    }

    public async Task<GetRegulatoryInformationDto> AddCompanyRegulatoryInformationAsync(int pharmaCompanyId,
        CreateRegulatoryInformationDto regulatoryInformationDto)
    {
        try
        {
            var regulatoryInformationEntity = mapper.Map<RegulatoryInformation>(regulatoryInformationDto);
            regulatoryInformationEntity.PharmacyId = pharmaCompanyId;
            var regulatoryInformation = await regulatoryInformationRepository.InsertAsync(regulatoryInformationEntity);

            return mapper.Map<GetRegulatoryInformationDto>(regulatoryInformation);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create regulatory information. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdateCompanyRegulatoryInformationAsync(int regulatoryInformationId,
        CreateRegulatoryInformationDto regulatoryInformationDto)
    {
        var regulatoryInformationEntity = await regulatoryInformationRepository.GetByIdAsync(regulatoryInformationId);

        if (regulatoryInformationEntity is null)
            return false;

        mapper.Map(regulatoryInformationDto, regulatoryInformationEntity);
        regulatoryInformationRepository.Update(regulatoryInformationEntity);

        var result = await regulatoryInformationRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteCompanyRegulatoryInformationAsync(int regulatoryInformationId)
    {
        var regulatoryInformation = await regulatoryInformationRepository.GetByIdAsync(regulatoryInformationId);

        if (regulatoryInformation is null)
            return false;

        regulatoryInformationRepository.Delete(regulatoryInformation);

        var result = await regulatoryInformationRepository.SaveChangesAsync();
        return result > 0;
    }
}