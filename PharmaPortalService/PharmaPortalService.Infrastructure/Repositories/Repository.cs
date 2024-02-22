using Microsoft.EntityFrameworkCore;
using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.Base;
using PharmaPortalService.Infrastructure.Interfaces;

namespace PharmaPortalService.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _entities;

    protected Repository(AppDbContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync() => 
        await _entities.AsNoTracking().ToListAsync();

    public virtual async Task<T?> GetByIdAsync(int? id) =>
        await _entities.AsNoTracking().SingleOrDefaultAsync(s => s.Id == id);

    public virtual async Task InsertAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _entities.AddAsync(entity);
    }

    public virtual void UpdateAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

         _entities.Update(entity);
    }

    public virtual void DeleteAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _entities.Remove(entity);
    }

    public virtual async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
} 
