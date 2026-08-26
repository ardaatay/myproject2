using System.Security.Cryptography;
using Core.Security;
using Microsoft.AspNetCore.DataProtection;

namespace Web.Services;

/// <summary>
/// Gizli ayarları ASP.NET Core DataProtection ile korur.
///
/// Anahtarlar kalıcı bir dizine yazılmazsa (bkz. <c>UygulamaAyarlari.VeriDizini</c>)
/// uygulama her yeniden oluşturulduğunda değişir; o durumda korunmuş değerler
/// çözülemez ve yöneticinin yeniden girmesi gerekir. Bu, çözülemeyen değerin
/// <c>null</c> dönmesiyle bildirilir — istisna fırlatılmaz, çünkü bu bir
/// yapılandırma durumudur, sunucu hatası değil.
/// </summary>
public class DataProtectionGizliVeriKoruyucu : IGizliVeriKoruyucu
{
    /// <summary>
    /// Amaç dizesi, bu koruyucunun ürettiği değerlerin başka bir bağlamda
    /// çözülmesini engeller. Değiştirilirse eski kayıtlar okunamaz hale gelir.
    /// </summary>
    private const string Amac = "VarlikEnvanteri.GizliAyar.v1";

    private readonly IDataProtector _koruyucu;

    public DataProtectionGizliVeriKoruyucu(IDataProtectionProvider saglayici)
    {
        _koruyucu = saglayici.CreateProtector(Amac);
    }

    public string Koru(string acikMetin)
    {
        ArgumentNullException.ThrowIfNull(acikMetin);
        return _koruyucu.Protect(acikMetin);
    }

    public string? Coz(string? korunmusMetin)
    {
        if (string.IsNullOrEmpty(korunmusMetin))
            return null;

        try
        {
            return _koruyucu.Unprotect(korunmusMetin);
        }
        catch (CryptographicException)
        {
            // Anahtar yenilendi ya da değer bozuldu: çağıran bunu "yeniden
            // girilmeli" olarak yorumlar.
            return null;
        }
    }
}
