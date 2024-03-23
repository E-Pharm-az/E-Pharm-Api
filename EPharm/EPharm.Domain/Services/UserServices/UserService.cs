using AutoMapper;
using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.Pharma;
using EPharm.Domain.Interfaces.User;
using EPharm.Domain.Models.Identity;
using EPharm.Infrastructure.Context.Entities.Identity;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Domain.Services.UserServices;

public class UserService(
    UserManager<AppIdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IPharmaCompanyManagerService pharmaCompanyManagerService,
    IPharmaCompanyService pharmaCompanyService,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IUserService
{
    public async Task<IEnumerable<GetUserDto>> GetAllUsersAsync()
    {
        var users = await userManager.Users.ToListAsync();
        return mapper.Map<IEnumerable<GetUserDto>>(users);
    }

    public async Task<GetUserDto?> GetUserByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        return mapper.Map<GetUserDto>(user);
    }

    public async Task<GetUserDto> CreateCustomerAsync(CreateUserDto createUserDto)
    {
        try
        {
            var existingUser = await userManager.FindByEmailAsync(createUserDto.Email);
            if (existingUser is not null)
                throw new InvalidOperationException("User with this email already exists.");
            
            var user = await CreateUserAsync(createUserDto, [IdentityData.Customer]);
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<GetPharmaCompanyManagerDto> CreatePharmaManagerAsync(int pharmaCompanyId, CreateUserDto createUserDto)
    {
        try
        {
            var user = await CreateUserAsync(createUserDto, [IdentityData.PharmaCompanyManager]);

            var pharmaCompanyManagerEntity = mapper.Map<CreatePharmaCompanyManagerDto>(user);
            pharmaCompanyManagerEntity.ExternalId = user.Id;
            pharmaCompanyManagerEntity.PharmaCompanyId = pharmaCompanyId;

            var result = await pharmaCompanyManagerService.CreatePharmaCompanyManagerAsync(pharmaCompanyManagerEntity);
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<GetPharmaCompanyManagerDto> CreatePharmaAdminAsync(CreateUserDto createUserDto, CreatePharmaCompanyDto createPharmaCompanyDto)
    {
        try
        {
            var user = await CreateUserAsync(createUserDto, [IdentityData.PharmaCompanyAdmin, IdentityData.PharmaCompanyManager]);

            await unitOfWork.BeginTransactionAsync();
            var pharmaCompany = await pharmaCompanyService.CreatePharmaCompanyAsync(createPharmaCompanyDto, user.Id);
            
            var pharmaCompanyAdminEntity = mapper.Map<CreatePharmaCompanyManagerDto>(user);
            pharmaCompanyAdminEntity.ExternalId = user.Id;
            pharmaCompanyAdminEntity.PharmaCompanyId = pharmaCompany.Id;

            var result = await pharmaCompanyManagerService.CreatePharmaCompanyManagerAsync(pharmaCompanyAdminEntity);
            await unitOfWork.CommitTransactionAsync();
            
            return result;
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw new Exception(ex.Message);
        }
    }

    public async Task<GetUserDto> CreateAdminAsync(CreateUserDto createUserDto)
    {
        try
        {
            var user = await CreateUserAsync(createUserDto, [IdentityData.Admin]);
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> UpdateUserAsync(string id, CreateUserDto createUserDto)
    {
        var user = await userManager.FindByIdAsync(id);
        
        if (user is null)
            return false;
        
        mapper.Map(createUserDto, user);
        
        var result = await userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return false;

        var result = await userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    private async Task<GetUserDto> CreateUserAsync(CreateUserDto createUserDto, string[] identityRole)
    {
        var userEntity = mapper.Map<AppIdentityUser>(createUserDto);
        userEntity.UserName = createUserDto.Email;
        var result = await userManager.CreateAsync(userEntity, createUserDto.Password);

        foreach (var role in identityRole)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        
        if (result.Succeeded)
        {
            foreach (var role in identityRole)
            {
                await userManager.AddToRoleAsync(userEntity, role);
            }
            
            return mapper.Map<GetUserDto>(userEntity);
        }

        throw new InvalidOperationException($"Failed to create user. Details: {string.Join("; ", result.Errors.Select(e => e.Description))}");
    }
}
