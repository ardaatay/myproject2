using Core.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Dto.DTOs;
using System.Linq.Dynamic.Core;

namespace Core.Repository;

public class GenericRepository<T, TId>(DbContext context) : IGenericRepository<T, TId>
    where T : class, IEntity<TId>
{
    protected readonly DbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public virtual async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? page = null,
        int? pageSize = null,
        params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _dbSet;

        // Include ilişkili entityleri
        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        // Filtreleme
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Sıralama
        if (orderBy != null)
        {
            query = orderBy(query);
        }

        // Sayfalama
        if (page.HasValue && pageSize.HasValue)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<DataTablesResponse<T>> ProcessTableRequest(
        DataTablesRequest request,
        Dictionary<string, string>? columnMappings = null,
        Expression<Func<T, bool>>? filter = null,
        params Expression<Func<T, object>>[]? includes)
    {
        var query = _dbSet as IQueryable<T>;

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        // Filtreleme
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Toplam kayıt sayısını al
        var recordsTotal = await query.CountAsync();
        var filteredQuery = query;

        // Global arama
        if (!string.IsNullOrEmpty(request.Searchs?.Value))
        {
            var columns = request.Columns ?? Enumerable.Empty<DataTablesRequest.Column>();
            var searchableColumns = columns
                .Where(c => c.Searchable && !string.IsNullOrEmpty(c.Data))
                .Select(c => columnMappings != null && c.Data != null && columnMappings.ContainsKey(c.Data)
                    ? columnMappings[c.Data]
                    : c.Data)
                .Where(c => c != null)
                .ToList();

            if (searchableColumns.Any())
            {
                // null kontrolü ile güvenli string filtreleme
                var predicates = searchableColumns
                    .Select(col => $"Convert.ToString({col}).ToLower().Contains(@0)")
                    .ToList();

                var predicate = string.Join(" OR ", predicates);

                try
                {
                    filteredQuery = filteredQuery.Where(predicate, request.Searchs.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Filtreleme hatası: {ex.Message}");
                }
            }
        }


        // Kolon bazlı arama
        foreach (var column in (request.Columns ?? Enumerable.Empty<DataTablesRequest.Column>()).Where(c => c.Searchable && !string.IsNullOrEmpty(c.Search?.Value)))
        {
            var colData = column.Data ?? "";
            var colName = columnMappings != null && columnMappings.ContainsKey(colData)
                ? columnMappings[colData]
                : colData;

            var searchValue = column.Search?.Value ?? "";

            try
            {
                // Kolon değerini string'e dönüştür ve içerme kontrolü yap
                filteredQuery = filteredQuery.Where($"(Convert({colName}, 'System.String')).Contains(@0)",
                    searchValue);
            }
            catch
            {
                // Alternatif yöntem
                filteredQuery = filteredQuery.Where($"({colName}.ToString()).Contains(@0)", searchValue);
            }
        }

        // Filtrelenen kayıt sayısı
        var recordsFiltered = await filteredQuery.CountAsync();

        // Sıralama
        if (request.Orders != null && request.Orders.Any())
        {
            var orderParams = new List<string>();

            foreach (var order in request.Orders)
            {
                var colIndex = order.Column;
                if (request.Columns != null && colIndex < request.Columns.Count)
                {
                    var colData = request.Columns[colIndex].Data;

                    if (!string.IsNullOrEmpty(colData))
                    {
                         var colName = columnMappings != null && columnMappings.ContainsKey(colData)
                        ? columnMappings[colData]
                        : colData;

                        var direction = (order.Dir ?? "asc").ToLower() == "asc" ? "" : " desc";
                        orderParams.Add($"{colName}{direction}");
                    }
                }
            }

            if (orderParams.Any())
            {
                var orderBy = string.Join(", ", orderParams);
                filteredQuery = filteredQuery.OrderBy(orderBy);
            }
        }

        // Sayfalama
        var data = await filteredQuery
            .Skip(request.Start)
            .Take(request.Length)
            .ToListAsync();

        // Yanıt oluştur
        return new DataTablesResponse<T>
        {
            Draw = request.Draw,
            RecordsTotal = recordsTotal,
            RecordsFiltered = recordsFiltered,
            Data = data
        };
    }

    public virtual async Task<IEnumerable<T>> WhereAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
    {
        IQueryable<T> query = _dbSet;

        // Filtreleme
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Sıralama
        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync();
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.CountAsync();
    }

    public virtual async Task<T?> GetByIdAsync(TId id, params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _dbSet;

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return await query.FirstOrDefaultAsync(e => e.Id!.Equals(id));
    }

    public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> filter,
        params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _dbSet;

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return await query.FirstOrDefaultAsync(filter);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        var entityList = entities.ToList();
        await _dbSet.AddRangeAsync(entityList);
        await _context.SaveChangesAsync();
        return entityList;
    }

    public virtual void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync();
    }

    public virtual void Delete(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbSet.AnyAsync(filter);
    }

    public virtual async Task<IEnumerable<T>> GetAllWithIncludeAsync(params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _dbSet;

        if (includes == null) return await query.ToListAsync();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllWithOptionsAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _dbSet;

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }

        return await query.ToListAsync();
    }
}