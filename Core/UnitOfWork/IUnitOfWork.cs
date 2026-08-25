using Core.Entity;
using Core.Repository;

namespace Core.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    IGenericRepository<T, TID> GetRepository<T, TID>() where T : class, IEntity<TID>;
    Task<int> SaveChangesAsync();
    Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> action);
    Task ExecuteInTransactionAsync(Func<Task> action);
}