using EPharm.Infrastructure.Context.Entities.Base;

namespace EPharm.Infrastructure.Interfaces.Base;

public interface IRepository<T> where T : BaseEntity
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T?> GetByIdAsync(int id);
    public Task<T> InsertAsync(T entity);
    public Task InsertManyAsync(IEnumerable<T> entities);

    void Update(T entity);
    void Delete(T entity);
    public Task<int> SaveChangesAsync();
}
