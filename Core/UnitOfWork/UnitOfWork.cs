using Core.Entity;
using Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed;
    private readonly Dictionary<Type, object> _repositories;

    public UnitOfWork(DbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    public IGenericRepository<T, TID> GetRepository<T, TID>() where T : class, IEntity<TID>
    {
        if (_repositories.ContainsKey(typeof(T)))
        {
            return (IGenericRepository<T, TID>)_repositories[typeof(T)];
        }

        var repository = new GenericRepository<T, TID>(_context);
        _repositories.Add(typeof(T), repository);
        return repository;
    }

    public async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> action)
    {
        if (_transaction != null)
        {
            return await action();
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var result = await action();
            await SaveChangesAsync();
            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task ExecuteInTransactionAsync(Func<Task> action)
    {
        if (_transaction != null)
        {
            await action();
            return;
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await action();
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
            await _context.DisposeAsync();
            _disposed = true;
        }
    }
}