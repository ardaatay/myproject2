using Npgsql;

namespace Web.Extensions;

/// <summary>
/// Veritabanı bağlantı dizesini yapılandırmadan çözer.
/// <para>
/// Öncelik <c>ConnectionStrings:DefaultConnection</c>'dadır. O boşsa Railway,
/// Render, Heroku gibi barındırıcıların enjekte ettiği <c>DATABASE_URL</c>
/// (<c>postgresql://kullanici:sifre@sunucu:port/veritabani</c>) Npgsql'in
/// anahtar=değer biçimine çevrilir. Npgsql URL biçimini kabul etmediği için
/// bu çeviri olmadan barındırıcının verdiği değer doğrudan kullanılamaz.
/// </para>
/// </summary>
public static class VeritabaniBaglantisi
{
    public static string Coz(IConfiguration yapilandirma)
    {
        var dogrudan = yapilandirma.GetConnectionString("DefaultConnection");

        if (!string.IsNullOrWhiteSpace(dogrudan))
            return dogrudan;

        var url = yapilandirma["DATABASE_URL"];

        if (!string.IsNullOrWhiteSpace(url))
            return UrldenCevir(url);

        throw new InvalidOperationException(
            "Veritabanı bağlantısı tanımlı değil. ConnectionStrings__DefaultConnection " +
            "veya DATABASE_URL ortam değişkenlerinden birini ayarlayın.");
    }

    /// <summary>URL biçimini Npgsql bağlantı dizesine çevirir.</summary>
    public static string UrldenCevir(string url)
    {
        if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var adres))
            throw new InvalidOperationException("DATABASE_URL geçerli bir adres değil.");

        if (adres.Scheme is not ("postgres" or "postgresql"))
            throw new InvalidOperationException(
                $"DATABASE_URL desteklenmeyen bir şema taşıyor: '{adres.Scheme}'. " +
                "postgres:// veya postgresql:// bekleniyor.");

        var veritabani = adres.AbsolutePath.Trim('/');

        if (string.IsNullOrEmpty(veritabani))
            throw new InvalidOperationException("DATABASE_URL bir veritabanı adı içermiyor.");

        var kurucu = new NpgsqlConnectionStringBuilder
        {
            Host = adres.Host,
            // Uri, şema için varsayılan port tanımadığından IsDefaultPort her
            // zaman false olur; port yoksa Port -1 döner.
            Port = adres.Port > 0 ? adres.Port : 5432,
            Database = veritabani
        };

        // Kullanıcı adı ve şifre yüzde kodlamalı gelebilir.
        var kimlik = adres.UserInfo.Split(':', 2);

        if (kimlik.Length > 0 && kimlik[0].Length > 0)
            kurucu.Username = Uri.UnescapeDataString(kimlik[0]);

        if (kimlik.Length > 1)
            kurucu.Password = Uri.UnescapeDataString(kimlik[1]);

        SorguParametreleriniUygula(adres.Query, kurucu);

        return kurucu.ConnectionString;
    }

    /// <summary>
    /// <c>?sslmode=require</c> gibi parametreleri karşılık gelen Npgsql
    /// anahtarlarına aktarır. Tanınmayan parametreler sessizce yok sayılır.
    /// </summary>
    private static void SorguParametreleriniUygula(string sorgu, NpgsqlConnectionStringBuilder kurucu)
    {
        if (string.IsNullOrWhiteSpace(sorgu))
            return;

        foreach (var parca in sorgu.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var esit = parca.IndexOf('=');

            if (esit <= 0)
                continue;

            var anahtar = Uri.UnescapeDataString(parca[..esit]);
            var deger = Uri.UnescapeDataString(parca[(esit + 1)..]);

            switch (anahtar.ToLowerInvariant())
            {
                // Npgsql 8'den beri Require yalnızca şifreler, sertifika
                // zincirini doğrulamaz; doğrulama isteniyorsa VerifyCA veya
                // VerifyFull kullanılır. Bu yüzden ayrıca bir "güven" anahtarı
                // gerekmez.
                case "sslmode":
                    if (Enum.TryParse<SslMode>(deger.Replace("-", string.Empty), true, out var mod))
                        kurucu.SslMode = mod;
                    break;

                case "application_name":
                case "applicationname":
                    kurucu.ApplicationName = deger;
                    break;
            }
        }
    }
}
