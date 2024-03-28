using AutoMapper;
using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Domain.Interfaces.Pharma;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces.PharmaRepositoriesInterfaces;

namespace EPharm.Domain.Services.PharmaServices;

public class PharmaCompanyService(IPharmaCompanyRepository pharmaCompanyRepository, IMapper mapper)
    : IPharmaCompanyService
{
    public async Task<IEnumerable<GetPharmaCompanyDto>> GetAllPharmaCompaniesAsync()
    {
        var pharmaCompanies = await pharmaCompanyRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetPharmaCompanyDto>>(pharmaCompanies);
    }

    public async Task<GetPharmaCompanyDto?> GetPharmaCompanyByIdAsync(int pharmaCompanyId)
    {
        var pharmaCompany = await pharmaCompanyRepository.GetByIdAsync(pharmaCompanyId);
        return mapper.Map<GetPharmaCompanyDto>(pharmaCompany);
    }

    public async Task<GetPharmaCompanyDto> CreatePharmaCompanyAsync(CreatePharmaCompanyDto pharmaCompanyDto,
        string pharmaAdminId)
    {
        try
        {
            var pharmaCompanyEntity = mapper.Map<PharmaCompany>(pharmaCompanyDto);
            pharmaCompanyEntity.OwnerId = pharmaAdminId;
            var pharmaCompany = await pharmaCompanyRepository.InsertAsync(pharmaCompanyEntity);

            return mapper.Map<GetPharmaCompanyDto>(pharmaCompany);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create pharmaceutical company. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdatePharmaCompanyAsync(int id, CreatePharmaCompanyDto pharmaCompanyDto)
    {
        var pharmaCompany = await pharmaCompanyRepository.GetByIdAsync(id);

        if (pharmaCompany is null)
            return false;

        mapper.Map(pharmaCompanyDto, pharmaCompany);

        pharmaCompanyRepository.Update(pharmaCompany);

        var result = await pharmaCompanyRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeletePharmaCompanyAsync(int pharmaCompanyId)
    {
        var pharmaCompany = await pharmaCompanyRepository.GetByIdAsync(pharmaCompanyId);

        if (pharmaCompany is null)
            return false;

        pharmaCompanyRepository.Delete(pharmaCompany);

        var result = await pharmaCompanyRepository.SaveChangesAsync();
        return result > 0;
    }
}
