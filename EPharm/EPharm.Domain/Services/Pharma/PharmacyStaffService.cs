using AutoMapper;
using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using EPharm.Infrastructure.Context.Entities.Identity;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces.Pharma;
using Microsoft.AspNetCore.Identity;

namespace EPharm.Domain.Services.Pharma;

public class PharmacyStaffService(
    IPharmacyStaffRepository pharmacyStaffRepository,
    IUserService userService,
    UserManager<AppIdentityUser> userManager,
    IMapper mapper) : IPharmacyStaffService
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
    
    public async Task<GetPharmaCompanyManagerDto> CreatePharmaManagerAsync(int pharmaCompanyId, EmailDto emailDto)
    {
        var user = await userService.CreateUserAsync(emailDto, [IdentityData.PharmaCompanyManager]);

        var pharmaCompanyManagerEntity = mapper.Map<CreatePharmaCompanyManagerDto>(user);
        pharmaCompanyManagerEntity.ExternalId = user.Id;
        pharmaCompanyManagerEntity.PharmaCompanyId = pharmaCompanyId;

        return await CreatePharmaCompanyManagerAsync(pharmaCompanyManagerEntity);
    }

    public async Task<bool> UpdateUserAsync(string id, EmailDto emailDto)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user is null)
            return false;

        mapper.Map(emailDto, user);

        var result = await userManager.UpdateAsync(user);

        return result.Succeeded;
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
