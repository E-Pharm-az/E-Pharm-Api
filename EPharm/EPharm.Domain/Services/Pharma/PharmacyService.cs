using AutoMapper;
using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces.PharmaRepositoriesInterfaces;

namespace EPharm.Domain.Services.PharmaServices;

public class PharmacyService(IPharmacyRepository pharmacyRepository, IMapper mapper)
    : IPharmaCompanyService
{
    public async Task<IEnumerable<GetPharmaCompanyDto>> GetAllPharmaCompaniesAsync()
    {
        var pharmaCompanies = await pharmacyRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetPharmaCompanyDto>>(pharmaCompanies);
    }

    public async Task<GetPharmaCompanyDto?> GetPharmaCompanyByIdAsync(int pharmaCompanyId)
    {
        var pharmaCompany = await pharmacyRepository.GetByIdAsync(pharmaCompanyId);
        return mapper.Map<GetPharmaCompanyDto>(pharmaCompany);
    }

    public async Task<GetPharmaCompanyDto> CreatePharmaCompanyAsync(CreatePharmaCompanyDto pharmaCompanyDto, string pharmaAdminId)
    {
        try
        {
            var pharmaCompanyEntity = mapper.Map<Pharmacy>(pharmaCompanyDto);
            pharmaCompanyEntity.OwnerId = pharmaAdminId;
            var pharmaCompany = await pharmacyRepository.InsertAsync(pharmaCompanyEntity);

            return mapper.Map<GetPharmaCompanyDto>(pharmaCompany);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create pharmaceutical company. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdatePharmaCompanyAsync(int id, CreatePharmaCompanyDto pharmaCompanyDto)
    {
        var pharmaCompany = await pharmacyRepository.GetByIdAsync(id);

        if (pharmaCompany is null)
            return false;

        mapper.Map(pharmaCompanyDto, pharmaCompany);

        pharmacyRepository.Update(pharmaCompany);

        var result = await pharmacyRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeletePharmaCompanyAsync(int pharmaCompanyId)
    {
        var pharmaCompany = await pharmacyRepository.GetByIdAsync(pharmaCompanyId);

        if (pharmaCompany is null)
            return false;

        pharmacyRepository.Delete(pharmaCompany);

        var result = await pharmacyRepository.SaveChangesAsync();
        return result > 0;
    }
}
