using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.Kullanici;
using Dto.KullaniciBirim;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class KullaniciBirimRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<KullaniciBirim, int>(context), IKullaniciBirimRepository
{
    public async Task<DataTablesResponse<ListKullaniciBirimDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListKullaniciBirimDto, bool>>? filter = null)
    {
        var query = from p in context.KullaniciBirimler
            join k in context.Kullanicilar on p.KullaniciId equals k.Id
            select new ListKullaniciBirimDto
            {
                Id = p.Id,
                KullaniciId = p.KullaniciId,
                Username = k.Username ?? "",
                BirimAd = p.BirimAd ?? "",
                DurumStr = p.Durum ? "Aktif" : "Pasif",
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
                (p.Username ?? "").ToLower().Contains(lowerSearch) ||
                (p.BirimAd ?? "").ToLower().Contains(lowerSearch) ||
                (p.DurumStr ?? "").ToLower().Contains(lowerSearch)
            );
        }

        // Column-based search
        foreach (var column in (request.Columns ?? Enumerable.Empty<DataTablesRequest.Column>()).Where(c => c.Searchable && !string.IsNullOrEmpty(c.Search?.Value)))
        {
            var val = column.Search?.Value?.ToLower() ?? "";
            switch (column.Data)
            {
                case "username":
                    filteredQuery = filteredQuery.Where(p => (p.Username ?? "").ToLower().Contains(val));
                    break;
                case "birimAd":
                    filteredQuery =
                        filteredQuery.Where(p => (p.BirimAd ?? "").ToLower().Contains(val));
                    break;
                case "durumStr":
                    filteredQuery = filteredQuery.Where(p => (p.DurumStr ?? "").ToLower().Contains(val));
                    break;
                default:
                    break;
            }
        }

        var recordsFiltered = await filteredQuery.CountAsync();

        // Order
        if (request.Columns != null && request.Orders != null && request.Orders.Any())
        {
            var order = request.Orders.First();
            var columnName = request.Columns[order.Column].Data;
            var direction = (order.Dir ?? "asc").ToLower() != "asc";

            filteredQuery = columnName switch
            {
                "username" => direction
                    ? filteredQuery.OrderByDescending(p => p.Username)
                    : filteredQuery.OrderBy(p => p.Username),
                "birimAd" => direction
                    ? filteredQuery.OrderByDescending(p => p.BirimAd)
                    : filteredQuery.OrderBy(p => p.BirimAd),
                "durumStr" => direction
                    ? filteredQuery.OrderByDescending(p => p.DurumStr)
                    : filteredQuery.OrderBy(p => p.DurumStr),
                _ => filteredQuery
            };
        }

        // Paging
        var data = await filteredQuery
            .Skip(request.Start)
            .Take(request.Length)
            .ToListAsync();

        return new DataTablesResponse<ListKullaniciBirimDto>
        {
            Draw = request.Draw,
            RecordsTotal = recordsTotal,
            RecordsFiltered = recordsFiltered,
            Data = data
        };
    }

    public async Task<ListKullaniciBirimDto?> GetKullaniciBirimById(int id)
    {
        var query = await (from p in context.KullaniciBirimler
            join k in context.Kullanicilar on p.KullaniciId equals k.Id
            where p.Id == id
            select new ListKullaniciBirimDto
            {
                Id = p.Id,
                KullaniciId = p.KullaniciId,
                Username = k.Username ?? "",
                BirimAd = p.BirimAd ?? "",
                DurumStr = p.Durum ? "Aktif" : "Pasif",
            }).FirstOrDefaultAsync();

        return query;
    }

    public async Task<List<KullaniciBirim>> GetByKullaniciIdAsync(int kullaniciId)
    {
        var kullaniciBirimleri = await context.KullaniciBirimler
            .Where(x => x.KullaniciId == kullaniciId && x.Durum == true)
            .ToListAsync();

        return kullaniciBirimleri;
    }
}