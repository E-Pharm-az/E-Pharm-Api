using EPharm.Infrastructure.Context.Entities.Identity;

namespace EPharm.Infrastructure.Interfaces.IdentityRepositoriesInterfaces;

public interface IAppIdentityUserRepository
{
    public Task<IEnumerable<AppIdentityUser>> GetAllAsync();
    public Task<AppIdentityUser?> GetByIdAsync(string id);
    public Task<AppIdentityUser?> GetByEmailAsync(string email);
    public Task<AppIdentityUser> InsertAsync(AppIdentityUser entity);
    public void Update(AppIdentityUser entity);
    public void Delete(AppIdentityUser entity);
    public Task<int> SaveChangesAsync();
}
