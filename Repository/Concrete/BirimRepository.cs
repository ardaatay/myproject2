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
    /// <summary>
    /// Danışmalı kilidin ad alanı. Aynı veritabanındaki başka bir kilit
    /// kullanıcısıyla çakışmaması için birim ağacına ayrılmış sabit değer.
    /// </summary>
    private const int AgacKilidiAlani = 0x4252494D; // "BRIM"

    /// <summary>
    /// Bozuk veriye (üst birim döngüsü) karşı gezinme derinliği sınırı. Yol
    /// sütunu 900 karakterle sınırlı olduğu için gerçek ağaçlar bunun yanına
    /// yaklaşamaz.
    /// </summary>
    private const int AzamiDerinlik = 64;

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
            Sol = b.Sol,
            Sag = b.Sag,
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
            .OrderBy(b => b.Sol)
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

        // Alt ağaç, üst birimin nested set aralığına düşen kayıtlardır. Aralık
        // yalnızca kendi kiracısı içinde anlamlı olduğundan organizasyon koşulu
        // açıkça yazılır: global sorgu filtresi kapalıyken (tohumlama, arka plan
        // işleri) tek başına aralık koşulu kiracıları birbirine karıştırır.
        var query = context.Birimler.Where(b =>
            b.OrganizasyonId == ust.OrganizasyonId && b.Sol > ust.Sol && b.Sag < ust.Sag);

        if (sadeceAktif)
            query = query.Where(b => b.Durum);

        return await query
            .OrderBy(b => b.Sol)
            .Select(b => new BirimSecimDto { Id = b.Id, Ad = b.Ad })
            .ToListAsync();
    }

    public async Task<List<Birim>> GetDogrudanAltBirimlerAsync(int ustId)
    {
        return await context.Birimler
            .Where(b => b.UstId == ustId)
            .OrderBy(b => b.Sol)
            .ToListAsync();
    }

    public async Task<List<Birim>> GetAltAgacEntityAsync(Birim kok, bool kendisiDahil = false)
    {
        var query = context.Birimler.Where(b => b.OrganizasyonId == kok.OrganizasyonId);

        query = kendisiDahil
            ? query.Where(b => b.Sol >= kok.Sol && b.Sag <= kok.Sag)
            : query.Where(b => b.Sol > kok.Sol && b.Sag < kok.Sag);

        return await query.OrderBy(b => b.Sol).ToListAsync();
    }

    public async Task<List<ListBirimDto>> GetAgacAsync()
    {
        var birimler = await ListeQuery().OrderBy(b => b.Sol).ToListAsync();

        // Sol sırası ön sıralı gezinme sırasıdır: her düğüm kendi atalarının
        // hemen ardından gelir. Ad zinciri bu yüzden tek geçişte, seviyeye göre
        // budanan bir yığınla kurulabilir; ağacı bellekte yeniden inşa etmeye
        // gerek kalmaz.
        var zincir = new List<string>();

        foreach (var birim in birimler)
        {
            // Numaralandırma bozuksa (henüz kurulmamış ağaç, elle müdahale)
            // girinti kaymasın diye seviye yığının boyuyla sınırlanır.
            var derinlik = Math.Clamp(birim.Seviye, 0, zincir.Count);

            zincir.RemoveRange(derinlik, zincir.Count - derinlik);
            zincir.Add(birim.Ad);

            birim.TamYol = string.Join(" / ", zincir);
        }

        return birimler;
    }

    public async Task<int> AgaciYenidenKurAsync(int organizasyonId)
    {
        // Yapısal değişiklik ağacın tamamını yeniden numaraladığı için iki
        // eşzamanlı düzenleme birbirinin numaralarını ezebilir. Danışmalı kilit
        // kiracı başınadır ve işlem sonunda kendiliğinden bırakılır. Çağıranın
        // açık bir işlem içinde olması şart: aksi halde kilit, kendisini alan
        // ifadenin örtük işlemiyle birlikte hemen serbest kalır.
        await context.Database.ExecuteSqlAsync(
            $"SELECT pg_advisory_xact_lock({AgacKilidiAlani}, {organizasyonId})");

        // Kardeş sırası veritabanında belirlenir. Karşılaştırma tek yerde
        // kaldığı için uygulamanın metin karşılaştırması ile PostgreSQL'in
        // harmanlaması ayrışamaz; GroupBy kaynak sırasını koruduğundan sıra
        // gruplara aynen taşınır.
        var dugumler = await context.Birimler
            .IgnoreQueryFilters()
            .Where(b => b.OrganizasyonId == organizasyonId)
            .OrderBy(b => b.Sira).ThenBy(b => b.Ad).ThenBy(b => b.Id)
            .ToListAsync();

        if (dugumler.Count == 0)
            return 0;

        var cocuklar = dugumler
            .Where(b => b.UstId.HasValue)
            .GroupBy(b => b.UstId!.Value)
            .ToDictionary(g => g.Key, g => g.ToList());

        var kimlikler = dugumler.Select(b => b.Id).ToHashSet();

        // Üst birimi bu kiracıda bulunmayan artıklar da kök gibi ele alınır;
        // aksi halde numarasız kalır ve Sol'a bakan hiçbir sorguda görünmezler.
        var kokler = dugumler
            .Where(b => b.UstId is not { } ustId || !kimlikler.Contains(ustId))
            .ToList();

        var gezilen = new HashSet<int>(dugumler.Count);
        var sonraki = 1;

        void Gez(Birim dugum, int seviye, string ustYol)
        {
            // Döngüsel üst birim bağı ancak veritabanına elle dokunulursa
            // oluşabilir; oluştuğunda numaralandırma yine de sonlanmalı.
            if (!gezilen.Add(dugum.Id))
                return;

            dugum.Sol = sonraki++;
            dugum.Seviye = seviye;
            dugum.Yol = $"{ustYol}{dugum.Id}/";

            if (seviye < AzamiDerinlik && cocuklar.TryGetValue(dugum.Id, out var altlar))
            {
                foreach (var alt in altlar)
                    Gez(alt, seviye + 1, dugum.Yol);
            }

            dugum.Sag = sonraki++;
        }

        foreach (var kok in kokler)
            Gez(kok, 0, "/");

        // Köklerden erişilemeyen düğümler (derinlik sınırına takılan ya da
        // döngü içindeki dallar) da numaralanır; böylece hiçbir satır aralıksız
        // kalmaz.
        foreach (var artik in dugumler.Where(b => !gezilen.Contains(b.Id)))
            Gez(artik, 0, "/");

        // Değeri değişmeyen satırları EF zaten yazmaz; sayım yalnızca çağırana
        // kaç satırın gerçekten kaydığını bildirmek için.
        var degisen = context.ChangeTracker.Entries<Birim>()
            .Count(giris => giris.State == EntityState.Modified);

        await context.SaveChangesAsync();

        return degisen;
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

        // Order. Varsayılan sıra ağaçtaki sıradır: Sol tek başına hem hiyerarşiyi
        // hem kardeş düzenini kodladığı için ek sıralama anahtarı gerekmez.
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
                _ => filteredQuery.OrderBy(p => p.Sol)
            };
        }
        else
        {
            filteredQuery = filteredQuery.OrderBy(p => p.Sol);
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
