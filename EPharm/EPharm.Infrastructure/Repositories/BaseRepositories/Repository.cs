using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.BaseRepositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    protected readonly DbSet<T> Entities;

    protected Repository(AppDbContext context)
    {
        _context = context;
        Entities = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync() =>
        await Entities.AsNoTracking().ToListAsync();

    public virtual async Task<T?> GetByIdAsync(int id) =>
        await Entities.AsNoTracking().SingleOrDefaultAsync(s => s.Id == id);

    public virtual async Task<T> InsertAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var entityItem = await Entities.AddAsync(entity);
        
        await _context.SaveChangesAsync();

        return (await Entities.AsNoTracking().SingleOrDefaultAsync(s => s.Id == entityItem.Entity.Id))!;
    }

    public virtual void Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        Entities.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        Entities.Remove(entity);
    }

    public virtual async Task<int> SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
