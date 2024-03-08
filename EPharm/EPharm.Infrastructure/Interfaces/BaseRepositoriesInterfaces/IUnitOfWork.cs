namespace EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

public interface IUnitOfWork
{
    public Task<bool> SaveChangesAsync();
    public Task BeginTransactionAsync();
    public Task CommitTransactionAsync();
    public Task RollbackTransactionAsync();
}
