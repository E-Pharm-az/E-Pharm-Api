using System.Text;
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
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace EPharm.Domain.Services.Pharma;

public class PharmacyService(
    IConfiguration configuration,
    IPharmacyRepository pharmacyRepository,
    IPharmacyStaffService pharmacyStaffService,
    IPharmacyStaffRepository pharmacyStaffRepository,
    IUserService userService,
    UserManager<AppIdentityUser> userManager,
    IPasswordHasher<AppIdentityUser> passwordHasher,
    RoleManager<IdentityRole> roleManager,
    IEmailSender emailSender,
    IEmailService emailService,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IPharmacyService
{
    public async Task<IEnumerable<GetPharmacyDto>> GetAllPharmacyAsync()
    {
        var pharmaCompanies = await pharmacyRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetPharmacyDto>>(pharmaCompanies);
    }

    public async Task<GetPharmacyDto?> GetPharmacyByIdAsync(int pharmaCompanyId)
    {
        var pharmaCompany = await pharmacyRepository.GetByIdAsync(pharmaCompanyId);
        return mapper.Map<GetPharmacyDto>(pharmaCompany);
    }


    public async Task InvitePharmacyAsync(EmailDto emailDto)
    {
        var user = await userService.CreateUserAsync(emailDto, [IdentityData.PharmacyAdmin, IdentityData.PharmacyStaff]);
        await pharmacyStaffService.CreatePharmacyStaffAsync(new CreatePharmacyStaffDto { Email = user.Email, ExternalId = user.Id });

        await SendEmailInvitationAsync(user);
    }

    public async Task InitializePharmacyAsync(string userId, string token, CreatePharmaDto createPharmaDto)
    {
        var user = await userManager.FindByEmailAsync(createPharmaDto.UserRequest.Email);
        if (user is null || user.Id != userId)
            throw new InvalidOperationException("USER_NOT_FOUND");

        var pharmacyManager = await pharmacyStaffRepository.GetPharmaCompanyManagerByExternalIdAsync(userId);
        if (pharmacyManager is null)
            throw new InvalidOperationException("USER_NOT_FOUND");

        await unitOfWork.ExecuteTransactionAsync(async () =>
        {
            await userManager.ConfirmEmailAsync(user, token);
            user.PasswordHash = passwordHasher.HashPassword(user, createPharmaDto.UserRequest.Password);

            var pharmacy = await CreatePharmacyAsync(createPharmaDto.PharmacyRequest, user.Id);
            pharmacyManager.PharmacyId = pharmacy.Id;
            pharmacyStaffRepository.Update(pharmacyManager);

            await pharmacyStaffService.UpdatePharmacyStaffAsync(pharmacyManager.Id,
                new CreatePharmacyStaffDto { PharmaCompanyId = pharmacy.Id });
            await userManager.UpdateAsync(user);
        });
    }

    public async Task<GetPharmacyDto> CreatePharmacyAsync(CreatePharmaDto createPharmaDto)
    {
        var existingUser = await userManager.FindByEmailAsync(createPharmaDto.UserRequest.Email);
        if (existingUser is not null)
        {
            if (existingUser.EmailConfirmed)
                throw new InvalidOperationException("USER_ALREADY_EXISTS");
        }

        GetPharmacyDto pharmacyDto = null;

        await unitOfWork.ExecuteTransactionAsync(async () =>
        {
            var userEntity = mapper.Map<AppIdentityUser>(createPharmaDto.UserRequest);
            userEntity.UserName = createPharmaDto.UserRequest.Email;
            userEntity.EmailConfirmed = true;

            await EnsureRolesExistAsync([IdentityData.PharmacyAdmin, IdentityData.PharmacyStaff]);

            var result = await userManager.CreateAsync(userEntity, createPharmaDto.UserRequest.Password);

            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

            var pharmacy = await CreatePharmacyAsync(createPharmaDto.PharmacyRequest, userEntity.Id);

            var pharmaCompanyAdminEntity = mapper.Map<CreatePharmacyStaffDto>(userEntity);
            pharmaCompanyAdminEntity.ExternalId = userEntity.Id;
            pharmaCompanyAdminEntity.PharmaCompanyId = pharmacy.Id;

            await pharmacyStaffService.CreatePharmacyStaffAsync(pharmaCompanyAdminEntity);
            await userManager.UpdateAsync(userEntity);
            
            pharmacyDto = mapper.Map<GetPharmacyDto>(pharmacy);
        });
        
        return pharmacyDto;
    }

    public async Task<bool> UpdatePharmacyAsync(int id, CreatePharmacyDto pharmacyDto)
    {
        var pharmaCompany = await pharmacyRepository.GetByIdAsync(id);

        if (pharmaCompany is null)
            return false;

        mapper.Map(pharmacyDto, pharmaCompany);

        pharmacyRepository.Update(pharmaCompany);

        var result = await pharmacyRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeletePharmacyAsync(int pharmaCompanyId)
    {
        var pharmaCompany = await pharmacyRepository.GetByIdAsync(pharmaCompanyId);

        if (pharmaCompany is null)
            return false;

        pharmacyRepository.Delete(pharmaCompany);

        var result = await pharmacyRepository.SaveChangesAsync();
        return result > 0;
    }


    private async Task<GetPharmacyDto> CreatePharmacyAsync(CreatePharmacyDto pharmacyDto,
        string pharmaAdminId)
    {
        try
        {
            var pharmaCompanyEntity = mapper.Map<Pharmacy>(pharmacyDto);
            pharmaCompanyEntity.OwnerId = pharmaAdminId;
            var pharmaCompany = await pharmacyRepository.InsertAsync(pharmaCompanyEntity);

            return mapper.Map<GetPharmacyDto>(pharmaCompany);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create pharmaceutical company. Details: {ex.Message}");
        }
    }

    private async Task SendEmailInvitationAsync(AppIdentityUser user)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var emailTemplate = emailService.GetEmail("pharmacy-invitation");
        if (emailTemplate is null)
            throw new KeyNotFoundException("EMAIL_NOT_FOUND");

        emailTemplate = emailTemplate.Replace("{url}",
            $"{configuration["AppUrls:PharmaPortalClient"]}/join?token={encodedToken}?userId={user.Id}");

        await emailSender.SendEmailAsync(new CreateEmailDto
        {
            Email = user.Email,
            Subject = "Welcome to E-Pharm: Complete Your Pharmacy Registration",
            Message = emailTemplate
        });
    }

    private async Task EnsureRolesExistAsync(string[] roles)
    {
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}