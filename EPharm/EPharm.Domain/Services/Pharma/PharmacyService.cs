using AutoMapper;
using EPharm.Domain.Dtos.EmailDto;
using EPharm.Domain.Dtos.PharmacyDtos;
using EPharm.Domain.Dtos.PharmacyStaffDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using EPharm.Infrastructure.Entities.Identity;
using EPharm.Infrastructure.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces.Base;
using EPharm.Infrastructure.Interfaces.Pharma;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace EPharm.Domain.Services.Pharma;

public class PharmacyService(
    IConfiguration config,
    IPharmacyRepository pharmacyRepository,
    IPharmacyStaffService pharmacyStaffService,
    IUserService userService,
    UserManager<AppIdentityUser> userManager,
    IEmailSender emailSender,
    IEmailService emailService,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IPharmacyService
{
    public async Task<GetPharmacyDto?> GetPharmacyByIdAsync(int pharmaCompanyId)
    {
        var pharmacy = await pharmacyRepository.GetByIdAsync(pharmaCompanyId);
        if (pharmacy is null)
            throw new InvalidOperationException("PHARMACY_NOT_FOUND");
        
        var admin = await userService.GetUserByIdAsync(pharmacy.OwnerId);
        if (admin is null)
            throw new InvalidOperationException("USER_NOT_FOUND");

        var pharmacyDto = mapper.Map<GetPharmacyDto>(pharmacy);
        pharmacyDto.Owner = admin;

        return pharmacyDto;
    }

    public async Task<IEnumerable<GetPharmacyDto>> GetAllPharmacyAsync()
    {
        var pharmaCompanies = await pharmacyRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetPharmacyDto>>(pharmaCompanies);
    }

    public async Task InviteAsync(EmailDto emailDto)
    {
        var user = await userService.CreateUserAsync(emailDto, [IdentityData.PharmacyAdmin, IdentityData.PharmacyStaff]);
        
        var existingPharmacyStaff = await pharmacyStaffService.GetByExternalIdAsync(user.Id);
        if (existingPharmacyStaff is null)
        {
            var pharmacy = await pharmacyRepository.InsertAsync(new Pharmacy { OwnerId = user.Id });
            await pharmacyStaffService.CreateAsync(new CreatePharmacyStaffDto
            {
                ExternalId = user.Id,
                Email = user.Email!,
                PharmacyId = pharmacy.Id
            });
        }

        await SendInvitationEmailAsync(user);
    }

    public async Task<AppIdentityUser> VerifyInvitationAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            throw new InvalidOperationException("INVALID_INVITATION");

        if (user.IsAccountSetup == true)
            throw new InvalidOperationException("INVALID_INVITATION");

        var staff = await pharmacyStaffService.GetByExternalIdAsync(user.Id);
        if (staff is null)
            throw new InvalidOperationException("INVALID_INVITATION");

        return user;
    }
    
    public async Task<GetPharmacyDto> CreateAsync(string userId, CreatePharmacyDto createPharmacyDto)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            throw new Exception("USER_NOT_FOUND");
        
        var pharmacy = await pharmacyRepository.GetByOwnerId(userId);
        if (pharmacy is null)
            throw new Exception("PHARMACY_NOT_FOUND");

        mapper.Map(createPharmacyDto, pharmacy);
        pharmacyRepository.Update(pharmacy);
        await pharmacyRepository.SaveChangesAsync();

        user.IsAccountSetup = true;
        await userManager.UpdateAsync(user);

        return mapper.Map<GetPharmacyDto>(pharmacy);
    }

    public async Task<GetPharmacyDto> Register(CreatePharmaDto createPharmaDto)
    {
        var user = await userService.CreateUserAsync(createPharmaDto.UserRequest, [IdentityData.PharmacyAdmin, IdentityData.PharmacyStaff]);
        
        GetPharmacyDto pharmacyDto = new();

        await unitOfWork.ExecuteTransactionAsync(async () =>
        {
            var pharmacy = await CreatePharmacyAsync(createPharmaDto.PharmacyRequest, user.Id);
            pharmacyDto = mapper.Map<GetPharmacyDto>(pharmacy);
            
            await pharmacyStaffService.CreateAsync(new CreatePharmacyStaffDto
            {
                Email = user.Email!,
                ExternalId = user.Id,
                PharmacyId = pharmacy.Id
            });
        });

        user.EmailConfirmed = true;
        user.IsAccountSetup = true;
        await userManager.UpdateAsync(user);

        return pharmacyDto;
    }

    public async Task<bool> UpdateAsync(int id, CreatePharmacyDto pharmacyDto)
    {
        var pharmaCompany = await pharmacyRepository.GetByIdAsync(id);

        if (pharmaCompany is null)
            return false;

        mapper.Map(pharmacyDto, pharmaCompany);

        pharmacyRepository.Update(pharmaCompany);

        var result = await pharmacyRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int pharmaCompanyId)
    {
        var pharmaCompany = await pharmacyRepository.GetByIdAsync(pharmaCompanyId);

        if (pharmaCompany is null)
            return false;

        pharmacyRepository.Delete(pharmaCompany);

        var result = await pharmacyRepository.SaveChangesAsync();
        return result > 0;
    }

    private async Task<GetPharmacyDto> CreatePharmacyAsync(CreatePharmacyDto pharmacyDto, string ownerId)
    {
        try
        {
            var pharmaCompanyEntity = mapper.Map<Pharmacy>(pharmacyDto);
            pharmaCompanyEntity.OwnerId = ownerId;
            var pharmaCompany = await pharmacyRepository.InsertAsync(pharmaCompanyEntity);

            return mapper.Map<GetPharmacyDto>(pharmaCompany);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create pharmaceutical company. Details: {ex.Message}");
        }
    }

    private async Task SendInvitationEmailAsync(AppIdentityUser user)
    {
        var email = emailService.GetEmail("pharmacy-invitation");
        if (email is null)
            throw new KeyNotFoundException("EMAIL_NOT_FOUND");

        email = email.Replace("{url}", $"{config["AppUrls:PharmaPortalClient"]}/onboarding/{user.Id}");

        await emailSender.SendEmailAsync(new CreateEmailDto
        {
            Email = user.Email!,
            Subject = "Welcome to E-Pharm: Complete Your Pharmacy Registration",
            Message = email
        });
    }
}
