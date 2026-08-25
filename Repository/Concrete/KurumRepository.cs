using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.Kurum;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class KurumRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<Kurum, int>(context), IKurumRepository
{
    public async Task<List<ListKurumDto>> GetListWithDetailsAsync(Expression<Func<ListKurumDto, bool>>? filter = null)
    {
        var query = from p in context.Set<Kurum>()
            select new ListKurumDto
            {
                Id = p.Id,
                Ad = p.Ad ?? "",
                Durum = p.Durum
            };

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<List<ListKurumDto>> GetListWithDetailsAsync(string search,
        Expression<Func<ListKurumDto, bool>>? filter = null)
    {
        var query = from p in context.Set<Kurum>()
            select new ListKurumDto
            {
                Id = p.Id,
                Ad = p.Ad ?? "",
                Durum = p.Durum
            };

        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Global search
        if (!string.IsNullOrEmpty(search))
        {
            var lowerSearch = search.ToLower();

            query = query.Where(p =>
                p.Ad.ToLower().Contains(lowerSearch)
            );
        }

        return await query.ToListAsync();
    }

    public async Task<DataTablesResponse<ListKurumDto>> ProcessTableRequestAsync(DataTablesRequest request,
        Expression<Func<ListKurumDto, bool>>? filter = null)
    {
        var query = from p in context.Set<Kurum>()
            select new ListKurumDto
            {
                Id = p.Id,
                Ad = p.Ad ?? "",
                Durum = p.Durum
            };

        if (filter != null)
        {
            query = query.Where(filter);
        }

        var recordsTotal = await query.CountAsync();
        var filteredQuery = query;

        // Global search
        if (!string.IsNullOrEmpty(request.Searchs?.Value))
        {
            var lowerSearch = request.Searchs.Value.ToLower();

            filteredQuery = filteredQuery.Where(p =>
                p.Ad.ToLower().Contains(lowerSearch)
            );
        }

        // Column-based search
        foreach (var column in (request.Columns ?? Enumerable.Empty<DataTablesRequest.Column>()).Where(c => c.Searchable && !string.IsNullOrEmpty(c.Search?.Value)))
        {
            var val = column.Search?.Value?.ToLower() ?? "";
            switch (column.Data)
            {
                case "ad":
                    filteredQuery = filteredQuery.Where(p => p.Ad.ToLower().Contains(val));
                    break;
                default:
                    break;
            }
        }

        var recordsFiltered = await filteredQuery.CountAsync();

        // Order
        if (request.Columns != null && request.Orders != null && request.Orders.Count != 0)
        {
            var order = request.Orders.First();
            var columnName = request.Columns[order.Column].Data;
            var direction = (order.Dir ?? "asc").ToLower() != "asc";

            filteredQuery = columnName switch
            {
                "ad" => direction
                    ? filteredQuery.OrderByDescending(p => p.Ad)
                    : filteredQuery.OrderBy(p => p.Ad),
                _ => filteredQuery
            };
        }

        // Paging
        var data = await filteredQuery
            .Skip(request.Start)
            .Take(request.Length)
            .ToListAsync();

        return new DataTablesResponse<ListKurumDto>
        {
            Draw = request.Draw,
            RecordsTotal = recordsTotal,
            RecordsFiltered = recordsFiltered,
            Data = data
        };
    }
}