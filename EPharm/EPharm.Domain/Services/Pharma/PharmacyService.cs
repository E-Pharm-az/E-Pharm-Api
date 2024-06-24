using System.Text;
using AutoMapper;
using EPharm.Domain.Dtos.EmailDto;
using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
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


    public async Task InvitePharmaAsync(EmailDto emailDto)
    {
        var user = await userService.CreateUserAsync(emailDto,
            [IdentityData.PharmaCompanyAdmin, IdentityData.PharmaCompanyManager]);
        await pharmacyStaffService.CreatePharmaCompanyManagerAsync(new CreatePharmaCompanyManagerDto
            { Email = user.Email, ExternalId = user.Id });

        await SendEmailInvitationAsync(user);
    }

    public async Task InitializePharmaAsync(string userId, string token, CreatePharmaDto createPharmaDto)
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

            var pharmacy = await CreatePharmaCompanyAsync(createPharmaDto.PharmaCompanyRequest, user.Id);
            pharmacyManager.PharmacyId = pharmacy.Id;
            pharmacyStaffRepository.Update(pharmacyManager);

            await pharmacyStaffService.UpdatePharmaCompanyManagerAsync(pharmacyManager.Id,
                new CreatePharmaCompanyManagerDto { PharmaCompanyId = pharmacy.Id });
            await userManager.UpdateAsync(user);
        });
    }

    public async Task CreatePharmaAsync(CreatePharmaDto createPharmaDto)
    {
        var existingUser = await userManager.FindByEmailAsync(createPharmaDto.UserRequest.Email);
        if (existingUser is not null)
        {
            if (existingUser.EmailConfirmed)
                throw new InvalidOperationException("USER_ALREADY_EXISTS");
        }

        await unitOfWork.ExecuteTransactionAsync(async () =>
        {
            var userEntity = mapper.Map<AppIdentityUser>(createPharmaDto.UserRequest);
            userEntity.UserName = createPharmaDto.UserRequest.Email;
            userEntity.EmailConfirmed = true;

            await EnsureRolesExistAsync([IdentityData.PharmaCompanyAdmin, IdentityData.PharmaCompanyManager]);

            var result = await userManager.CreateAsync(userEntity, createPharmaDto.UserRequest.Password);

            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

            var pharmaCompany = await CreatePharmaCompanyAsync(createPharmaDto.PharmaCompanyRequest, userEntity.Id);

            var pharmaCompanyAdminEntity = mapper.Map<CreatePharmaCompanyManagerDto>(userEntity);
            pharmaCompanyAdminEntity.ExternalId = userEntity.Id;
            pharmaCompanyAdminEntity.PharmaCompanyId = pharmaCompany.Id;

            await pharmacyStaffService.CreatePharmaCompanyManagerAsync(pharmaCompanyAdminEntity);
            await userManager.UpdateAsync(userEntity);
        });
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


    private async Task<GetPharmaCompanyDto> CreatePharmaCompanyAsync(CreatePharmaCompanyDto pharmaCompanyDto,
        string pharmaAdminId)
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

    private async Task SendEmailInvitationAsync(AppIdentityUser user)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var emailTemplate = emailService.GetEmail("pharmacy-invitation");
        if (emailTemplate is null)
            throw new KeyNotFoundException("EMAIL_NOT_FOUND");

        emailTemplate = emailTemplate.Replace("{url}",
            $"{configuration["PharmaPortalClient"]}/join?token={encodedToken}?userId={user.Id}");

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