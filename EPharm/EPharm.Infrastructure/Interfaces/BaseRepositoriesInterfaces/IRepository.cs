using EPharm.Infrastructure.Context.Entities.Base;

namespace EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

public interface IRepository<T> where T : BaseEntity
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T?> GetByIdAsync(int id);
    public Task<T> InsertAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    public Task<int> SaveChangesAsync();
}
