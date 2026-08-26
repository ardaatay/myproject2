using System.Text;
using Business.Abstract;
using Core.Logging;
using Dto.DTOs;
using Dto.Loglar;
using Entity.Concrete;
using Microsoft.Extensions.Logging;
using Repository.Abstract;

namespace Business.Concrete;

/// <summary>
/// Log yazma ve okuma. Yazma tarafında hiçbir istisna dışarı sızmaz: loglama
/// bir yan iştir, asıl işi ya da kullanıcıya gösterilecek hatayı gölgelememelidir.
/// </summary>
public class LogManager(
    ILogRepository repository,
    IIstekBaglami istekBaglami,
    ILogger<LogManager> logger) : ILogService
{
    /// <summary>Yığın izi kayıtta bu uzunlukta kırpılır; ayrıntı için yeterli, tablo için makul.</summary>
    private const int AyrintiUzunlugu = 20000;

    public void Add(Log entity)
    {
        try
        {
            repository.AddLog(entity);
        }
        catch (Exception ex)
        {
            // Log yazılamadıysa yapılacak tek şey bunu uygulama günlüğüne
            // düşürmek. Çağıran hiçbir şey fark etmemelidir.
            logger.LogError(ex, "İşlem logu yazılamadı: {Sinif}.{Metot}", entity.ClassName, entity.MethodName);
        }
    }

    public async Task<string> HataKaydetAsync(
        Exception istisna,
        int durumKodu,
        string? kullaniciMesaji,
        CancellationToken cancellationToken = default)
    {
        var kod = istekBaglami.HataKodu();

        var kayit = new HataLog
        {
            OrganizasyonId = istekBaglami.OrganizasyonId,
            Kod = kod,
            OlusmaTarihi = DateTime.Now,
            Tur = istisna.GetType().Name,
            Mesaj = istisna.Message,
            KullaniciMesaji = kullaniciMesaji,
            Ayrinti = AyrintiUret(istisna),
            DurumKodu = durumKodu,
            Yol = istekBaglami.Yol,
            HttpYontemi = istekBaglami.HttpYontemi,
            Kullanici = istekBaglami.Kullanici,
            IpAdresi = istekBaglami.IpAdresi,
            IstekId = istekBaglami.IstekId
        };

        try
        {
            return await repository.AddHataLogAsync(kayit, cancellationToken);
        }
        catch (Exception ex)
        {
            // Kayıt tutulamasa bile kullanıcıya bir kod verilir ve aynı kod
            // uygulama günlüğüne yazılır; hata yine de izlenebilir kalır.
            logger.LogError(ex, "Hata logu yazılamadı. Kod: {Kod}", kod);
            logger.LogError(istisna, "Kaydedilemeyen hata. Kod: {Kod}", kod);

            return kod;
        }
    }

    public Task<DataTablesResponse<ListIslemLogDto>> IslemLoglariAsync(DataTablesRequest request, LogFiltreDto filtre) =>
        repository.IslemLoglariAsync(request, filtre);

    public Task<IslemLogDetayDto?> IslemLogGetirAsync(int id) =>
        repository.IslemLogGetirAsync(id);

    public Task<DataTablesResponse<ListHataLogDto>> HataLoglariAsync(DataTablesRequest request, LogFiltreDto filtre) =>
        repository.HataLoglariAsync(request, filtre);

    public Task<HataLogDetayDto?> HataLogGetirAsync(int id) =>
        repository.HataLogGetirAsync(id);

    public Task<HataLogDetayDto?> HataKoduIleGetirAsync(string kod) =>
        repository.HataKoduIleGetirAsync(kod);

    public Task<bool> CozumIsaretleAsync(int id, bool cozuldu, string? not) =>
        repository.CozumIsaretleAsync(id, cozuldu, not, istekBaglami.Kullanici);

    public Task<(int Toplam, int Cozulmemis, int SonYirmiDortSaat)> HataOzetiAsync() =>
        repository.HataOzetiAsync();

    /// <summary>
    /// Yığın izi ve iç istisnalar. Asıl nedeni bulmak için zincirin tamamı
    /// yazılır; en dıştaki mesaj çoğu zaman en az bilgilendirici olandır.
    /// </summary>
    private static string AyrintiUret(Exception istisna)
    {
        var metin = new StringBuilder();
        var sira = 0;

        for (var mevcut = istisna; mevcut is not null; mevcut = mevcut.InnerException)
        {
            if (sira > 0)
                metin.AppendLine().AppendLine($"--- İç istisna {sira} ---");

            metin.AppendLine($"{mevcut.GetType().FullName}: {mevcut.Message}");

            if (!string.IsNullOrWhiteSpace(mevcut.StackTrace))
                metin.AppendLine(mevcut.StackTrace);

            sira++;
        }

        var sonuc = metin.ToString();

        return sonuc.Length <= AyrintiUzunlugu ? sonuc : sonuc[..AyrintiUzunlugu];
    }
}
