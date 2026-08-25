using System.Security.Claims;
using Core.Security;

namespace Web.Services;

/// <summary>
/// Aktif kiracıyı oturum claim'inden okur.
///
/// Kurumlar arası yetkili (SUPERADMIN) oturumlarda ve kimlik doğrulanmamış
/// isteklerde <see cref="Id"/> null döner; bu durumda kiracı filtresi
/// uygulanmaz. Kimlik doğrulanmamış istekler zaten veriye erişemez, çünkü
/// tüm denetleyiciler yetkilendirme politikalarıyla korunur.
/// </summary>
public class AktifOrganizasyon(IHttpContextAccessor httpContextAccessor) : IAktifOrganizasyon
{
    public const string ClaimTuru = "OrganizasyonId";

    /// <summary>Bu role sahip kullanıcılar tüm kiracıların verisini görür.</summary>
    public const string KurumlarArasiRol = "SUPERADMIN";

    public int? Id
    {
        get
        {
            var kullanici = httpContextAccessor.HttpContext?.User;

            if (kullanici?.Identity?.IsAuthenticated != true)
                return null;

            if (kullanici.IsInRole(KurumlarArasiRol))
                return null;

            var deger = kullanici.FindFirst(ClaimTuru)?.Value;
            return int.TryParse(deger, out var id) ? id : null;
        }
    }
}
