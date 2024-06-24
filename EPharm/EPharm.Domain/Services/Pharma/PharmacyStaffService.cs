using AutoMapper;
using EPharm.Domain.Dtos.PharmacyStaffDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using EPharm.Infrastructure.Entities.Identity;
using EPharm.Infrastructure.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces.Pharma;
using Microsoft.AspNetCore.Identity;

namespace EPharm.Domain.Services.Pharma;

public class PharmacyStaffService(
    IPharmacyStaffRepository pharmacyStaffRepository,
    IUserService userService,
    UserManager<AppIdentityUser> userManager,
    IMapper mapper) : IPharmacyStaffService
{
    public async Task<IEnumerable<GetPharmacyStaffDto>> GetAllPharmacyStaffAsync(int companyId)
    {
        var pharmaCompanyManagers = await pharmacyStaffRepository.GetAllPharmaCompanyManagersAsync(companyId);
        return mapper.Map<IEnumerable<GetPharmacyStaffDto>>(pharmaCompanyManagers);
    }

    public async Task<GetPharmacyStaffDto?> GetPharmacyStaffByExternalIdAsync(string externalId)
    {
        var pharmaCompanyManager = await pharmacyStaffRepository.GetPharmaCompanyManagerByExternalIdAsync(externalId);
        return mapper.Map<GetPharmacyStaffDto>(pharmaCompanyManager);
    }

    public async Task<GetPharmacyStaffDto?> GetPharmacyStaffByIdAsync(int pharmaCompanyManagerId)
    {
        var pharmaCompanyManager = await pharmacyStaffRepository.GetByIdAsync(pharmaCompanyManagerId);
        return mapper.Map<GetPharmacyStaffDto>(pharmaCompanyManager);
    }

    public async Task<GetPharmacyStaffDto> CreatePharmacyStaffAsync(
        CreatePharmacyStaffDto pharmacyStaffDto)
    {
        var pharmaCompanyManagerEntity = mapper.Map<PharmacyStaff>(pharmacyStaffDto);
        var pharmaCompanyManger = await pharmacyStaffRepository.InsertAsync(pharmaCompanyManagerEntity);

        return mapper.Map<GetPharmacyStaffDto>(pharmaCompanyManger);
    }

    public async Task<GetPharmacyStaffDto> CreatePharmacyStaffAsync(int pharmaCompanyId, EmailDto emailDto)
    {
        var user = await userService.CreateUserAsync(emailDto, [IdentityData.PharmacyStaff]);

        var pharmaCompanyManagerEntity = mapper.Map<CreatePharmacyStaffDto>(user);
        pharmaCompanyManagerEntity.ExternalId = user.Id;
        pharmaCompanyManagerEntity.PharmaCompanyId = pharmaCompanyId;

        return await CreatePharmacyStaffAsync(pharmaCompanyManagerEntity);
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

    public async Task<bool> UpdatePharmacyStaffAsync(int id,
        CreatePharmacyStaffDto pharmacyStaffDto)
    {
        var pharmaCompanyManager = await pharmacyStaffRepository.GetByIdAsync(id);

        if (pharmaCompanyManager is null)
            return false;

        mapper.Map(pharmacyStaffDto, pharmaCompanyManager);

        pharmacyStaffRepository.Update(pharmaCompanyManager);

        var result = await pharmacyStaffRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeletePharmacyStaffAsync(int pharmaCompanyManagerId)
    {
        var pharmaCompanyManager = await pharmacyStaffRepository.GetByIdAsync(pharmaCompanyManagerId);

        if (pharmaCompanyManager is null)
            return false;

        pharmacyStaffRepository.Delete(pharmaCompanyManager);

        var result = await pharmacyStaffRepository.SaveChangesAsync();
        return result > 0;
    }
}