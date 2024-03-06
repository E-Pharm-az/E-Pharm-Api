using AutoMapper;
using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Domain.Interfaces.Pharma;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces.PharmaRepositoriesInterfaces;

namespace EPharm.Domain.Services.PharmaServices;

public class PharmaCompanyManagerService(IPharmaCompanyManagerRepository pharmaCompanyManagerRepository, IMapper mapper)
    : IPharmaCompanyManagerService
{
    public async Task<IEnumerable<GetPharmaCompanyManagerDto>> GetAllPharmaCompanyManagersAsync()
    {
        var pharmaCompanyManagers = await pharmaCompanyManagerRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetPharmaCompanyManagerDto>>(pharmaCompanyManagers);
    }

    public async Task<GetPharmaCompanyManagerDto?> GetPharmaCompanyManagerByIdAsync(int pharmaCompanyManagerId)
    {
        var pharmaCompanyManager = await pharmaCompanyManagerRepository.GetByIdAsync(pharmaCompanyManagerId);
        return mapper.Map<GetPharmaCompanyManagerDto>(pharmaCompanyManager);
    }

    public async Task<GetPharmaCompanyManagerDto> CreatePharmaCompanyManagerAsync(
        CreatePharmaCompanyManagerDto pharmaCompanyManagerDto)
    {
        var pharmaCompanyManagerEntity = mapper.Map<PharmaCompanyManager>(pharmaCompanyManagerDto);
        var pharmaCompanyManger = pharmaCompanyManagerRepository.InsertAsync(pharmaCompanyManagerEntity);

        var result = await pharmaCompanyManagerRepository.SaveChangesAsync();

        if (result > 0)
            return mapper.Map<GetPharmaCompanyManagerDto>(pharmaCompanyManger);

        throw new InvalidOperationException("Failed to create pharmaceutical company manager.");
    }

    public async Task<bool> UpdatePharmaCompanyManagerAsync(int id, CreatePharmaCompanyManagerDto pharmaCompanyManagerDto)
    {
        var pharmaCompanyManager = await pharmaCompanyManagerRepository.GetByIdAsync(id);
        
        if (pharmaCompanyManager is null)
            return false;
        
        var pharmaCompanyManagerEntity = mapper.Map<PharmaCompanyManager>(pharmaCompanyManagerDto);
        mapper.Map(pharmaCompanyManagerEntity, pharmaCompanyManager);
        
        pharmaCompanyManagerRepository.Update(pharmaCompanyManager);

        var result = await pharmaCompanyManagerRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeletePharmaCompanyManagerAsync(int pharmaCompanyManagerId)
    {
        var pharmaCompanyManager = await pharmaCompanyManagerRepository.GetByIdAsync(pharmaCompanyManagerId);

        if (pharmaCompanyManager is null)
            return false;

        pharmaCompanyManagerRepository.Delete(pharmaCompanyManager);

        var result = await pharmaCompanyManagerRepository.SaveChangesAsync();
        return result > 0;
    }
}
