using AutoMapper;
using Business.Abstract;
using Core.Logging;
using Core.Security;
using Dto.ActiveDirectory;
using Dto.Kullanici;
using Dto.Kullanici.Enum;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repository.Context;
using System.Security.Cryptography;

namespace Business.Concrete;

public class KimlikDogrulamaManager(
    VarlikEnvanteriDbContext context,
    ISifreKoruyucu sifreKoruyucu,
    IActiveDirectoryService activeDirectoryService,
    IActiveDirectoryAyarService activeDirectoryAyarService,
    IOptions<SifrePolitikasi> sifrePolitikasi,
    ILogService logService,
    IMapper mapper) : IKimlikDogrulamaService
{
    private readonly SifrePolitikasi _politika = sifrePolitikasi.Value;

    // Hangi bilginin yanlış olduğunu sızdırmamak için kullanıcı adı ve şifre
    // hatalarında aynı metin döner.
    private const string GenelHataMesaji = "Kullanıcı adı veya şifre hatalı.";

    public async Task<GirisSonucu> GirisYapAsync(string username, string sifre)
    {
        var kullanici = await context.Kullanicilar
            .Include(k => k.KullaniciRoller)
            .ThenInclude(kr => kr.Rol)
            .FirstOrDefaultAsync(k => k.Username == username);

        if (kullanici is null)
        {
            // Kullanıcı yoksa olay hiçbir kiracıya bağlanamaz; tek organizasyonlu
            // kurulumlarda kayıt görünmez kalmasın diye o organizasyona yazılır.
            logService.OturumOlayiEkle(
                OturumOlaylari.Giris, username, await TekOrganizasyonAsync(), false, "Kullanıcı bulunamadı.");

            return new GirisSonucu { Durum = GirisDurumu.HataliKimlik, Mesaj = GenelHataMesaji };
        }

        if (kullanici.KilitBitisTarihi is { } kilit && kilit > DateTime.Now)
        {
            var kalan = (int)Math.Ceiling((kilit - DateTime.Now).TotalMinutes);

            return GirisiKaydet(kullanici, new GirisSonucu
            {
                Durum = GirisDurumu.Kilitli,
                Mesaj = $"Hesabınız çok sayıda hatalı denemeden dolayı kilitlendi. " +
                        $"Lütfen {kalan} dakika sonra tekrar deneyin."
            });
        }

        if (!kullanici.Durum)
            return GirisiKaydet(kullanici, new GirisSonucu
            {
                Durum = GirisDurumu.Pasif,
                Mesaj = "Hesabınız pasif durumda. Lütfen sistem yöneticinizle iletişime geçin."
            });

        // Kimlik, kullanıcının giriş yöntemine göre ya yerel karmayla ya da
        // dizine bağlanarak doğrulanır. İkisinin de ardından gelen roller,
        // birim ve oturum kuralları ortaktır.
        var dogrulama = kullanici.GirisYontemi == GirisYontemi.ActiveDirectory
            ? await DizinUzerindenDogrulaAsync(kullanici, sifre)
            : YerelOlarakDogrula(kullanici, sifre);

        if (dogrulama is not null)
        {
            // Sayaç yalnızca kimlik hatalarında işler: dizin tarafındaki hesap
            // kısıtları ya da erişilemeyen sunucu, kullanıcının denemesinin
            // hatalı olduğu anlamına gelmez.
            if (dogrulama.Durum == GirisDurumu.HataliKimlik)
                await BasarisizDenemeKaydetAsync(kullanici);

            return GirisiKaydet(kullanici, dogrulama);
        }

        var roller = kullanici.KullaniciRoller
            .Where(kr => kr.Durum && kr.Rol.Durum)
            .Select(kr => kr.Rol.Ad)
            .Distinct()
            .ToList();

        if (roller.Count == 0)
            return GirisiKaydet(kullanici, new GirisSonucu
            {
                Durum = GirisDurumu.RolTanimlanmamis,
                Mesaj = "Kullanıcınıza rol tanımlaması yapılmalıdır. Lütfen sistem yöneticinizle iletişime geçin."
            });

        kullanici.BasarisizGirisSayisi = 0;
        kullanici.KilitBitisTarihi = null;
        kullanici.SonGirisTarihi = DateTime.Now;
        kullanici.SecurityStamp ??= YeniDamga();

        await context.SaveChangesAsync();

        return GirisiKaydet(kullanici, new GirisSonucu
        {
            Durum = GirisDurumu.Basarili,
            Kullanici = mapper.Map<ListKullaniciDto>(kullanici),
            Roller = roller,
            // Dizine bağlı hesapların şifresi uygulamada tutulmadığı için
            // uygulama içi şifre değiştirme akışı çalıştırılmaz.
            SifreDegistirmeliMi = kullanici.GirisYontemi == GirisYontemi.Yerel && kullanici.SifreDegistirmeliMi,
            SecurityStamp = kullanici.SecurityStamp
        });
    }

    /// <summary>
    /// Giriş denemesinin sonucunu işlem loguna yazar ve sonucu olduğu gibi
    /// döndürür; böylece her çıkış noktası tek satırla kayıt altına alınır.
    ///
    /// Kullanıcıya gösterilen mesaj değil, denemenin gerçek sonucu yazılır:
    /// giriş ekranı hangi bilginin yanlış olduğunu gizler, denetim izi gizlememelidir.
    /// </summary>
    private GirisSonucu GirisiKaydet(Kullanici kullanici, GirisSonucu sonuc)
    {
        var basarili = sonuc.Durum == GirisDurumu.Basarili;

        var aciklama = sonuc.Durum switch
        {
            GirisDurumu.Basarili => kullanici.GirisYontemi == GirisYontemi.ActiveDirectory
                ? "Active Directory üzerinden giriş yapıldı."
                : "Yerel şifreyle giriş yapıldı.",
            GirisDurumu.HataliKimlik => "Şifre hatalı.",
            GirisDurumu.Kilitli => "Hesap kilitli.",
            GirisDurumu.Pasif => "Hesap pasif.",
            GirisDurumu.SifreBelirlenmemis => "Hesaba şifre tanımlanmamış.",
            GirisDurumu.RolTanimlanmamis => "Kullanıcının rolü yok.",
            GirisDurumu.DizinYapilandirilmamis => "Dizin bağlantısı yapılandırılmamış veya erişilemiyor.",
            GirisDurumu.DizinErisimHatasi => "Dizin girişe izin vermedi.",
            _ => sonuc.Durum.ToString()
        };

        logService.OturumOlayiEkle(
            OturumOlaylari.Giris, kullanici.Username, kullanici.OrganizasyonId, basarili, aciklama);

        return sonuc;
    }

    /// <summary>
    /// Kiracısı belirlenemeyen oturum olayları için son çare. Kurulumda tek
    /// organizasyon varsa kayıt ona bağlanır; birden fazlaysa sahipsiz bırakılır
    /// ve yalnızca kurumlar arası yetkili görür.
    /// </summary>
    private async Task<int> TekOrganizasyonAsync()
    {
        var organizasyonlar = await context.Organizasyonlar
            .AsNoTracking()
            .Select(o => o.Id)
            .Take(2)
            .ToListAsync();

        return organizasyonlar.Count == 1 ? organizasyonlar[0] : 0;
    }

    /// <summary>
    /// Yerel şifre doğrulaması. Başarılıysa <c>null</c>, aksi halde çağırana
    /// döndürülecek hatayı verir.
    /// </summary>
    private GirisSonucu? YerelOlarakDogrula(Kullanici kullanici, string sifre)
    {
        if (string.IsNullOrEmpty(kullanici.PasswordHash))
            return new GirisSonucu
            {
                Durum = GirisDurumu.SifreBelirlenmemis,
                Mesaj = "Hesabınız için henüz şifre belirlenmemiş. Lütfen sistem yöneticinizle iletişime geçin."
            };

        var dogrulama = sifreKoruyucu.Dogrula(kullanici.PasswordHash, sifre);

        if (dogrulama == SifreDogrulamaSonucu.Basarisiz)
            return new GirisSonucu { Durum = GirisDurumu.HataliKimlik, Mesaj = GenelHataMesaji };

        // Karma eski parametrelerle üretilmişse, düz metin hâlâ elimizdeyken yenilenir.
        if (dogrulama == SifreDogrulamaSonucu.BasariliYenilenmeli)
            kullanici.PasswordHash = sifreKoruyucu.Karmala(sifre);

        return null;
    }

    /// <summary>
    /// Active Directory doğrulaması. Başarılıysa <c>null</c> döner ve —
    /// yapılandırma izin veriyorsa — profil alanları dizinden tazelenir.
    /// </summary>
    private async Task<GirisSonucu?> DizinUzerindenDogrulaAsync(Kullanici kullanici, string sifre)
    {
        var ayar = await activeDirectoryAyarService.BaglantiAyariGetirAsync(kullanici.OrganizasyonId);

        if (ayar is null || !ayar.Aktif || !ayar.Yapilandirilmis)
            return new GirisSonucu
            {
                Durum = GirisDurumu.DizinYapilandirilmamis,
                Mesaj = "Hesabınız Active Directory üzerinden doğrulanıyor ancak dizin bağlantısı " +
                        "yapılandırılmamış. Lütfen sistem yöneticinizle iletişime geçin."
            };

        var sonuc = await activeDirectoryService.DogrulaAsync(ayar, kullanici.DizinKullaniciAdi, sifre);

        switch (sonuc.Durum)
        {
            case AdDogrulamaDurumu.Basarili:
                break;

            case AdDogrulamaDurumu.HataliKimlik:
                return new GirisSonucu { Durum = GirisDurumu.HataliKimlik, Mesaj = GenelHataMesaji };

            case AdDogrulamaDurumu.HesapKullanilamaz:
            case AdDogrulamaDurumu.GrupUyeligiYok:
                // Kimlik doğru ama dizin girişe izin vermiyor. Yanlış şifre
                // sayacı işletilmez; sorun kullanıcının denemesinde değil.
                return new GirisSonucu
                {
                    Durum = GirisDurumu.DizinErisimHatasi,
                    Mesaj = "Hesabınız Active Directory üzerinde giriş için uygun durumda değil. " +
                            "Lütfen sistem yöneticinizle iletişime geçin."
                };

            default:
                return new GirisSonucu
                {
                    Durum = GirisDurumu.DizinYapilandirilmamis,
                    Mesaj = "Active Directory sunucusuna ulaşılamadı. Lütfen daha sonra tekrar deneyin."
                };
        }

        if (ayar.ProfilBilgileriniGuncelle && sonuc.Kullanici is { } dizinBilgisi)
        {
            if (!string.IsNullOrWhiteSpace(dizinBilgisi.AdSoyad))
                kullanici.AdSoyad = dizinBilgisi.AdSoyad;

            if (!string.IsNullOrWhiteSpace(dizinBilgisi.Eposta))
                kullanici.Eposta = dizinBilgisi.Eposta;
        }

        // Yöntem yerelden dizine çevrildiyse eski karma artık geçersizdir;
        // uygulamada dizin hesabına ait bir şifre kalmamalıdır.
        kullanici.PasswordHash = null;
        kullanici.SifreDegistirmeliMi = false;

        return null;
    }

    public async Task<(bool Basarili, IReadOnlyList<string> Hatalar, string? YeniSecurityStamp)> SifreDegistirAsync(
        int kullaniciId,
        string? mevcutSifre,
        string yeniSifre,
        bool mevcutSifreDogrulansin = true)
    {
        var kullanici = await context.Kullanicilar.FirstOrDefaultAsync(k => k.Id == kullaniciId);

        if (kullanici is null)
            return (false, new[] { "Kullanıcı bulunamadı." }, null);

        if (kullanici.GirisYontemi == GirisYontemi.ActiveDirectory)
        {
            SifreOlayiKaydet(OturumOlaylari.SifreDegistir, kullanici, false, "Şifre Active Directory üzerinde yönetiliyor.");

            return (false, new[]
            {
                "Şifreniz Active Directory üzerinde yönetiliyor. " +
                "Değiştirmek için kurumsal oturum açma araçlarınızı kullanın."
            }, null);
        }

        if (mevcutSifreDogrulansin &&
            sifreKoruyucu.Dogrula(kullanici.PasswordHash, mevcutSifre ?? "") == SifreDogrulamaSonucu.Basarisiz)
        {
            SifreOlayiKaydet(OturumOlaylari.SifreDegistir, kullanici, false, "Mevcut şifre hatalı.");
            return (false, new[] { "Mevcut şifre hatalı." }, null);
        }

        var politikaHatalari = _politika.Dogrula(yeniSifre);
        if (politikaHatalari.Count > 0)
            return (false, politikaHatalari, null);

        if (sifreKoruyucu.Dogrula(kullanici.PasswordHash, yeniSifre) != SifreDogrulamaSonucu.Basarisiz)
            return (false, new[] { "Yeni şifre mevcut şifreyle aynı olamaz." }, null);

        kullanici.PasswordHash = sifreKoruyucu.Karmala(yeniSifre);
        kullanici.SecurityStamp = YeniDamga();
        kullanici.SifreDegistirmeliMi = false;
        kullanici.BasarisizGirisSayisi = 0;
        kullanici.KilitBitisTarihi = null;

        await context.SaveChangesAsync();

        SifreOlayiKaydet(OturumOlaylari.SifreDegistir, kullanici, true, "Şifre değiştirildi.");

        return (true, [], kullanici.SecurityStamp);
    }

    public async Task<bool> SecurityStampGecerliMiAsync(int kullaniciId, string? securityStamp)
    {
        if (string.IsNullOrEmpty(securityStamp))
            return false;

        var kayitli = await context.Kullanicilar
            .Where(k => k.Id == kullaniciId && k.Durum)
            .Select(k => k.SecurityStamp)
            .FirstOrDefaultAsync();

        return kayitli is not null && kayitli == securityStamp;
    }

    public async Task<SifreSifirlamaSonucu> SifreSifirlaAsync(int kullaniciId)
    {
        var kullanici = await context.Kullanicilar.FirstOrDefaultAsync(k => k.Id == kullaniciId);

        if (kullanici is null)
            return SifreSifirlamaSonucu.Basarisiz("Kullanıcı bulunamadı.");

        // Dizine bağlı hesabın şifresi uygulamada tutulmadığı için sıfırlanamaz;
        // bu işlem Active Directory tarafında yapılmalıdır.
        if (kullanici.GirisYontemi == GirisYontemi.ActiveDirectory)
        {
            SifreOlayiKaydet(OturumOlaylari.SifreSifirla, kullanici, false, "Dizine bağlı hesabın şifresi buradan sıfırlanamaz.");

            return SifreSifirlamaSonucu.Basarisiz(
                "Bu hesabın şifresi Active Directory üzerinde yönetiliyor ve buradan sıfırlanamaz.");
        }

        var yeniSifre = GeciciSifreUret();

        kullanici.PasswordHash = sifreKoruyucu.Karmala(yeniSifre);
        kullanici.SifreDegistirmeliMi = true;

        // Damga yenilenince kullanıcının açık oturumları düşer.
        kullanici.SecurityStamp = YeniDamga();

        kullanici.BasarisizGirisSayisi = 0;
        kullanici.KilitBitisTarihi = null;

        await context.SaveChangesAsync();

        // Sıfırlamayı yapan yöneticinin adı istek bağlamından okunur; kayıt
        // hangi hesabın sıfırlandığını taşır.
        SifreOlayiKaydet(OturumOlaylari.SifreSifirla, kullanici, true, "Yönetici tek kullanımlık şifre üretti.");

        return SifreSifirlamaSonucu.Olustu(yeniSifre);
    }

    /// <summary>
    /// Şifre olaylarını işlem loguna yazar. Ne mevcut ne de yeni şifre kayda
    /// geçer; yalnızca hangi hesapta ne olduğu yazılır.
    /// </summary>
    private void SifreOlayiKaydet(string islem, Kullanici kullanici, bool basarili, string aciklama) =>
        logService.OturumOlayiEkle(islem, kullanici.Username, kullanici.OrganizasyonId, basarili, aciklama);

    /// <summary>
    /// Şifre politikasının her maddesini karşılayan, kriptografik olarak
    /// güvenli rastgele bir değer üretir.
    /// </summary>
    private string GeciciSifreUret()
    {
        const string buyuk = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        const string kucuk = "abcdefghijkmnopqrstuvwxyz";
        const string rakam = "23456789";
        const string ozel = "!@#$%*?";
        const string tumu = buyuk + kucuk + rakam + ozel;

        var uzunluk = Math.Max(_politika.EnAzUzunluk, 12);

        var karakterler = new List<char> { Sec(buyuk), Sec(kucuk), Sec(rakam), Sec(ozel) };

        while (karakterler.Count < uzunluk)
            karakterler.Add(Sec(tumu));

        // Zorunlu karakterlerin baştaki sabit sırasını bozmak için karıştırılır.
        for (var i = karakterler.Count - 1; i > 0; i--)
        {
            var j = RandomNumberGenerator.GetInt32(i + 1);
            (karakterler[i], karakterler[j]) = (karakterler[j], karakterler[i]);
        }

        return new string(karakterler.ToArray());

        static char Sec(string kaynak) => kaynak[RandomNumberGenerator.GetInt32(kaynak.Length)];
    }

    private async Task BasarisizDenemeKaydetAsync(Kullanici kullanici)
    {
        kullanici.BasarisizGirisSayisi++;

        if (_politika.KilitEsigi > 0 && kullanici.BasarisizGirisSayisi >= _politika.KilitEsigi)
        {
            kullanici.KilitBitisTarihi = DateTime.Now.AddMinutes(_politika.KilitSuresiDk);
            kullanici.BasarisizGirisSayisi = 0;
        }

        await context.SaveChangesAsync();
    }

    private static string YeniDamga() => Guid.NewGuid().ToString("N");
}
