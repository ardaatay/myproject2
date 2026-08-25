using Core.Security;
using Microsoft.AspNetCore.Authorization;

namespace Web.Extensions;

public static class YetkilendirmeExtensions
{
    /// <summary>
    /// <see cref="Yetkiler"/> içindeki her politikayı, konfigürasyondan gelen
    /// rol listesiyle kaydeder.
    /// </summary>
    public static IServiceCollection AddYetkilendirmeExt(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var bolum = configuration.GetSection(YetkiAyarlari.BolumAdi);
        var ayarlar = bolum.Get<YetkiAyarlari>() ?? new YetkiAyarlari();

        services.Configure<YetkiAyarlari>(bolum);

        services.AddAuthorization(options =>
        {
            foreach (var politika in Yetkiler.Tumu)
            {
                var roller = ayarlar.RolleriGetir(politika);

                options.AddPolicy(politika, kural =>
                {
                    if (roller.Count == 0)
                    {
                        // Rolü olmayan bir politika kimseye açılmaz.
                        kural.RequireAssertion(_ => false);
                        return;
                    }

                    kural.RequireRole(roller);
                });
            }
        });

        return services;
    }

    /// <summary>
    /// Görünümlerde menü ve buton görünürlüğü için kısa yol.
    /// </summary>
    public static async Task<bool> YetkiliMi(
        this IAuthorizationService authorizationService,
        System.Security.Claims.ClaimsPrincipal kullanici,
        string politika)
    {
        var sonuc = await authorizationService.AuthorizeAsync(kullanici, politika);
        return sonuc.Succeeded;
    }
}
