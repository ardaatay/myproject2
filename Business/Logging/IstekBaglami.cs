using System.Diagnostics;
using System.Security.Claims;
using Core.Logging;
using Core.Security;
using Microsoft.AspNetCore.Http;

namespace Business.Logging;

/// <summary>
/// Log alanlarını istekten okur.
///
/// Kiracı kimliği <see cref="IAktifOrganizasyon"/> yerine doğrudan claim'den
/// alınır: orası kurumlar arası yetkili oturumlarda bilinçli olarak null döner,
/// oysa log kaydının hangi kurumda oluştuğu her zaman yazılmalıdır.
/// </summary>
public class IstekBaglami(IHttpContextAccessor httpContextAccessor) : IIstekBaglami
{
    private const string Anonim = "Anonim";

    public int OrganizasyonId
    {
        get
        {
            var deger = Kullanicisi?.FindFirst(KiraciClaim.OrganizasyonId)?.Value;
            return int.TryParse(deger, out var id) ? id : 0;
        }
    }

    public string Kullanici =>
        Kullanicisi?.Identity?.IsAuthenticated == true
            ? Kullanicisi.FindFirst(ClaimTypes.Name)?.Value
              ?? Kullanicisi.FindFirst(ClaimTypes.NameIdentifier)?.Value
              ?? Anonim
            : Anonim;

    public string? IpAdresi => httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

    public string? Yol => httpContextAccessor.HttpContext?.Request.Path.Value;

    public string? HttpYontemi => httpContextAccessor.HttpContext?.Request.Method;

    public string? IstekId =>
        httpContextAccessor.HttpContext?.TraceIdentifier ?? Activity.Current?.Id;

    public string HataKodu()
    {
        var context = httpContextAccessor.HttpContext;

        // İstek dışında (arka plan işi, açılış) bağlam yoktur; kod yine üretilir
        // ama paylaşılamaz.
        if (context is null)
            return Core.Logging.HataKodu.Uret();

        if (context.Items.TryGetValue(Core.Logging.HataKodu.OgeAnahtari, out var mevcut) &&
            mevcut is string kod && !string.IsNullOrEmpty(kod))
        {
            return kod;
        }

        var yeni = Core.Logging.HataKodu.Uret();
        context.Items[Core.Logging.HataKodu.OgeAnahtari] = yeni;

        return yeni;
    }

    private ClaimsPrincipal? Kullanicisi => httpContextAccessor.HttpContext?.User;
}
