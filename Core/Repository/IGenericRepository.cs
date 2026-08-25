using Core.Entity;
using System.Linq.Expressions;
using Dto.DTOs;

namespace Core.Repository;

public interface IGenericRepository<T, TID> where T : class, IEntity<TID>

{
    // Listeleme - Sıralama ve Sayfalama ile
    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? page = null,
        int? pageSize = null,
        params Expression<Func<T, object>>[]? includes);

    Task<DataTablesResponse<T>> ProcessTableRequest(
        DataTablesRequest request,
        Dictionary<string, string>? columnMappings = null,
        Expression<Func<T, bool>>? filter = null,
        params Expression<Func<T, object>>[]? includes);

    Task<IEnumerable<T>> WhereAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

    // Toplam Kayıt Sayısı
    Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);

    // Tekil Kayıt Getirme
    Task<T?> GetByIdAsync(TID id, params Expression<Func<T, object>>[]? includes);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[]? includes);

    // Ekleme
    Task<T> AddAsync(T entity);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

    // Güncelleme
    void Update(T entity);
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(IEnumerable<T> entities);

    // Silme
    void Delete(T entity);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);

    // Varlık Kontrolü
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter);

    Task<IEnumerable<T>> GetAllWithIncludeAsync(params Expression<Func<T, object>>[]? includes);

    Task<IEnumerable<T>> GetAllWithOptionsAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[]? includes);
}