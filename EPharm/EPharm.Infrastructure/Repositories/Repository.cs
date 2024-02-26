using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories;

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

    public virtual async Task<T> InsertAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _entities.AddAsync(entity);
        
        return entity;
    }

    public virtual void Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

         _entities.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _entities.Remove(entity);
    }

    public virtual async Task<int> SaveChangesAsync() =>
        await _context.SaveChangesAsync();
} 
