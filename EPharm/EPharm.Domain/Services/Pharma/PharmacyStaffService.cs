using System.Text;
using AutoMapper;
using EPharm.Domain.Dtos.EmailDto;
using EPharm.Domain.Dtos.PharmacyStaffDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using EPharm.Infrastructure.Entities.Identity;
using EPharm.Infrastructure.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces.Pharma;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace EPharm.Domain.Services.Pharma;

public class PharmacyStaffService(
    IConfiguration configuration,
    IPharmacyStaffRepository pharmacyStaffRepository,
    IUserService userService,
    UserManager<AppIdentityUser> userManager,
    IEmailService emailService,
    IEmailSender emailSender,
    IMapper mapper) : IPharmacyStaffService
{
    public async Task<IEnumerable<GetPharmacyStaffDto>> GetAllAsync(int pharmacyId)
    {
        var pharmaCompanyManagers = await pharmacyStaffRepository.GetAllAsync(pharmacyId);
        return mapper.Map<IEnumerable<GetPharmacyStaffDto>>(pharmaCompanyManagers);
    }

    public async Task<GetPharmacyStaffDto?> GetByExternalIdAsync(string externalId)
    {
        var pharmaCompanyManager = await pharmacyStaffRepository.GetByExternalIdAsync(externalId);
        return mapper.Map<GetPharmacyStaffDto>(pharmaCompanyManager);
    }

    public async Task<GetPharmacyStaffDto?> GetByIdAsync(int pharmaCompanyManagerId)
    {
        var pharmaCompanyManager = await pharmacyStaffRepository.GetByIdAsync(pharmaCompanyManagerId);
        return mapper.Map<GetPharmacyStaffDto>(pharmaCompanyManager);
    }

    public async Task<GetPharmacyStaffDto> CreateAsync(CreatePharmacyStaffDto pharmacyStaffDto)
    {
        var pharmaCompanyManagerEntity = mapper.Map<PharmacyStaff>(pharmacyStaffDto);
        var pharmaCompanyManger = await pharmacyStaffRepository.InsertAsync(pharmaCompanyManagerEntity);

        return mapper.Map<GetPharmacyStaffDto>(pharmaCompanyManger);
    }

    public async Task<AppIdentityUser> CreateAsync(int pharmacyId, EmailDto emailDto)
    {
        var user = await userService.CreateUserAsync(emailDto, [IdentityData.PharmacyStaff]);
        await CreateAsync(new CreatePharmacyStaffDto{ExternalId = user.Id, Email = user.Email!, PharmacyId = pharmacyId});
        
        return user;
    }

    public async Task<bool> UpdateAsync(int id,
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

    public async Task<bool> DeleteAsync(int pharmaCompanyManagerId)
    {
        var pharmaCompanyManager = await pharmacyStaffRepository.GetByIdAsync(pharmaCompanyManagerId);

        if (pharmaCompanyManager is null)
            return false;

        pharmacyStaffRepository.Delete(pharmaCompanyManager);

        var result = await pharmacyStaffRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task BulkInviteAsync(int pharmaCompanyId, BulkEmailDto bulkEmailDto)
    {
        foreach (var email in bulkEmailDto.Emails)
        {
            var user = await CreateAsync(pharmaCompanyId, email);
            await InviteAsync(user);
        }
    }

    private async Task InviteAsync(AppIdentityUser user)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var emailTemplate = emailService.GetEmail("pharmacy-staff-invitation");
        if (emailTemplate is null)
            throw new Exception("EMAIL_NOT_FOUND");

        emailTemplate = emailTemplate.Replace("{url}", $"{configuration["AppUrls:PharmaPortalClient"]}/onboarding?token={encodedToken}&userId={user.Id}");

        await emailSender.SendEmailAsync(new CreateEmailDto
        {
            Email = user.Email,
            Subject = "Welcome to E-Pharm: Complete Your Pharmacy Registration",
            Message = emailTemplate
        });
    }
}
