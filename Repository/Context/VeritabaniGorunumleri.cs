using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Repository.Context;

/// <summary>
/// EF Core görünümleri (view) migration'larla yönetmez. Görünüm tanımları
/// <c>Repository/Sql/Views</c> altında kaynak kontrolünde tutulur, gömülü kaynak
/// olarak derlenir ve buradan uygulanır.
///
/// Her dosya <c>DROP VIEW ... CASCADE</c> ile başladığı için işlem yinelenebilir:
/// PostgreSQL'de <c>CREATE OR REPLACE VIEW</c> mevcut bir görünümün sütunlarını
/// değiştiremez, yalnızca sona ekleme yapabilir. Görünümler veri tutmadığından
/// düşürüp yeniden oluşturmanın maliyeti yoktur.
/// </summary>
public static class VeritabaniGorunumleri
{
    private const string KaynakOneki = ".Sql.Views.";

    public static async Task UygulaAsync(
        VarlikEnvanteriDbContext context,
        CancellationToken cancellationToken = default)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Dosya adlarındaki sayısal önek sırayı belirler: 00_fonksiyonlar.sql,
        // görünümlerin kullandığı yardımcı fonksiyonları tanımladığı için önce gelmelidir.
        var kaynaklar = assembly.GetManifestResourceNames()
            .Where(ad => ad.Contains(KaynakOneki, StringComparison.Ordinal)
                         && ad.EndsWith(".sql", StringComparison.Ordinal))
            .OrderBy(ad => ad, StringComparer.Ordinal)
            .ToList();

        if (kaynaklar.Count == 0)
            throw new InvalidOperationException(
                "Görünüm tanımları gömülü kaynak olarak bulunamadı. " +
                "Repository.csproj içindeki EmbeddedResource tanımını kontrol edin.");

        foreach (var kaynak in kaynaklar)
        {
            await using var stream = assembly.GetManifestResourceStream(kaynak)
                ?? throw new InvalidOperationException($"Gömülü kaynak okunamadı: {kaynak}");

            using var reader = new StreamReader(stream);
            var sql = await reader.ReadToEndAsync(cancellationToken);

            await context.Database.ExecuteSqlRawAsync(sql, cancellationToken);
        }
    }
}
