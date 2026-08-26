using Core.Security;
using Dto.Durum.Enum;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Repository.Context;

/// <summary>
/// Temiz kurulumda sisteme ilk kez girilebilmesi için gereken en küçük veri
/// kümesi: roller ve bir yönetici hesabı. Var olan kayıtlara dokunulmaz,
/// dolayısıyla her açılışta güvenle çalıştırılabilir.
/// </summary>
public static class BaslangicVerisi
{
    public const string YoneticiRolu = "ADMIN";

    /// <summary>Kurumlar arası yetkili. Bu role sahip kullanıcılar kiracı filtresinden muaftır.</summary>
    public const string KurumlarArasiRol = "SUPERADMIN";

    private static readonly string[] VarsayilanRoller =
    [
        YoneticiRolu, KurumlarArasiRol,
        "BIGRADMINS", "BIGRUSERS", "BGYSADMINS", "BGYSUSERS", "OPOWNERS", "VERIGIRIS"
    ];

    /// <summary>
    /// Yönetici hesabı yoksa oluşturur ve üretilen geçici şifreyi döner.
    /// Hesap zaten varsa null döner ve hiçbir değişiklik yapılmaz.
    /// </summary>
    public static async Task<string?> UygulaAsync(
        VarlikEnvanteriDbContext context,
        ISifreKoruyucu sifreKoruyucu,
        string yoneticiKullaniciAdi,
        string? istenenSifre = null,
        string varsayilanBirimAdi = "Merkez",
        string varsayilanOrganizasyonAdi = "Varsayılan Organizasyon",
        CancellationToken cancellationToken = default)
    {
        // Kiracı, diğer her şeyin bağlanacağı kök kayıttır; önce o kurulur.
        var organizasyon = await context.Organizasyonlar.FirstOrDefaultAsync(cancellationToken);

        if (organizasyon is null)
        {
            organizasyon = new Organizasyon
            {
                Ad = varsayilanOrganizasyonAdi,
                Kod = "varsayilan",
                Durum = true,
                CreatedDate = DateTime.Now
            };

            context.Organizasyonlar.Add(organizasyon);
            await context.SaveChangesAsync(cancellationToken);
        }

        await DurumlariKurAsync(context, cancellationToken);

        var eklenenRoller = false;

        foreach (var rolAdi in VarsayilanRoller)
        {
            if (!await context.Roller.AnyAsync(r => r.Ad == rolAdi, cancellationToken))
            {
                context.Roller.Add(new Rol { Ad = rolAdi, Durum = true });
                eklenenRoller = true;
            }
        }

        if (eklenenRoller)
            await context.SaveChangesAsync(cancellationToken);

        // Birim seçimi zorunlu olduğu için en az bir kök birim bulunmalıdır;
        // aksi halde giriş yapan yönetici seçebileceği birim olmayan bir
        // seçim ekranına kilitlenir.
        var kokBirim = await context.Birimler.FirstOrDefaultAsync(b => b.UstId == null, cancellationToken);

        if (kokBirim is null)
        {
            kokBirim = new Birim
            {
                Ad = varsayilanBirimAdi,
                OrganizasyonId = organizasyon.Id,
                Seviye = 0,
                Sira = 0,
                Durum = true,
                CreatedDate = DateTime.Now,
                Yol = string.Empty
            };

            // Nested set numaraları kiracı içinde tekil olmalı: yeni kök,
            // organizasyonda hâlihazırda kullanılan en büyük sınırın ardına
            // yerleşir. Kök birimi olmayan ama artık kayıt taşıyan bir
            // organizasyonda da numaralar çakışmaz.
            var enSagSinir = await context.Birimler
                .IgnoreQueryFilters()
                .Where(b => b.OrganizasyonId == organizasyon.Id)
                .MaxAsync(b => (int?)b.Sag, cancellationToken) ?? 0;

            kokBirim.Sol = enSagSinir + 1;
            kokBirim.Sag = enSagSinir + 2;

            context.Birimler.Add(kokBirim);
            await context.SaveChangesAsync(cancellationToken);

            // Yol kendi kimliğini içerdiğinden ancak ekleme sonrası kesinleşir.
            kokBirim.Yol = $"/{kokBirim.Id}/";
            await context.SaveChangesAsync(cancellationToken);
        }

        if (await context.Kullanicilar.AnyAsync(cancellationToken))
            return null;

        // Şifre verilmediyse tek kullanımlık bir şifre üretilir ve günlüğe yazılır.
        // İlk girişte değiştirilmesi zorunludur.
        var sifre = string.IsNullOrWhiteSpace(istenenSifre) ? GeciciSifreUret() : istenenSifre;

        var yonetici = new Kullanici
        {
            Username = yoneticiKullaniciAdi,
            OrganizasyonId = organizasyon.Id,
            AdSoyad = "Sistem Yöneticisi",
            PasswordHash = sifreKoruyucu.Karmala(sifre),
            SecurityStamp = Guid.NewGuid().ToString("N"),
            SifreDegistirmeliMi = true,
            Durum = true,
            BirimId = kokBirim.Id,
            BirimAd = kokBirim.Ad
        };

        context.Kullanicilar.Add(yonetici);
        await context.SaveChangesAsync(cancellationToken);

        var adminRol = await context.Roller.FirstAsync(r => r.Ad == YoneticiRolu, cancellationToken);

        context.KullaniciRoller.Add(new KullaniciRol
        {
            KullaniciId = yonetici.Id,
            RolId = adminRol.Id,
            OrganizasyonId = organizasyon.Id,
            Durum = true
        });

        // Birim seçim ekranı bu tablodan beslenir.
        context.KullaniciBirimler.Add(new KullaniciBirim
        {
            KullaniciId = yonetici.Id,
            BirimId = kokBirim.Id,
            BirimAd = kokBirim.Ad,
            OrganizasyonId = organizasyon.Id,
            Durum = true
        });

        await context.SaveChangesAsync(cancellationToken);

        return sifre;
    }

    /// <summary>
    /// Durum listesi, diğer referans tablolarından farklı olarak keyfi değildir:
    /// kimlikleri <see cref="DurumEnum"/> ile koda gömülüdür ve iş kuralları
    /// bunlara dayanır (örneğin "Hurda/İmha" seçilince envanterden çıkış tarihi
    /// yazılır). Bu yüzden kimlikler açıkça verilir.
    ///
    /// Diğer referans listeleri (gizlilik, kategori, konum ve benzeri) kuruma
    /// göre değiştiği ve kodda karşılıkları olmadığı için tohumlanmaz.
    /// </summary>
    private static async Task DurumlariKurAsync(
        VarlikEnvanteriDbContext context,
        CancellationToken cancellationToken)
    {
        if (await context.Durumlar.AnyAsync(cancellationToken))
            return;

        await context.Database.ExecuteSqlRawAsync(
            """
            INSERT INTO durumlar (id, ad, durum) VALUES
                (1, 'Aktif Varlık',            true),
                (2, 'Pasif Varlık',            true),
                (3, 'Hurda / İmha',            true),
                (5, 'Yedek / Fazlalık / Depo', true),
                (6, 'Bağış',                   true)
            ON CONFLICT (id) DO NOTHING;

            -- Kimlikler elle verildiği için kimlik dizisi ileri sarılır;
            -- aksi halde sonraki ekleme mevcut bir kimlikle çakışır.
            SELECT setval(
                pg_get_serial_sequence('durumlar', 'id'),
                (SELECT COALESCE(MAX(id), 1) FROM durumlar));
            """,
            cancellationToken);
    }

    private static string GeciciSifreUret()
    {
        // Şifre politikasının her maddesini karşılayan, tahmin edilemez bir değer.
        const string buyuk = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        const string kucuk = "abcdefghijkmnopqrstuvwxyz";
        const string rakam = "23456789";
        const string ozel = "!@#$%*?";
        const string tumu = buyuk + kucuk + rakam + ozel;

        var karakterler = new List<char>
        {
            Sec(buyuk), Sec(kucuk), Sec(rakam), Sec(ozel)
        };

        while (karakterler.Count < 16)
            karakterler.Add(Sec(tumu));

        // Zorunlu karakterlerin baştaki sabit sırasını bozmak için karıştırılır.
        for (var i = karakterler.Count - 1; i > 0; i--)
        {
            var j = System.Security.Cryptography.RandomNumberGenerator.GetInt32(i + 1);
            (karakterler[i], karakterler[j]) = (karakterler[j], karakterler[i]);
        }

        return new string(karakterler.ToArray());

        static char Sec(string kaynak) =>
            kaynak[System.Security.Cryptography.RandomNumberGenerator.GetInt32(kaynak.Length)];
    }
}
