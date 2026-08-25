using Dto.DTOs;
using Dto.Raporlama;
using Microsoft.EntityFrameworkCore;
using Repository.Abstract;
using Repository.Context;
using System.Linq.Expressions;
using Util.Query;

namespace Repository.Concrete;

public class RaporlamaRepository(VarlikEnvanteriDbContext context) : IRaporlamaRepository
{
    public async Task<DataTablesResponse<ListRaporDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListRaporDto, bool>>? filter = null)
    {
        var query = context.RaporlamaDetay.AsQueryable();

        query = request.FilterBag.ApplyFilters(query);

        if (filter != null)
        {
            query = query.Where(filter);
        }


        // Global Arama
        if (!string.IsNullOrEmpty(request.Searchs?.Value))
        {
            var search = request.Searchs.Value.ToLower();
            query = query.Where(r =>
                (r.KategoriAd != null && r.KategoriAd.ToLower().Contains(search)) ||
                (r.AltKategoriAd != null && r.AltKategoriAd.ToLower().Contains(search)) ||
                (r.VarlikAdi != null && r.VarlikAdi.ToLower().Contains(search))
            );
        }

        var recordsTotal = await query.CountAsync();
        var recordsFiltered = recordsTotal;

        // Sıralama
        if (request.Columns != null && request.Orders != null && request.Orders.Count != 0)
        {
            var order = request.Orders.First();
            var columnName = request.Columns[order.Column].Data;
            var direction = (order.Dir ?? "asc").ToLower();
            var ascending = direction == "asc";

            query = columnName switch
            {
                "kategoriAd" => ascending ? query.OrderBy(p => p.KategoriAd) : query.OrderByDescending(p => p.KategoriAd),
                "altKategoriAd" => ascending ? query.OrderBy(p => p.AltKategoriAd) : query.OrderByDescending(p => p.AltKategoriAd),
                "varlikAdi" => ascending ? query.OrderBy(p => p.VarlikAdi) : query.OrderByDescending(p => p.VarlikAdi),
                "kullanimAmaci" => ascending ? query.OrderBy(p => p.KullanimAmaci) : query.OrderByDescending(p => p.KullanimAmaci),
                "miktar" => ascending ? query.OrderBy(p => p.Miktar) : query.OrderByDescending(p => p.Miktar),
                "durumAd" => ascending ? query.OrderBy(p => p.DurumAd) : query.OrderByDescending(p => p.DurumAd),
                "konum" => ascending ? query.OrderBy(p => p.Konum) : query.OrderByDescending(p => p.Konum),
                "varlikSahibi" => ascending ? query.OrderBy(p => p.VarlikSahibi) : query.OrderByDescending(p => p.VarlikSahibi),
                "varlikSahibiAltDepartman" => ascending
                    ? query.OrderBy(p => p.VarlikSahibiAltDepartman)
                    : query.OrderByDescending(p => p.VarlikSahibiAltDepartman),
                "operasyonelSahibi" => ascending
                    ? query.OrderBy(p => p.OperasyonelSahibi)
                    : query.OrderByDescending(p => p.OperasyonelSahibi),
                "envantereGirisTarihi" => ascending
                    ? query.OrderBy(p => p.EnvantereGirisTarihi)
                    : query.OrderByDescending(p => p.EnvantereGirisTarihi),
                "envanterGuncellemeTarihi" => ascending
                    ? query.OrderBy(p => p.EnvanterGuncellemeTarihi)
                    : query.OrderByDescending(p => p.EnvanterGuncellemeTarihi),
                "envanterdenCikisTarihi" => ascending
                    ? query.OrderBy(p => p.EnvanterdenCikisTarihi)
                    : query.OrderByDescending(p => p.EnvanterdenCikisTarihi),
                _ => ascending ? query.OrderBy(p => p.Id) : query.OrderByDescending(p => p.Id)
            };
        }
        else
        {
            query = query.OrderBy(r => r.Id);
        }

        // Sayfalama
        var data = await query
            .Skip(request.Start)
            .Take(request.Length)
            .ToListAsync();

        return new DataTablesResponse<ListRaporDto>
        {
            Draw = request.Draw,
            RecordsTotal = recordsTotal,
            RecordsFiltered = recordsFiltered,
            Data = data
        };
    }

    public async Task<List<ListRaporDto>> GetAllExcelAsync(string? search = null, FilterBag? filterBag = null)
    {
        var query = context.RaporlamaDetay.AsQueryable();

        if (filterBag != null)
            query = filterBag.ApplyFilters(query);

        // DurumId 3 olan kayıtları hariç tut
        query = query.Where(r => r.DurumId != 3);


        // Arama
        if (!string.IsNullOrEmpty(search))
        {
            var s = search.ToLower();
            query = query.Where(r =>
                (r.KategoriAd != null && r.KategoriAd.ToLower().Contains(s)) ||
                (r.AltKategoriAd != null && r.AltKategoriAd.ToLower().Contains(s)) ||
                (r.VarlikAdi != null && r.VarlikAdi.ToLower().Contains(s))
            );
        }

        return await query.ToListAsync();
    }
}