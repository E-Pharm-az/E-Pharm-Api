using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Identity;
using EPharm.Infrastructure.Interfaces.IdentityRepositoriesInterfaces;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.IdentityRepositories;

public class AppIdentityUserRepository(AppIdentityDbContext context) : IAppIdentityUserRepository
{
    private readonly DbSet<AppIdentityUser> _entities = context.Set<AppIdentityUser>();

    public async Task<IEnumerable<AppIdentityUser>> GetAllAsync() =>
        await _entities.AsNoTracking().ToListAsync();

    public async Task<AppIdentityUser?> GetByIdAsync(string id) =>
        await _entities.AsNoTracking().SingleOrDefaultAsync(s => s.Id == id);

    public async Task<AppIdentityUser?> GetByEmailAsync(string email) =>
        await _entities.AsNoTracking().SingleOrDefaultAsync(s => s.Email == email);

    public async Task<AppIdentityUser> InsertAsync(AppIdentityUser entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _entities.AddAsync(entity);

        return entity;
    }

    public void Update(AppIdentityUser entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _entities.Update(entity);
    }

    public void Delete(AppIdentityUser entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _entities.Remove(entity);
    }

    public async Task<int> SaveChangesAsync() =>
        await context.SaveChangesAsync();
}
