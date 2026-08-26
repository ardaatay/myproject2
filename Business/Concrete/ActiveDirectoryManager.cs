using System.DirectoryServices.Protocols;
using System.Net;
using System.Text;
using Business.Abstract;
using Dto.ActiveDirectory;
using Microsoft.Extensions.Logging;

namespace Business.Concrete;

/// <summary>
/// LDAP üzerinden Active Directory doğrulaması.
///
/// <c>System.DirectoryServices.AccountManagement</c> yerine
/// <c>System.DirectoryServices.Protocols</c> kullanılır: uygulama konteynerde
/// Linux üzerinde de çalıştığı için Windows'a bağlı API'ler kullanılamaz.
/// </summary>
public class ActiveDirectoryManager(ILogger<ActiveDirectoryManager> logger) : IActiveDirectoryService
{
    /// <summary>
    /// AD'nin iç içe grup üyeliğini de kapsayan eşleştirme kuralı. Doğrudan
    /// <c>memberOf</c> karşılaştırması yalnızca birinci seviye üyeliği görür.
    /// </summary>
    private const string ZincirliUyelikKurali = "1.2.840.113556.1.4.1941";

    /// <summary>
    /// LDAP "invalidCredentials" sonuç kodu (RFC 4511). Hem Windows hem de
    /// OpenLDAP arka ucu bu değeri döndürür.
    /// </summary>
    private const int GecersizKimlikKodu = 49;

    public Task<AdDogrulamaSonucu> DogrulaAsync(
        ActiveDirectoryBaglantiAyari ayar,
        string kullaniciAdi,
        string sifre,
        CancellationToken cancellationToken = default)
    {
        // LDAP istemcisinin eşzamansız karşılığı yok; çağrı havuz iş parçacığına
        // taşınarak istek iş parçacığı bloke edilmez.
        return Task.Run(() => Dogrula(ayar, kullaniciAdi, sifre), cancellationToken);
    }

    public Task<ActiveDirectoryTestSonucu> BaglantiTestEtAsync(
        ActiveDirectoryBaglantiAyari ayar,
        string? testKullaniciAdi,
        string? testSifre,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(() => BaglantiTestEt(ayar, testKullaniciAdi, testSifre), cancellationToken);
    }

    private AdDogrulamaSonucu Dogrula(ActiveDirectoryBaglantiAyari ayar, string kullaniciAdi, string sifre)
    {
        if (!ayar.Yapilandirilmis)
            return AdDogrulamaSonucu.Hata(AdDogrulamaDurumu.SunucuHatasi, "Dizin ayarları eksik.");

        // Boş şifreyle simple bind, dizin tarafında anonim bağlanma sayılıp
        // başarıyla dönebilir. Bu, şifresiz girişe kapı açar.
        if (string.IsNullOrEmpty(sifre))
            return AdDogrulamaSonucu.Hata(AdDogrulamaDurumu.HataliKimlik, "Şifre boş olamaz.");

        var baglanmaAdi = BaglanmaAdiUret(ayar, kullaniciAdi);

        try
        {
            using var baglanti = BaglantiAc(ayar);
            baglanti.Bind(new NetworkCredential(baglanmaAdi, sifre));

            // Kimlik doğru. Bundan sonrası profil bilgisi ve grup denetimi.
            return KullaniciyiIncele(ayar, kullaniciAdi, baglanti);
        }
        catch (LdapException ex) when (ex.ErrorCode == GecersizKimlikKodu)
        {
            return KimlikHatasiniYorumla(ex);
        }
        catch (LdapException ex)
        {
            logger.LogWarning(ex, "Dizin bağlantısı kurulamadı. Sunucu: {Sunucu}", ayar.Sunucu);
            return AdDogrulamaSonucu.Hata(AdDogrulamaDurumu.SunucuHatasi, LdapHataMetni(ex));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Dizin doğrulaması beklenmedik biçimde başarısız oldu.");
            return AdDogrulamaSonucu.Hata(AdDogrulamaDurumu.SunucuHatasi, "Dizin sunucusuna ulaşılamadı.");
        }
    }

    /// <summary>
    /// Kimlik doğrulandıktan sonraki adımlar: profil alanlarının okunması ve
    /// zorunlu grup üyeliğinin denetlenmesi.
    /// </summary>
    private AdDogrulamaSonucu KullaniciyiIncele(
        ActiveDirectoryBaglantiAyari ayar,
        string kullaniciAdi,
        LdapConnection kullaniciBaglantisi)
    {
        var grupDenetimiGerekli = !string.IsNullOrWhiteSpace(ayar.ZorunluGrupDn);
        var aramaGerekli = grupDenetimiGerekli || ayar.ProfilBilgileriniGuncelle;

        var temelBilgi = new AdKullaniciBilgisi { KullaniciAdi = kullaniciAdi };

        if (!aramaGerekli || string.IsNullOrWhiteSpace(ayar.TabanDn))
        {
            // Grup zorunluysa ve aranacak taban yoksa üyelik doğrulanamaz.
            if (grupDenetimiGerekli)
            {
                return AdDogrulamaSonucu.Hata(
                    AdDogrulamaDurumu.SunucuHatasi,
                    "Zorunlu grup tanımlı ancak taban DN girilmediği için üyelik doğrulanamıyor.");
            }

            return new AdDogrulamaSonucu { Durum = AdDogrulamaDurumu.Basarili, Kullanici = temelBilgi };
        }

        LdapConnection? servisBaglantisi = null;

        try
        {
            // Arama tercihen servis hesabıyla yapılır: kullanıcının kendi
            // haklarının dizini okumaya yetmediği kurulumlar yaygındır.
            var aramaBaglantisi = kullaniciBaglantisi;

            if (ayar.ServisHesabiVar)
            {
                servisBaglantisi = BaglantiAc(ayar);
                servisBaglantisi.Bind(new NetworkCredential(
                    BaglanmaAdiUret(ayar, ayar.ServisHesabi!), ayar.ServisHesabiSifresi));
                aramaBaglantisi = servisBaglantisi;
            }

            var bulunan = KullaniciAra(aramaBaglantisi, ayar, kullaniciAdi) ?? temelBilgi;

            if (grupDenetimiGerekli && !GrupUyesiMi(aramaBaglantisi, ayar, kullaniciAdi))
            {
                return AdDogrulamaSonucu.Hata(
                    AdDogrulamaDurumu.GrupUyeligiYok,
                    "Kullanıcı, giriş için zorunlu tutulan grubun üyesi değil.");
            }

            return new AdDogrulamaSonucu { Durum = AdDogrulamaDurumu.Basarili, Kullanici = bulunan };
        }
        catch (Exception ex)
        {
            // Grup zorunluysa arama hatası girişi engeller: doğrulanamayan bir
            // yetki, verilmiş sayılmaz.
            if (grupDenetimiGerekli)
            {
                logger.LogWarning(ex, "Zorunlu grup üyeliği doğrulanamadı. Kullanıcı: {Kullanici}", kullaniciAdi);
                return AdDogrulamaSonucu.Hata(
                    AdDogrulamaDurumu.SunucuHatasi,
                    "Grup üyeliği doğrulanamadığı için giriş reddedildi.");
            }

            // Yalnızca profil tazeleme başarısızsa giriş engellenmez; kimlik
            // zaten doğrulanmıştır.
            logger.LogWarning(ex, "Dizinden profil bilgileri okunamadı. Kullanıcı: {Kullanici}", kullaniciAdi);
            return new AdDogrulamaSonucu { Durum = AdDogrulamaDurumu.Basarili, Kullanici = temelBilgi };
        }
        finally
        {
            servisBaglantisi?.Dispose();
        }
    }

    private ActiveDirectoryTestSonucu BaglantiTestEt(
        ActiveDirectoryBaglantiAyari ayar,
        string? testKullaniciAdi,
        string? testSifre)
    {
        if (!ayar.Yapilandirilmis)
        {
            return new ActiveDirectoryTestSonucu
            {
                Mesaj = "Sunucu adresi ile alan adı veya NetBIOS adı girilmeden sınama yapılamaz."
            };
        }

        // Test kullanıcısı verildiyse gerçek giriş akışının aynısı çalıştırılır;
        // böylece filtre ve grup ayarları da sınanmış olur.
        if (!string.IsNullOrWhiteSpace(testKullaniciAdi) && !string.IsNullOrEmpty(testSifre))
        {
            var sonuc = Dogrula(ayar, testKullaniciAdi.Trim(), testSifre);

            return new ActiveDirectoryTestSonucu
            {
                Basarili = sonuc.Basarili,
                Kullanici = sonuc.Kullanici,
                Mesaj = sonuc.Basarili
                    ? "Bağlantı ve kimlik doğrulama başarılı. Dizinde bulunan ad: " +
                      (sonuc.Kullanici?.AdSoyad ?? sonuc.Kullanici?.KullaniciAdi ?? testKullaniciAdi)
                    : sonuc.Mesaj ?? "Kimlik doğrulanamadı."
            };
        }

        if (!ayar.ServisHesabiVar)
        {
            return new ActiveDirectoryTestSonucu
            {
                Mesaj = "Sınama için ya bir servis hesabı tanımlayın ya da test kullanıcı adı ve şifresi girin."
            };
        }

        try
        {
            using var baglanti = BaglantiAc(ayar);
            baglanti.Bind(new NetworkCredential(
                BaglanmaAdiUret(ayar, ayar.ServisHesabi!), ayar.ServisHesabiSifresi));

            if (string.IsNullOrWhiteSpace(ayar.TabanDn))
            {
                return new ActiveDirectoryTestSonucu
                {
                    Basarili = true,
                    Mesaj = "Servis hesabıyla bağlantı kuruldu. Taban DN girilmediği için arama sınanmadı."
                };
            }

            // Taban DN'in gerçekten okunabildiğini görmek için tek kayıtlık arama.
            var istek = new SearchRequest(ayar.TabanDn, "(objectClass=*)", SearchScope.Base, "distinguishedName");
            var yanit = (SearchResponse)baglanti.SendRequest(istek);

            return new ActiveDirectoryTestSonucu
            {
                Basarili = true,
                Mesaj = yanit.Entries.Count > 0
                    ? $"Servis hesabıyla bağlantı kuruldu ve taban DN okundu: {ayar.TabanDn}"
                    : "Servis hesabıyla bağlantı kuruldu ancak taban DN okunamadı."
            };
        }
        catch (LdapException ex) when (ex.ErrorCode == GecersizKimlikKodu)
        {
            return new ActiveDirectoryTestSonucu { Mesaj = "Servis hesabının kullanıcı adı veya şifresi hatalı." };
        }
        catch (LdapException ex)
        {
            logger.LogWarning(ex, "Dizin sınaması başarısız. Sunucu: {Sunucu}", ayar.Sunucu);
            return new ActiveDirectoryTestSonucu { Mesaj = LdapHataMetni(ex) };
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Dizin sınaması beklenmedik biçimde başarısız oldu.");
            return new ActiveDirectoryTestSonucu { Mesaj = $"Bağlantı kurulamadı: {ex.Message}" };
        }
    }

    private static LdapConnection BaglantiAc(ActiveDirectoryBaglantiAyari ayar)
    {
        var port = ayar.Port > 0
            ? ayar.Port
            : ayar.SslKullan ? ActiveDirectoryVarsayilan.SslPort : ActiveDirectoryVarsayilan.Port;

        var kimlik = new LdapDirectoryIdentifier(ayar.Sunucu, port, false, false);

        var baglanti = new LdapConnection(kimlik)
        {
            // Dizin, UPN ve ALAN\kullanici biçimlerini simple bind ile kabul eder.
            AuthType = AuthType.Basic,
            Timeout = TimeSpan.FromSeconds(ayar.ZamanAsimiSn > 0
                ? ayar.ZamanAsimiSn
                : ActiveDirectoryVarsayilan.ZamanAsimiSn)
        };

        baglanti.SessionOptions.ProtocolVersion = 3;

        // Yönlendirme takibi kapalı: takip edilen sunucuya kimlik taşınmadığı
        // için aramalar sessizce boş dönebilir.
        baglanti.SessionOptions.ReferralChasing = ReferralChasingOptions.None;

        if (ayar.SertifikaDogrulamasiAtla)
            baglanti.SessionOptions.VerifyServerCertificate = (_, _) => true;

        if (ayar.SslKullan)
            baglanti.SessionOptions.SecureSocketLayer = true;

        // Şifre düz metin gitmesin diye bağlanmadan önce TLS'e yükseltilir.
        if (ayar.StartTlsKullan)
            baglanti.SessionOptions.StartTransportLayerSecurity(null);

        return baglanti;
    }

    /// <summary>
    /// Kullanıcının yazdığı adı dizinin beklediği biçime tamamlar. Kullanıcı
    /// zaten <c>ad@alan</c> veya <c>ALAN\ad</c> yazdıysa dokunulmaz.
    /// </summary>
    private static string BaglanmaAdiUret(ActiveDirectoryBaglantiAyari ayar, string kullaniciAdi)
    {
        if (kullaniciAdi.Contains('@') || kullaniciAdi.Contains('\\'))
            return kullaniciAdi;

        if (!string.IsNullOrWhiteSpace(ayar.AlanAdi))
            return $"{kullaniciAdi}@{ayar.AlanAdi}";

        if (!string.IsNullOrWhiteSpace(ayar.NetBiosAdi))
            return $"{ayar.NetBiosAdi}\\{kullaniciAdi}";

        return kullaniciAdi;
    }

    private static AdKullaniciBilgisi? KullaniciAra(
        LdapConnection baglanti,
        ActiveDirectoryBaglantiAyari ayar,
        string kullaniciAdi)
    {
        var filtre = string.Format(ayar.KullaniciAramaFiltresi, FiltreKacir(kullaniciAdi));

        var istek = new SearchRequest(
            ayar.TabanDn,
            filtre,
            SearchScope.Subtree,
            ayar.KullaniciAdiOzniteligi,
            ayar.AdSoyadOzniteligi,
            ayar.EpostaOzniteligi,
            "distinguishedName");

        var yanit = (SearchResponse)baglanti.SendRequest(istek);

        if (yanit.Entries.Count == 0)
            return null;

        var kayit = yanit.Entries[0];

        return new AdKullaniciBilgisi
        {
            KullaniciAdi = OznitelikOku(kayit, ayar.KullaniciAdiOzniteligi) ?? kullaniciAdi,
            AdSoyad = OznitelikOku(kayit, ayar.AdSoyadOzniteligi),
            Eposta = OznitelikOku(kayit, ayar.EpostaOzniteligi),
            DistinguishedName = kayit.DistinguishedName
        };
    }

    private static bool GrupUyesiMi(
        LdapConnection baglanti,
        ActiveDirectoryBaglantiAyari ayar,
        string kullaniciAdi)
    {
        // Kullanıcı filtresi ile grup koşulu birleştirilir; iç içe gruplar da
        // kapsansın diye zincirli üyelik kuralı kullanılır.
        var kullaniciFiltresi = string.Format(ayar.KullaniciAramaFiltresi, FiltreKacir(kullaniciAdi));

        var filtre =
            $"(&{kullaniciFiltresi}(memberOf:{ZincirliUyelikKurali}:={FiltreKacir(ayar.ZorunluGrupDn!)}))";

        var istek = new SearchRequest(ayar.TabanDn, filtre, SearchScope.Subtree, "distinguishedName");
        var yanit = (SearchResponse)baglanti.SendRequest(istek);

        return yanit.Entries.Count > 0;
    }

    private static string? OznitelikOku(SearchResultEntry kayit, string oznitelik)
    {
        if (string.IsNullOrWhiteSpace(oznitelik) || !kayit.Attributes.Contains(oznitelik))
            return null;

        var deger = kayit.Attributes[oznitelik][0];

        var metin = deger switch
        {
            string s => s,
            byte[] b => Encoding.UTF8.GetString(b),
            _ => deger?.ToString()
        };

        return string.IsNullOrWhiteSpace(metin) ? null : metin;
    }

    /// <summary>RFC 4515 uyarınca filtre içinde özel anlam taşıyan karakterleri kaçırır.</summary>
    private static string FiltreKacir(string deger)
    {
        var sonuc = new StringBuilder(deger.Length);

        foreach (var karakter in deger)
        {
            switch (karakter)
            {
                case '\\': sonuc.Append("\\5c"); break;
                case '*': sonuc.Append("\\2a"); break;
                case '(': sonuc.Append("\\28"); break;
                case ')': sonuc.Append("\\29"); break;
                case '\0': sonuc.Append("\\00"); break;
                case '/': sonuc.Append("\\2f"); break;
                default: sonuc.Append(karakter); break;
            }
        }

        return sonuc.ToString();
    }

    /// <summary>
    /// Dizin, geçersiz kimlik hatasının gerçek nedenini uzatılmış mesajda
    /// "data 52e" gibi bir kodla bildirir. Kodu ayırt etmek, yöneticinin
    /// "şifre yanlış" ile "hesap kapalı" durumlarını ayırabilmesini sağlar.
    /// </summary>
    private static AdDogrulamaSonucu KimlikHatasiniYorumla(LdapException ex)
    {
        var mesaj = ex.ServerErrorMessage ?? string.Empty;

        var (durum, metin) = mesaj switch
        {
            _ when mesaj.Contains("data 525") => (AdDogrulamaDurumu.HataliKimlik, "Kullanıcı dizinde bulunamadı."),
            _ when mesaj.Contains("data 52e") => (AdDogrulamaDurumu.HataliKimlik, "Şifre hatalı."),
            _ when mesaj.Contains("data 530") => (AdDogrulamaDurumu.HesapKullanilamaz, "Hesabın bu saatte giriş izni yok."),
            _ when mesaj.Contains("data 531") => (AdDogrulamaDurumu.HesapKullanilamaz, "Hesabın bu bilgisayardan giriş izni yok."),
            _ when mesaj.Contains("data 532") => (AdDogrulamaDurumu.HesapKullanilamaz, "Dizindeki şifrenin süresi dolmuş."),
            _ when mesaj.Contains("data 533") => (AdDogrulamaDurumu.HesapKullanilamaz, "Hesap dizinde devre dışı."),
            _ when mesaj.Contains("data 701") => (AdDogrulamaDurumu.HesapKullanilamaz, "Hesabın süresi dolmuş."),
            _ when mesaj.Contains("data 773") => (AdDogrulamaDurumu.HesapKullanilamaz, "Kullanıcı dizindeki şifresini değiştirmelidir."),
            _ when mesaj.Contains("data 775") => (AdDogrulamaDurumu.HesapKullanilamaz, "Hesap dizinde kilitli."),
            _ => (AdDogrulamaDurumu.HataliKimlik, "Kullanıcı adı veya şifre hatalı.")
        };

        return AdDogrulamaSonucu.Hata(durum, metin);
    }

    private static string LdapHataMetni(LdapException ex) =>
        string.IsNullOrWhiteSpace(ex.ServerErrorMessage)
            ? $"Dizin sunucusuna ulaşılamadı ({ex.Message})."
            : $"Dizin sunucusu hatası: {ex.ServerErrorMessage}";
}
