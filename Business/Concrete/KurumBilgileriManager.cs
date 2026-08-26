using Business.Abstract;
using Core.Aspects;
using Core.Configuration;
using Core.Exceptions;
using Core.Logging;
using Dto.Organizasyon;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repository.Context;

namespace Business.Concrete;

/// <summary>
/// Kurulumun kendi kurum kaydını okur ve günceller.
///
/// <c>Organizasyon</c> kiracının kendisidir, bu yüzden kiracı sorgu filtresine
/// tabi değildir; kayıt her zaman oturumun organizasyon kimliğiyle açıkça
/// aranır.
/// </summary>
public class KurumBilgileriManager(
    VarlikEnvanteriDbContext context,
    IIstekBaglami istekBaglami,
    IOptions<UygulamaAyarlari> uygulamaAyarlari,
    ILogger<KurumBilgileriManager> logger) : IKurumBilgileriService
{
    private readonly UygulamaAyarlari _ayarlar = uygulamaAyarlari.Value;

    /// <summary>Görünen kimlik istek başına bir kez okunur; her sayfa çiziminde sorgu atılmaz.</summary>
    private KurumKimligiDto? _kimlik;

    public async Task<KurumBilgileriDto> GetirAsync()
    {
        var organizasyonId = OrganizasyonIdCoz();

        var kayit = await context.Organizasyonlar
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == organizasyonId)
            ?? throw new NotFoundException("Kurum kaydı bulunamadı.");

        return new KurumBilgileriDto
        {
            Id = kayit.Id,
            Ad = kayit.Ad,
            Kod = kayit.Kod,
            LogoUrl = kayit.LogoUrl
        };
    }

    [LogAspect]
    public async Task GuncelleAsync(KurumBilgileriDto dto)
    {
        var organizasyonId = OrganizasyonIdCoz();

        var kayit = await context.Organizasyonlar.FirstOrDefaultAsync(o => o.Id == organizasyonId)
                    ?? throw new NotFoundException("Kurum kaydı bulunamadı.");

        kayit.Ad = dto.Ad.Trim();
        kayit.Kod = Kirp(dto.Kod);
        kayit.LogoUrl = Kirp(dto.LogoUrl);
        kayit.UpdatedDate = DateTime.Now;

        await context.SaveChangesAsync();

        // Aynı istekte önbelleğe alınmış kimlik artık eskidir.
        _kimlik = null;
    }

    public async Task<KurumKimligiDto> GorunenKimlikAsync()
    {
        if (_kimlik is not null)
            return _kimlik;

        _kimlik = await KimlikOkuAsync();
        return _kimlik;
    }

    private async Task<KurumKimligiDto> KimlikOkuAsync()
    {
        var varsayilan = new KurumKimligiDto
        {
            Ad = _ayarlar.UygulamaAdi,
            LogoYolu = _ayarlar.LogoYolu
        };

        try
        {
            // Giriş ekranında oturum yoktur; tek organizasyonlu kurulumlarda
            // kurum adı yine de gösterilebilsin diye o kayda düşülür.
            var organizasyonId = istekBaglami.OrganizasyonId;

            var kayit = organizasyonId > 0
                ? await context.Organizasyonlar.AsNoTracking()
                    .FirstOrDefaultAsync(o => o.Id == organizasyonId)
                : await TekOrganizasyonAsync();

            if (kayit is null)
                return varsayilan;

            return new KurumKimligiDto
            {
                Ad = string.IsNullOrWhiteSpace(kayit.Ad) ? varsayilan.Ad : kayit.Ad,
                LogoYolu = string.IsNullOrWhiteSpace(kayit.LogoUrl) ? varsayilan.LogoYolu : kayit.LogoUrl
            };
        }
        catch (Exception ex)
        {
            // Bu çağrı sayfa düzeninden gelir. Veritabanına ulaşılamıyorsa
            // başlığın yanlış olması, sayfanın hiç çizilememesinden iyidir —
            // hata sayfası da bu düzeni kullanır.
            logger.LogWarning(ex, "Kurum kimliği okunamadı; uygulama ayarlarına düşüldü.");
            return varsayilan;
        }
    }

    private async Task<Entity.Concrete.Organizasyon?> TekOrganizasyonAsync()
    {
        var kayitlar = await context.Organizasyonlar
            .AsNoTracking()
            .Take(2)
            .ToListAsync();

        return kayitlar.Count == 1 ? kayitlar[0] : null;
    }

    private int OrganizasyonIdCoz()
    {
        var organizasyonId = istekBaglami.OrganizasyonId;

        if (organizasyonId > 0)
            return organizasyonId;

        throw new InvalidOperationException(
            "Kurum bilgileri için aktif organizasyon belirlenemedi. Oturumun organizasyon bilgisi eksik.");
    }

    private static string? Kirp(string? deger) =>
        string.IsNullOrWhiteSpace(deger) ? null : deger.Trim();
}
