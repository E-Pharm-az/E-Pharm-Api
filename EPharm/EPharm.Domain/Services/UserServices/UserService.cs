using AutoMapper;
using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.Pharma;
using EPharm.Domain.Interfaces.User;
using EPharm.Domain.Models.Identity;
using EPharm.Infrastructure.Context.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Domain.Services.User;

public class UserService(
    UserManager<AppIdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IPharmaCompanyManagerService pharmaCompanyManagerService,
    IPharmaCompanyService pharmaCompanyService,
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
            var user = await CreateUserAsync(createUserDto, IdentityData.Customer);
            return user;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<GetPharmaCompanyManagerDto> CreatePharmaManagerAsync(int pharmaCompanyId, CreateUserDto createUserDto)
    {
        try
        {
            var user = await CreateUserAsync(createUserDto, IdentityData.PharmaCompanyManager);

            var pharmaCompanyManagerEntity = mapper.Map<CreatePharmaCompanyManagerDto>(user);
            pharmaCompanyManagerEntity.ExternalId = user.Id;
            pharmaCompanyManagerEntity.PharmaCompanyId = pharmaCompanyId;

            var result = await pharmaCompanyManagerService.CreatePharmaCompanyManagerAsync(pharmaCompanyManagerEntity);
            return result;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    // Creates a pharma company with a pharma company admin
    public async Task<GetPharmaCompanyManagerDto> CreatePharmaAdminAsync(CreateUserDto createUserDto, CreatePharmaCompanyDto createPharmaCompanyDto)
    {
        try
        {
            var user = await CreateUserAsync(createUserDto, IdentityData.PharmaCompanyAdmin);
            
            var pharmaCompanyAdminEntity = mapper.Map<CreatePharmaCompanyManagerDto>(user);
            await pharmaCompanyService.CreatePharmaCompanyAsync(createPharmaCompanyDto, user.Id);

            var pharmaCompanyAdmin = await pharmaCompanyManagerService.CreatePharmaCompanyManagerAsync(pharmaCompanyAdminEntity);
            pharmaCompanyAdminEntity.ExternalId = user.Id;
            pharmaCompanyAdminEntity.PharmaCompanyId = pharmaCompanyAdmin.Id;
            
            return pharmaCompanyAdmin;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<GetUserDto> CreateAdminAsync(CreateUserDto createUserDto)
    {
        try
        {
            var user = await CreateUserAsync(createUserDto, IdentityData.Admin);
            return user;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> UpdateUserAsync(GetUserDto getUserDto)
    {
        var userEntity = mapper.Map<AppIdentityUser>(getUserDto);
        var result = await userManager.UpdateAsync(userEntity);

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

    private async Task<GetUserDto> CreateUserAsync(CreateUserDto createUserDto, string identityRole)
    {
        try
        {
            var userEntity = mapper.Map<AppIdentityUser>(createUserDto);

            var role = await roleManager.FindByNameAsync(identityRole);

            if (role is null)
                await roleManager.CreateAsync(new IdentityRole(identityRole));

            await userManager.AddToRoleAsync(userEntity, role.Name);
            userEntity.UserName = createUserDto.Email;

            var createUserResult = await userManager.CreateAsync(userEntity, createUserDto.Password);

            if (!createUserResult.Succeeded)
            {
                var errors = createUserResult.Errors.Select(e => e.Description);
                var errorMessage = string.Join("; ", errors);

                throw new Exception($"Failed to create user: {errorMessage}");
            }

            return mapper.Map<GetUserDto>(createUserDto);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
