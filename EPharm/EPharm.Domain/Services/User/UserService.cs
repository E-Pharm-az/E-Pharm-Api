using AutoMapper;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Domain.Interfaces.User;
using EPharm.Domain.Models.Identity;
using EPharm.Infrastructure.Context.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace EPharm.Domain.Services.User;

public class UserService(
    UserManager<AppIdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IMapper mapper) : IUserService
{
    
    public async Task<UserRegistrationDto> CreateUser(UserRegistrationDto user)
    {
        var userEntity = mapper.Map<AppIdentityUser>(user);

        var role = await roleManager.FindByNameAsync(IdentityData.Customer);

        if (role is null)
        {
            await roleManager.CreateAsync(new IdentityRole(IdentityData.Customer));
        }

        await userManager.AddToRoleAsync(userEntity, role.Name);
        userEntity.UserName = user.Email;

        var result = await userManager.CreateAsync(userEntity, user.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            var errorMessage = string.Join("; ", errors);

            throw new Exception($"Failed to create user: {errorMessage}");
        }

        return user;
    }
}
