using AutoMapper;
using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces.PharmaRepositoriesInterfaces;

namespace EPharm.Domain.Services.Pharma;

public class PharmacyStaffService(IPharmacyStaffRepository pharmacyStaffRepository, IMapper mapper) : IPharmaCompanyManagerService
{
    public async Task<IEnumerable<GetPharmaCompanyManagerDto>> GetAllPharmaCompanyManagersAsync(int companyId)
    {
        var pharmaCompanyManagers = await pharmacyStaffRepository.GetAllPharmaCompanyManagersAsync(companyId);
        return mapper.Map<IEnumerable<GetPharmaCompanyManagerDto>>(pharmaCompanyManagers);
    }
    
    public async Task<GetPharmaCompanyManagerDto?> GetPharmaCompanyManagerByExternalIdAsync(string externalId)
    {
        var pharmaCompanyManager = await pharmacyStaffRepository.GetPharmaCompanyManagerByExternalIdAsync(externalId);
        return mapper.Map<GetPharmaCompanyManagerDto>(pharmaCompanyManager);
    }
 
    public async Task<GetPharmaCompanyManagerDto?> GetPharmaCompanyManagerByIdAsync(int pharmaCompanyManagerId)
    {
        var pharmaCompanyManager = await pharmacyStaffRepository.GetByIdAsync(pharmaCompanyManagerId);
        return mapper.Map<GetPharmaCompanyManagerDto>(pharmaCompanyManager);
    }

    public async Task<GetPharmaCompanyManagerDto> CreatePharmaCompanyManagerAsync(CreatePharmaCompanyManagerDto pharmaCompanyManagerDto)
    {
        var pharmaCompanyManagerEntity = mapper.Map<PharmacyStaff>(pharmaCompanyManagerDto);
        var pharmaCompanyManger = await pharmacyStaffRepository.InsertAsync(pharmaCompanyManagerEntity);

        return mapper.Map<GetPharmaCompanyManagerDto>(pharmaCompanyManger);
    }

    public async Task<bool> UpdatePharmaCompanyManagerAsync(int id, CreatePharmaCompanyManagerDto pharmaCompanyManagerDto)
    {
        var pharmaCompanyManager = await pharmacyStaffRepository.GetByIdAsync(id);

        if (pharmaCompanyManager is null)
            return false;

        mapper.Map(pharmaCompanyManagerDto, pharmaCompanyManager);

        pharmacyStaffRepository.Update(pharmaCompanyManager);

        var result = await pharmacyStaffRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeletePharmaCompanyManagerAsync(int pharmaCompanyManagerId)
    {
        var pharmaCompanyManager = await pharmacyStaffRepository.GetByIdAsync(pharmaCompanyManagerId);

        if (pharmaCompanyManager is null)
            return false;

        pharmacyStaffRepository.Delete(pharmaCompanyManager);

        var result = await pharmacyStaffRepository.SaveChangesAsync();
        return result > 0;
    }
}
