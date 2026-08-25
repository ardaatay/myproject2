using System.Linq.Expressions;
using Core.Repository;
using Dto.Birim;
using Dto.DTOs;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class BirimRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<Birim, int>(context), IBirimRepository
{
    private IQueryable<ListBirimDto> ListeQuery() =>
        from b in context.Birimler
        join u in context.Birimler on b.UstId equals u.Id into ust
        from u in ust.DefaultIfEmpty()
        select new ListBirimDto
        {
            Id = b.Id,
            UstId = b.UstId,
            Ad = b.Ad,
            Kod = b.Kod,
            UstAd = u != null ? u.Ad : "",
            Seviye = b.Seviye,
            Sira = b.Sira,
            Durum = b.Durum,
            DurumStr = b.Durum ? "Aktif" : "Pasif",
            TamYol = b.Yol
        };

    public async Task<List<BirimSecimDto>> GetKokBirimlerAsync(bool sadeceAktif = true)
    {
        var query = context.Birimler.Where(b => b.UstId == null);

        if (sadeceAktif)
            query = query.Where(b => b.Durum);

        return await query
            .OrderBy(b => b.Sira).ThenBy(b => b.Ad)
            .Select(b => new BirimSecimDto { Id = b.Id, Ad = b.Ad })
            .ToListAsync();
    }

    public async Task<List<BirimSecimDto>> GetAltAgacAsync(int ustId, bool sadeceAktif = true)
    {
        var ust = await context.Birimler
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == ustId);

        if (ust is null)
            return [];

        // Yol "/1/5/12/" biçiminde olduğu için StartsWith ile alt ağaç tam olarak
        // eşleşir; "/1/5/" öneki "/1/50/" ile karışmaz çünkü ayraç sonda da var.
        var query = context.Birimler.Where(b => b.Yol.StartsWith(ust.Yol) && b.Id != ustId);

        if (sadeceAktif)
            query = query.Where(b => b.Durum);

        return await query
            .OrderBy(b => b.Seviye).ThenBy(b => b.Sira).ThenBy(b => b.Ad)
            .Select(b => new BirimSecimDto { Id = b.Id, Ad = b.Ad })
            .ToListAsync();
    }

    public async Task<List<Birim>> GetDogrudanAltBirimlerAsync(int ustId)
    {
        return await context.Birimler
            .Where(b => b.UstId == ustId)
            .OrderBy(b => b.Sira).ThenBy(b => b.Ad)
            .ToListAsync();
    }

    public async Task<List<Birim>> GetAltAgacEntityAsync(string yolOneki)
    {
        return await context.Birimler
            .Where(b => b.Yol.StartsWith(yolOneki))
            .ToListAsync();
    }

    public async Task<List<ListBirimDto>> GetAgacAsync()
    {
        var birimler = await ListeQuery().ToListAsync();

        // Hiyerarşik sıra: her düğüm kendi atalarının hemen ardından gelir.
        // Sıralama Yol üzerinden yapılamaz (kimlikler metin olarak sıralanır),
        // bu yüzden ağaç bellekte gezilerek düzleştirilir.
        // Kök birimler 0 anahtarı altında toplanır; kimlikler 1'den başladığı
        // için bu değerle çakışma olmaz.
        var cocuklar = birimler
            .GroupBy(b => b.UstId ?? 0)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.Sira).ThenBy(x => x.Ad).ToList());

        var sonuc = new List<ListBirimDto>(birimler.Count);
        var adlar = birimler.ToDictionary(b => b.Id, b => b.Ad);

        void Gez(int ustId, string onek)
        {
            if (!cocuklar.TryGetValue(ustId, out var dugumler))
                return;

            foreach (var dugum in dugumler)
            {
                dugum.TamYol = onek.Length == 0 ? dugum.Ad : $"{onek} / {dugum.Ad}";
                sonuc.Add(dugum);
                Gez(dugum.Id, dugum.TamYol);
            }
        }

        Gez(0, "");

        // Üst birimi silinmiş / erişilemez olan artıklar da listede görünsün.
        var yerlesenler = sonuc.Select(b => b.Id).ToHashSet();
        foreach (var artik in birimler.Where(b => !yerlesenler.Contains(b.Id)))
        {
            artik.TamYol = adlar.TryGetValue(artik.UstId ?? 0, out var ustAd)
                ? $"{ustAd} / {artik.Ad}"
                : artik.Ad;
            sonuc.Add(artik);
        }

        return sonuc;
    }

    public async Task<DataTablesResponse<ListBirimDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListBirimDto, bool>>? filter = null)
    {
        var query = ListeQuery();

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
                p.Ad.ToLower().Contains(lowerSearch) ||
                (p.Kod ?? "").ToLower().Contains(lowerSearch) ||
                p.UstAd.ToLower().Contains(lowerSearch) ||
                p.DurumStr.ToLower().Contains(lowerSearch)
            );
        }

        // Column-based search
        foreach (var column in (request.Columns ?? Enumerable.Empty<DataTablesRequest.Column>())
                 .Where(c => c.Searchable && !string.IsNullOrEmpty(c.Search?.Value)))
        {
            var val = column.Search?.Value?.ToLower() ?? "";
            switch (column.Data)
            {
                case "ad":
                    filteredQuery = filteredQuery.Where(p => p.Ad.ToLower().Contains(val));
                    break;
                case "kod":
                    filteredQuery = filteredQuery.Where(p => (p.Kod ?? "").ToLower().Contains(val));
                    break;
                case "ustAd":
                    filteredQuery = filteredQuery.Where(p => p.UstAd.ToLower().Contains(val));
                    break;
                case "durumStr":
                    filteredQuery = filteredQuery.Where(p => p.DurumStr.ToLower().Contains(val));
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
                "kod" => direction
                    ? filteredQuery.OrderByDescending(p => p.Kod)
                    : filteredQuery.OrderBy(p => p.Kod),
                "ustAd" => direction
                    ? filteredQuery.OrderByDescending(p => p.UstAd)
                    : filteredQuery.OrderBy(p => p.UstAd),
                "durumStr" => direction
                    ? filteredQuery.OrderByDescending(p => p.DurumStr)
                    : filteredQuery.OrderBy(p => p.DurumStr),
                _ => filteredQuery.OrderBy(p => p.Seviye).ThenBy(p => p.Sira).ThenBy(p => p.Ad)
            };
        }
        else
        {
            filteredQuery = filteredQuery.OrderBy(p => p.Seviye).ThenBy(p => p.Sira).ThenBy(p => p.Ad);
        }

        // Paging
        var data = await filteredQuery
            .Skip(request.Start)
            .Take(request.Length)
            .ToListAsync();

        return new DataTablesResponse<ListBirimDto>
        {
            Draw = request.Draw,
            RecordsTotal = recordsTotal,
            RecordsFiltered = recordsFiltered,
            Data = data
        };
    }
}
