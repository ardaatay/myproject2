using Core.Logging;
using Core.Security;
using Core.Util;
using Dto.DTOs;
using Dto.Loglar;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

/// <summary>
/// Log yazma ve okuma.
///
/// Yazma tarafı EF yerine ayrı bir bağlantıyla ADO.NET kullanır: log, işin
/// kendi işlemine bağlı olmamalıdır. Aksi halde başarısız olup geri alınan bir
/// işlemin logu da silinir ve geriye hiçbir iz kalmaz — oysa asıl kaydedilmek
/// istenen tam olarak o durumdur.
/// </summary>
public class LogRepository(
    VarlikEnvanteriDbContext context,
    IAktifOrganizasyon aktifOrganizasyon) : ILogRepository
{
    /// <summary>Kod çakışması pratikte görülmez; yine de sonsuz döngüye girmeden birkaç kez denenir.</summary>
    private const int KodDenemeSayisi = 3;

    /// <summary>PostgreSQL'in benzersizlik ihlali durum kodu.</summary>
    private const string BenzersizlikIhlali = "23505";

    public void AddLog(Log entity)
    {
        // Oturum açılmadan oluşan kayıtlar kiracıya bağlanmazsa hiçbir listede
        // görünmez; tek kurumlu dağıtımda o organizasyona yazılır.
        if (entity.OrganizasyonId == 0)
            entity.OrganizasyonId = context.TekOrganizasyonId();

        using var connection = new NpgsqlConnection(context.Database.GetConnectionString());
        connection.Open();

        using var command = connection.CreateCommand();

        // Tablo ve sütun adları snake_case olduğu için tırnaklamaya gerek yok.
        command.CommandText =
            """
            INSERT INTO logs (organizasyon_id, method_name, class_name, parameters, executing_time,
                              return_value, error, username, basarili, hata_kodu, sure_ms, ip_adresi, yol)
            VALUES (@OrganizasyonId, @MethodName, @ClassName, @Parameters, @ExecutingTime,
                    @ReturnValue, @Error, @Username, @Basarili, @HataKodu, @SureMs, @IpAdresi, @Yol)
            """;

        command.Parameters.AddWithValue("OrganizasyonId", entity.OrganizasyonId);
        command.Parameters.AddWithValue("MethodName", Kisalt(entity.MethodName, 500)!);
        command.Parameters.AddWithValue("ClassName", Kisalt(entity.ClassName, 500)!);
        command.Parameters.AddWithValue("Parameters", Kisalt(entity.Parameters, 4000)!);
        command.Parameters.AddWithValue("ExecutingTime", entity.ExecutingTime);
        command.Parameters.AddWithValue("ReturnValue", (object?)Kisalt(entity.ReturnValue, 4000) ?? DBNull.Value);
        command.Parameters.AddWithValue("Error", (object?)Kisalt(entity.Error, 4000) ?? DBNull.Value);
        command.Parameters.AddWithValue("Username", Kisalt(entity.Username, 500)!);
        command.Parameters.AddWithValue("Basarili", entity.Basarili);
        command.Parameters.AddWithValue("HataKodu", (object?)entity.HataKodu ?? DBNull.Value);
        command.Parameters.AddWithValue("SureMs", entity.SureMs);
        command.Parameters.AddWithValue("IpAdresi", (object?)Kisalt(entity.IpAdresi, 45) ?? DBNull.Value);
        command.Parameters.AddWithValue("Yol", (object?)Kisalt(entity.Yol, 500) ?? DBNull.Value);

        command.ExecuteNonQuery();
    }

    public async Task<string> AddHataLogAsync(HataLog entity, CancellationToken cancellationToken = default)
    {
        if (entity.OrganizasyonId == 0)
            entity.OrganizasyonId = await context.TekOrganizasyonIdAsync(cancellationToken);

        await using var connection = new NpgsqlConnection(context.Database.GetConnectionString());
        await connection.OpenAsync(cancellationToken);

        for (var deneme = 1; ; deneme++)
        {
            await using var command = connection.CreateCommand();

            command.CommandText =
                """
                INSERT INTO hata_loglari (organizasyon_id, kod, olusma_tarihi, tur, mesaj, kullanici_mesaji,
                                          ayrinti, durum_kodu, yol, http_yontemi, kullanici, ip_adresi,
                                          istek_id, cozuldu)
                VALUES (@OrganizasyonId, @Kod, @OlusmaTarihi, @Tur, @Mesaj, @KullaniciMesaji,
                        @Ayrinti, @DurumKodu, @Yol, @HttpYontemi, @Kullanici, @IpAdresi,
                        @IstekId, false)
                """;

            command.Parameters.AddWithValue("OrganizasyonId", entity.OrganizasyonId);
            command.Parameters.AddWithValue("Kod", entity.Kod);
            command.Parameters.AddWithValue("OlusmaTarihi", entity.OlusmaTarihi);
            command.Parameters.AddWithValue("Tur", Kisalt(entity.Tur, 300)!);
            command.Parameters.AddWithValue("Mesaj", Kisalt(entity.Mesaj, 2000)!);
            command.Parameters.AddWithValue("KullaniciMesaji", (object?)Kisalt(entity.KullaniciMesaji, 2000) ?? DBNull.Value);
            command.Parameters.AddWithValue("Ayrinti", (object?)entity.Ayrinti ?? DBNull.Value);
            command.Parameters.AddWithValue("DurumKodu", entity.DurumKodu);
            command.Parameters.AddWithValue("Yol", (object?)Kisalt(entity.Yol, 500) ?? DBNull.Value);
            command.Parameters.AddWithValue("HttpYontemi", (object?)Kisalt(entity.HttpYontemi, 10) ?? DBNull.Value);
            command.Parameters.AddWithValue("Kullanici", (object?)Kisalt(entity.Kullanici, 500) ?? DBNull.Value);
            command.Parameters.AddWithValue("IpAdresi", (object?)Kisalt(entity.IpAdresi, 45) ?? DBNull.Value);
            command.Parameters.AddWithValue("IstekId", (object?)Kisalt(entity.IstekId, 100) ?? DBNull.Value);

            try
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
                return entity.Kod;
            }
            catch (PostgresException ex) when (ex.SqlState == BenzersizlikIhlali && deneme < KodDenemeSayisi)
            {
                // Aynı kod zaten kullanılmış: yenisiyle denenir. Bu durumda
                // aynı istekteki işlem logu eski kodu taşıyabilir; kullanıcıya
                // her zaman burada dönen kod gösterilir.
                entity.Kod = HataKodu.Uret();
            }
        }
    }

    public Task<DataTablesResponse<ListIslemLogDto>> IslemLoglariAsync(
        DataTablesRequest request,
        LogFiltreDto filtre)
    {
        var sorgu = context.Logs.AsNoTracking().AsQueryable();

        if (filtre.Baslangic is { } baslangic)
            sorgu = sorgu.Where(l => l.ExecutingTime >= baslangic.Date);

        if (filtre.BitisGunSonu is { } bitis)
            sorgu = sorgu.Where(l => l.ExecutingTime < bitis);

        if (!string.IsNullOrWhiteSpace(filtre.Kullanici))
        {
            var kullanici = filtre.Kullanici.Trim().ToLower();
            sorgu = sorgu.Where(l => l.Username.ToLower().Contains(kullanici));
        }

        if (filtre.YalnizcaSorunlu)
            sorgu = sorgu.Where(l => !l.Basarili);

        if (HataKodu.Duzelt(filtre.HataKodu) is { } kod)
            sorgu = sorgu.Where(l => l.HataKodu == kod);

        var projeksiyon = sorgu
            .OrderByDescending(l => l.Id)
            .Select(l => new ListIslemLogDto
            {
                Id = l.Id,
                Tarih = l.ExecutingTime,
                Kullanici = l.Username,
                Modul = l.ClassName,
                Islem = l.MethodName,
                Basarili = l.Basarili,
                DurumStr = l.Basarili ? "Başarılı" : "Hatalı",
                HataKodu = l.HataKodu,
                SureMs = l.SureMs,
                Yol = l.Yol,
                IpAdresi = l.IpAdresi
            });

        return DataTablesHelper.ProcessAsync(projeksiyon, request);
    }

    public async Task<IslemLogDetayDto?> IslemLogGetirAsync(int id)
    {
        var detay = await context.Logs
            .AsNoTracking()
            .Where(l => l.Id == id)
            .Select(l => new IslemLogDetayDto
            {
                Id = l.Id,
                Tarih = l.ExecutingTime,
                Kullanici = l.Username,
                Modul = l.ClassName,
                Islem = l.MethodName,
                Basarili = l.Basarili,
                DurumStr = l.Basarili ? "Başarılı" : "Hatalı",
                HataKodu = l.HataKodu,
                SureMs = l.SureMs,
                Yol = l.Yol,
                IpAdresi = l.IpAdresi,
                Parametreler = l.Parameters,
                DonusDegeri = l.ReturnValue,
                Hata = l.Error
            })
            .FirstOrDefaultAsync();

        if (detay?.HataKodu is { } kod)
        {
            detay.HataLogId = await KodSorgusu(kod)
                .Select(h => (int?)h.Id)
                .FirstOrDefaultAsync();
        }

        return detay;
    }

    public Task<DataTablesResponse<ListHataLogDto>> HataLoglariAsync(
        DataTablesRequest request,
        LogFiltreDto filtre)
    {
        var sorgu = context.HataLoglari.AsNoTracking().AsQueryable();

        if (filtre.Baslangic is { } baslangic)
            sorgu = sorgu.Where(h => h.OlusmaTarihi >= baslangic.Date);

        if (filtre.BitisGunSonu is { } bitis)
            sorgu = sorgu.Where(h => h.OlusmaTarihi < bitis);

        if (!string.IsNullOrWhiteSpace(filtre.Kullanici))
        {
            var kullanici = filtre.Kullanici.Trim().ToLower();
            sorgu = sorgu.Where(h => (h.Kullanici ?? "").ToLower().Contains(kullanici));
        }

        if (filtre.YalnizcaSorunlu)
            sorgu = sorgu.Where(h => !h.Cozuldu);

        if (HataKodu.Duzelt(filtre.HataKodu) is { } kod)
            sorgu = sorgu.Where(h => h.Kod == kod);

        var projeksiyon = sorgu
            .OrderByDescending(h => h.Id)
            .Select(h => new ListHataLogDto
            {
                Id = h.Id,
                Kod = h.Kod,
                Tarih = h.OlusmaTarihi,
                Tur = h.Tur,
                Mesaj = h.Mesaj,
                DurumKodu = h.DurumKodu,
                Yol = h.Yol,
                Kullanici = h.Kullanici,
                Cozuldu = h.Cozuldu,
                DurumStr = h.Cozuldu ? "Çözüldü" : "Açık"
            });

        return DataTablesHelper.ProcessAsync(projeksiyon, request);
    }

    public Task<HataLogDetayDto?> HataLogGetirAsync(int id) =>
        DetayGetirAsync(context.HataLoglari.AsNoTracking().Where(h => h.Id == id));

    public Task<HataLogDetayDto?> HataKoduIleGetirAsync(string kod)
    {
        var duzeltilmis = HataKodu.Duzelt(kod);

        return duzeltilmis is null
            ? Task.FromResult<HataLogDetayDto?>(null)
            : DetayGetirAsync(KodSorgusu(duzeltilmis));
    }

    public async Task<bool> CozumIsaretleAsync(int id, bool cozuldu, string? not, string kullanici)
    {
        var kayit = await context.HataLoglari.FirstOrDefaultAsync(h => h.Id == id);

        if (kayit is null)
            return false;

        kayit.Cozuldu = cozuldu;
        kayit.CozumNotu = string.IsNullOrWhiteSpace(not) ? null : not.Trim();
        kayit.CozulmeTarihi = cozuldu ? DateTime.Now : null;
        kayit.CozenKullanici = cozuldu ? kullanici : null;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<(int Toplam, int Cozulmemis, int SonYirmiDortSaat)> HataOzetiAsync()
    {
        var esik = DateTime.Now.AddHours(-24);

        var toplam = await context.HataLoglari.CountAsync();
        var cozulmemis = await context.HataLoglari.CountAsync(h => !h.Cozuldu);
        var sonGun = await context.HataLoglari.CountAsync(h => h.OlusmaTarihi >= esik);

        return (toplam, cozulmemis, sonGun);
    }

    private async Task<HataLogDetayDto?> DetayGetirAsync(IQueryable<HataLog> sorgu)
    {
        var detay = await sorgu
            .Select(h => new HataLogDetayDto
            {
                Id = h.Id,
                Kod = h.Kod,
                Tarih = h.OlusmaTarihi,
                Tur = h.Tur,
                Mesaj = h.Mesaj,
                DurumKodu = h.DurumKodu,
                Yol = h.Yol,
                Kullanici = h.Kullanici,
                Cozuldu = h.Cozuldu,
                DurumStr = h.Cozuldu ? "Çözüldü" : "Açık",
                KullaniciMesaji = h.KullaniciMesaji,
                Ayrinti = h.Ayrinti,
                HttpYontemi = h.HttpYontemi,
                IpAdresi = h.IpAdresi,
                IstekId = h.IstekId,
                CozumNotu = h.CozumNotu,
                CozulmeTarihi = h.CozulmeTarihi,
                CozenKullanici = h.CozenKullanici
            })
            .FirstOrDefaultAsync();

        if (detay is not null)
        {
            detay.IslemLogId = await context.Logs
                .AsNoTracking()
                .Where(l => l.HataKodu == detay.Kod)
                .Select(l => (int?)l.Id)
                .FirstOrDefaultAsync();
        }

        return detay;
    }

    /// <summary>
    /// Koda göre arama, kiracı süzgecini bilerek atlar: oturum açılmadan
    /// oluşan hatalar (giriş ekranı gibi) hiçbir kiracıya bağlı değildir ve
    /// aksi halde koduyla bile bulunamazdı. Kodun tahmin edilemez olması bu
    /// erişimi sınırlar; yine de yalnızca kendi kiracısının ve sahipsiz
    /// kayıtların görülmesine izin verilir.
    /// </summary>
    private IQueryable<HataLog> KodSorgusu(string kod)
    {
        var sorgu = context.HataLoglari.AsNoTracking().IgnoreQueryFilters().Where(h => h.Kod == kod);

        // Kurumlar arası yetkili (null) tüm kayıtları görür.
        if (aktifOrganizasyon.Id is { } organizasyonId)
            sorgu = sorgu.Where(h => h.OrganizasyonId == organizasyonId || h.OrganizasyonId == 0);

        return sorgu;
    }

    private static string? Kisalt(string? deger, int uzunluk)
    {
        if (deger is null)
            return null;

        return deger.Length <= uzunluk ? deger : deger[..uzunluk];
    }
}
