using System.Text.Json;
using Business.Abstract;
using Core.Logging;
using Core.Security;
using Dto.ActiveDirectory;
using Dto.Kullanici.Enum;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Repository.Context;

namespace Business.Concrete;

/// <summary>
/// Kiracının dizin ayarlarını okur ve yazar.
///
/// Eşleme elle yapılır: servis hesabı şifresi hiçbir koşulda görünüm modeline
/// çözülmüş olarak taşınmamalıdır. Otomatik eşleme, ileride eklenecek bir alanla
/// bu kuralı sessizce bozabilir.
/// </summary>
public class ActiveDirectoryAyarManager(
    VarlikEnvanteriDbContext context,
    IGizliVeriKoruyucu gizliVeriKoruyucu,
    ILogService logService,
    IIstekBaglami istekBaglami) : IActiveDirectoryAyarService
{
    public async Task<ActiveDirectoryAyarDto> GetirAsync()
    {
        var organizasyonId = OrganizasyonIdCoz();
        var kayit = await KayitGetirAsync(organizasyonId);

        if (kayit is null)
            return new ActiveDirectoryAyarDto();

        var dto = new ActiveDirectoryAyarDto
        {
            Id = kayit.Id,
            Aktif = kayit.Aktif,
            Sunucu = kayit.Sunucu,
            Port = kayit.Port,
            SslKullan = kayit.SslKullan,
            StartTlsKullan = kayit.StartTlsKullan,
            SertifikaDogrulamasiAtla = kayit.SertifikaDogrulamasiAtla,
            AlanAdi = kayit.AlanAdi,
            NetBiosAdi = kayit.NetBiosAdi,
            TabanDn = kayit.TabanDn,
            ServisHesabi = kayit.ServisHesabi,
            KullaniciAramaFiltresi = kayit.KullaniciAramaFiltresi,
            KullaniciAdiOzniteligi = kayit.KullaniciAdiOzniteligi,
            AdSoyadOzniteligi = kayit.AdSoyadOzniteligi,
            EpostaOzniteligi = kayit.EpostaOzniteligi,
            ZorunluGrupDn = kayit.ZorunluGrupDn,
            ZamanAsimiSn = kayit.ZamanAsimiSn,
            ProfilBilgileriniGuncelle = kayit.ProfilBilgileriniGuncelle,
            ServisSifresiKayitli = !string.IsNullOrEmpty(kayit.ServisHesabiSifresiKorunmus)
        };

        // Anahtarlar yenilenmişse kayıtlı şifre artık açılamaz; yönetici bunu
        // ekranda görüp yeniden girebilmelidir.
        if (dto.ServisSifresiKayitli && gizliVeriKoruyucu.Coz(kayit.ServisHesabiSifresiKorunmus) is null)
            dto.ServisSifresiCozulemedi = true;

        return dto;
    }

    public async Task KaydetAsync(ActiveDirectoryAyarDto dto)
    {
        var organizasyonId = OrganizasyonIdCoz();
        var kayit = await KayitGetirAsync(organizasyonId);
        var yeni = kayit is null;

        kayit ??= new ActiveDirectoryAyari
        {
            OrganizasyonId = organizasyonId,
            CreatedDate = DateTime.Now
        };

        kayit.Aktif = dto.Aktif;
        kayit.Sunucu = Kirp(dto.Sunucu);
        kayit.Port = dto.Port > 0 ? dto.Port : VarsayilanPort(dto);
        kayit.SslKullan = dto.SslKullan;
        kayit.StartTlsKullan = dto.StartTlsKullan;
        kayit.SertifikaDogrulamasiAtla = dto.SertifikaDogrulamasiAtla;
        kayit.AlanAdi = Kirp(dto.AlanAdi);
        kayit.NetBiosAdi = Kirp(dto.NetBiosAdi);
        kayit.TabanDn = Kirp(dto.TabanDn);
        kayit.ServisHesabi = Kirp(dto.ServisHesabi);
        kayit.KullaniciAramaFiltresi = BosaVarsayilan(dto.KullaniciAramaFiltresi, ActiveDirectoryVarsayilan.AramaFiltresi);
        kayit.KullaniciAdiOzniteligi = BosaVarsayilan(dto.KullaniciAdiOzniteligi, ActiveDirectoryVarsayilan.KullaniciAdiOzniteligi);
        kayit.AdSoyadOzniteligi = BosaVarsayilan(dto.AdSoyadOzniteligi, ActiveDirectoryVarsayilan.AdSoyadOzniteligi);
        kayit.EpostaOzniteligi = BosaVarsayilan(dto.EpostaOzniteligi, ActiveDirectoryVarsayilan.EpostaOzniteligi);
        kayit.ZorunluGrupDn = Kirp(dto.ZorunluGrupDn);
        kayit.ZamanAsimiSn = dto.ZamanAsimiSn > 0 ? dto.ZamanAsimiSn : ActiveDirectoryVarsayilan.ZamanAsimiSn;
        kayit.ProfilBilgileriniGuncelle = dto.ProfilBilgileriniGuncelle;

        // Şifre alanı boş bırakıldıysa kayıtlı değere dokunulmaz; yönetici
        // ayarları değiştirmek için şifreyi yeniden yazmak zorunda kalmaz.
        if (!string.IsNullOrEmpty(dto.ServisHesabiSifresi))
            kayit.ServisHesabiSifresiKorunmus = gizliVeriKoruyucu.Koru(dto.ServisHesabiSifresi);

        // Servis hesabı tamamen kaldırıldıysa şifresinin saklanması anlamsız.
        if (string.IsNullOrEmpty(kayit.ServisHesabi))
            kayit.ServisHesabiSifresiKorunmus = null;

        if (yeni)
        {
            context.ActiveDirectoryAyarlari.Add(kayit);
        }
        else
        {
            kayit.UpdatedDate = DateTime.Now;
        }

        await context.SaveChangesAsync();

        DenetimIziBirak(kayit, yeni, sifreDegisti: !string.IsNullOrEmpty(dto.ServisHesabiSifresi));
    }

    /// <summary>
    /// Bu servis, servis hesabı şifresini taşıdığı için bilinçli olarak
    /// <c>LogAspect</c> ile sarılmaz — aksi halde şifre parametreleriyle
    /// birlikte loglanırdı. Denetim izi bu yüzden elle ve yalnızca güvenli
    /// alanlarla bırakılır.
    /// </summary>
    private void DenetimIziBirak(ActiveDirectoryAyari kayit, bool yeni, bool sifreDegisti)
    {
        var ozet = new
        {
            kayit.Aktif,
            kayit.Sunucu,
            kayit.Port,
            kayit.SslKullan,
            kayit.StartTlsKullan,
            kayit.SertifikaDogrulamasiAtla,
            kayit.AlanAdi,
            kayit.NetBiosAdi,
            kayit.TabanDn,
            kayit.ServisHesabi,
            ServisSifresiDegisti = sifreDegisti,
            kayit.ZorunluGrupDn,
            kayit.ProfilBilgileriniGuncelle
        };

        logService.Add(new Log
        {
            OrganizasyonId = kayit.OrganizasyonId,
            ClassName = nameof(ActiveDirectoryAyarManager),
            MethodName = yeni ? "AyarlariOlustur" : "AyarlariGuncelle",
            Parameters = JsonSerializer.Serialize(ozet),
            ExecutingTime = DateTime.Now,
            Username = istekBaglami.Kullanici,
            IpAdresi = istekBaglami.IpAdresi,
            Yol = istekBaglami.Yol,
            Basarili = true,
            ReturnValue = "Kaydedildi"
        });
    }

    public async Task<ActiveDirectoryBaglantiAyari?> BaglantiAyariGetirAsync(int organizasyonId)
    {
        var kayit = await KayitGetirAsync(organizasyonId);
        return kayit is null ? null : BaglantiAyarinaCevir(kayit);
    }

    public async Task<ActiveDirectoryBaglantiAyari> SinamaAyariUretAsync(ActiveDirectoryAyarDto dto)
    {
        var ayar = new ActiveDirectoryBaglantiAyari
        {
            // Sınama, ayarın etkin olup olmamasından bağımsız çalışır: yönetici
            // önce sınayıp sonra etkinleştirebilmelidir.
            Aktif = true,
            Sunucu = Kirp(dto.Sunucu) ?? string.Empty,
            Port = dto.Port > 0 ? dto.Port : VarsayilanPort(dto),
            SslKullan = dto.SslKullan,
            StartTlsKullan = dto.StartTlsKullan,
            SertifikaDogrulamasiAtla = dto.SertifikaDogrulamasiAtla,
            AlanAdi = Kirp(dto.AlanAdi),
            NetBiosAdi = Kirp(dto.NetBiosAdi),
            TabanDn = Kirp(dto.TabanDn),
            ServisHesabi = Kirp(dto.ServisHesabi),
            ServisHesabiSifresi = dto.ServisHesabiSifresi,
            KullaniciAramaFiltresi = BosaVarsayilan(dto.KullaniciAramaFiltresi, ActiveDirectoryVarsayilan.AramaFiltresi),
            KullaniciAdiOzniteligi = BosaVarsayilan(dto.KullaniciAdiOzniteligi, ActiveDirectoryVarsayilan.KullaniciAdiOzniteligi),
            AdSoyadOzniteligi = BosaVarsayilan(dto.AdSoyadOzniteligi, ActiveDirectoryVarsayilan.AdSoyadOzniteligi),
            EpostaOzniteligi = BosaVarsayilan(dto.EpostaOzniteligi, ActiveDirectoryVarsayilan.EpostaOzniteligi),
            ZorunluGrupDn = Kirp(dto.ZorunluGrupDn),
            ZamanAsimiSn = dto.ZamanAsimiSn > 0 ? dto.ZamanAsimiSn : ActiveDirectoryVarsayilan.ZamanAsimiSn,
            ProfilBilgileriniGuncelle = dto.ProfilBilgileriniGuncelle
        };

        // Formda şifre boş bırakıldıysa kayıtlı olanla sınanır.
        if (string.IsNullOrEmpty(ayar.ServisHesabiSifresi))
        {
            var kayit = await KayitGetirAsync(OrganizasyonIdCoz());
            ayar.ServisHesabiSifresi = gizliVeriKoruyucu.Coz(kayit?.ServisHesabiSifresiKorunmus);
        }

        return ayar;
    }

    public Task<int> DizinKullanicisiSayisiAsync()
    {
        var organizasyonId = OrganizasyonIdCoz();

        return context.Kullanicilar
            .IgnoreQueryFilters()
            .CountAsync(k => k.OrganizasyonId == organizasyonId &&
                             k.GirisYontemi == GirisYontemi.ActiveDirectory);
    }

    private ActiveDirectoryBaglantiAyari BaglantiAyarinaCevir(ActiveDirectoryAyari kayit) => new()
    {
        Aktif = kayit.Aktif,
        Sunucu = kayit.Sunucu ?? string.Empty,
        Port = kayit.Port,
        SslKullan = kayit.SslKullan,
        StartTlsKullan = kayit.StartTlsKullan,
        SertifikaDogrulamasiAtla = kayit.SertifikaDogrulamasiAtla,
        AlanAdi = kayit.AlanAdi,
        NetBiosAdi = kayit.NetBiosAdi,
        TabanDn = kayit.TabanDn,
        ServisHesabi = kayit.ServisHesabi,
        ServisHesabiSifresi = gizliVeriKoruyucu.Coz(kayit.ServisHesabiSifresiKorunmus),
        KullaniciAramaFiltresi = kayit.KullaniciAramaFiltresi,
        KullaniciAdiOzniteligi = kayit.KullaniciAdiOzniteligi,
        AdSoyadOzniteligi = kayit.AdSoyadOzniteligi,
        EpostaOzniteligi = kayit.EpostaOzniteligi,
        ZorunluGrupDn = kayit.ZorunluGrupDn,
        ZamanAsimiSn = kayit.ZamanAsimiSn,
        ProfilBilgileriniGuncelle = kayit.ProfilBilgileriniGuncelle
    };

    /// <summary>
    /// Kiracı kimliği açıkça verilir ve genel sorgu filtresi devre dışı bırakılır:
    /// bu kayıt, henüz oturum açılmamışken (giriş anında) de okunabilmelidir.
    /// </summary>
    private Task<ActiveDirectoryAyari?> KayitGetirAsync(int organizasyonId) =>
        context.ActiveDirectoryAyarlari
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(a => a.OrganizasyonId == organizasyonId);

    /// <summary>
    /// Ayarların ait olduğu kiracı. Değer oturum claim'inden okunur: kurumlar
    /// arası yetkili oturumlarda IAktifOrganizasyon bilinçli olarak null döner,
    /// oysa ayarın hangi kuruma yazıldığı her zaman belirli olmalıdır.
    /// </summary>
    private int OrganizasyonIdCoz()
    {
        var organizasyonId = istekBaglami.OrganizasyonId;

        if (organizasyonId > 0)
            return organizasyonId;

        throw new InvalidOperationException(
            "Dizin ayarları için aktif organizasyon belirlenemedi. Oturumun organizasyon bilgisi eksik.");
    }

    private static int VarsayilanPort(ActiveDirectoryAyarDto dto) =>
        dto.SslKullan ? ActiveDirectoryVarsayilan.SslPort : ActiveDirectoryVarsayilan.Port;

    private static string? Kirp(string? deger) =>
        string.IsNullOrWhiteSpace(deger) ? null : deger.Trim();

    private static string BosaVarsayilan(string? deger, string varsayilan) =>
        string.IsNullOrWhiteSpace(deger) ? varsayilan : deger.Trim();
}
