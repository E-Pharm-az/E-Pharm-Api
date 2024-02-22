using PharmaPortalService.Infrastructure.Context.Entities.Base;

namespace PharmaPortalService.Infrastructure.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T?> GetByIdAsync(int? id);
    public Task InsertAsync(T entity);
    void UpdateAsync(T entity);
    void DeleteAsync(T entity);
    public Task SaveChangesAsync();
}
