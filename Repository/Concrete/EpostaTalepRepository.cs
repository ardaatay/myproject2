using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.EpostaTalep;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace Repository.Concrete;

public class EpostaTalepRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<EpostaTalep, int>(context), IEpostaTalepRepository
{
    public async Task<List<ListEpostaTalepDto>> GetListWithDetailsAsync(
        Expression<Func<ListEpostaTalepDto, bool>>? filter = null)
    {
        var query = from p in context.Set<EpostaTalep>()
            join c in context.Set<Kurum>() on p.KurumId equals c.Id into categoryJoin
            from c in categoryJoin.DefaultIfEmpty()
            select new ListEpostaTalepDto
            {
                Id = p.Id,
                KurumId = p.KurumId,
                KurumAd = c != null ? c.Ad ?? "" : "",
                UcuncuTaraf = p.UcuncuTaraf,
                TalepEdilen = p.TalepEdilen,
                TalepEden = p.TalepEden,
                TalepNedeni = p.TalepNedeni,
                TalepSuresi = p.TalepSuresi,
                DosyaYolu = p.DosyaYolu,
                Durum = p.Durum ? "Aktif" : "Pasif"
            };

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<List<ListEpostaTalepDto>> GetListWithDetailsAsync(string search,
        Expression<Func<ListEpostaTalepDto, bool>>? filter = null)
    {
        var query = from p in context.Set<EpostaTalep>()
            join c in context.Set<Kurum>() on p.KurumId equals c.Id into categoryJoin
            from c in categoryJoin.DefaultIfEmpty()
            select new ListEpostaTalepDto
            {
                Id = p.Id,
                KurumId = p.KurumId,
                KurumAd = c != null ? c.Ad ?? "" : "",
                UcuncuTaraf = p.UcuncuTaraf,
                TalepEdilen = p.TalepEdilen,
                TalepEden = p.TalepEden,
                TalepNedeni = p.TalepNedeni,
                TalepSuresi = p.TalepSuresi,
                DosyaYolu = p.DosyaYolu,
                Durum = p.Durum ? "Aktif" : "Pasif"
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
                (p.KurumAd != null && p.KurumAd.ToLower().Contains(lowerSearch)) ||
                (p.UcuncuTaraf != null && p.UcuncuTaraf.ToLower().Contains(lowerSearch)) ||
                (p.TalepEdilen != null && p.TalepEdilen.ToLower().Contains(lowerSearch)) ||
                (p.TalepEden != null && p.TalepEden.ToLower().Contains(lowerSearch)) ||
                (p.TalepNedeni != null && p.TalepNedeni.ToLower().Contains(lowerSearch))
            );
        }

        return await query.ToListAsync();
    }

    public async Task<DataTablesResponse<ListEpostaTalepDto>> ProcessTableRequestAsync(DataTablesRequest request,
        Expression<Func<ListEpostaTalepDto, bool>>? filter = null)
    {
        var query = from p in context.Set<EpostaTalep>()
            join c in context.Set<Kurum>() on p.KurumId equals c.Id into categoryJoin
            from c in categoryJoin.DefaultIfEmpty()
            select new ListEpostaTalepDto
            {
                Id = p.Id,
                KurumId = p.KurumId,
                KurumAd = c != null ? c.Ad ?? "" : "",
                UcuncuTaraf = p.UcuncuTaraf,
                TalepEdilen = p.TalepEdilen,
                TalepEden = p.TalepEden,
                TalepNedeni = p.TalepNedeni,
                TalepSuresi = p.TalepSuresi,
                DosyaYolu = p.DosyaYolu,
                Durum = p.Durum ? "Aktif" : "Pasif"
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
                (p.KurumAd != null && p.KurumAd.ToLower().Contains(lowerSearch)) ||
                (p.UcuncuTaraf != null && p.UcuncuTaraf.ToLower().Contains(lowerSearch)) ||
                (p.TalepEdilen != null && p.TalepEdilen.ToLower().Contains(lowerSearch)) ||
                (p.TalepEden != null && p.TalepEden.ToLower().Contains(lowerSearch)) ||
                (p.TalepNedeni != null && p.TalepNedeni.ToLower().Contains(lowerSearch))
            );
        }

        // Column-based search
        foreach (var column in (request.Columns ?? Enumerable.Empty<DataTablesRequest.Column>()).Where(c => c.Searchable && !string.IsNullOrEmpty(c.Search?.Value)))
        {
            var val = column.Search?.Value?.ToLower() ?? "";
            switch (column.Data)
            {
                case "kurumAd":
                    filteredQuery = filteredQuery.Where(p => p.KurumAd.ToLower().Contains(val));
                    break;
                case "ucuncuTaraf":
                    filteredQuery =
                        filteredQuery.Where(p => p.UcuncuTaraf != null && p.UcuncuTaraf.ToLower().Contains(val));
                    break;
                case "talepEdilen":
                    filteredQuery =
                        filteredQuery.Where(p => p.TalepEdilen != null && p.TalepEdilen.ToLower().Contains(val));
                    break;
                case "talepEden":
                    filteredQuery =
                        filteredQuery.Where(p => p.TalepEden != null && p.TalepEden.ToLower().Contains(val));
                    break;
                case "talepNedeni":
                    filteredQuery =
                        filteredQuery.Where(p => p.TalepNedeni != null && p.TalepNedeni.ToLower().Contains(val));
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
                "kurumAd" => direction
                    ? filteredQuery.OrderByDescending(p => p.KurumAd)
                    : filteredQuery.OrderBy(p => p.KurumAd),
                "ucuncuTaraf" => direction
                    ? filteredQuery.OrderByDescending(p => p.UcuncuTaraf)
                    : filteredQuery.OrderBy(p => p.UcuncuTaraf),
                "talepEdilen" => direction
                    ? filteredQuery.OrderByDescending(p => p.TalepEdilen)
                    : filteredQuery.OrderBy(p => p.TalepEdilen),
                "talepEden" => direction
                    ? filteredQuery.OrderByDescending(p => p.TalepEden)
                    : filteredQuery.OrderBy(p => p.TalepEden),
                "talepNedeni" => direction
                    ? filteredQuery.OrderByDescending(p => p.TalepNedeni)
                    : filteredQuery.OrderBy(p => p.TalepNedeni),
                _ => filteredQuery
            };
        }

        // Paging
        var data = await filteredQuery
            .Skip(request.Start)
            .Take(request.Length)
            .ToListAsync();

        return new DataTablesResponse<ListEpostaTalepDto>
        {
            Draw = request.Draw,
            RecordsTotal = recordsTotal,
            RecordsFiltered = recordsFiltered,
            Data = data
        };
    }
}