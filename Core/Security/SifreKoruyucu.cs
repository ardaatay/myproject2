using Microsoft.AspNetCore.Identity;

namespace Core.Security;

/// <summary>
/// ASP.NET Core Identity'nin <see cref="PasswordHasher{TUser}"/> sınıfını sarar.
/// Tam Identity yığını kurulmaz; yalnızca karma algoritması (PBKDF2-HMAC-SHA512,
/// rastgele tuz, sürüm etiketli biçim) ödünç alınır. Bu, iterasyon sayısı ve
/// sabit zamanlı karşılaştırma gibi ayrıntıları elle yazmaktan daha güvenlidir.
/// </summary>
public class SifreKoruyucu : ISifreKoruyucu
{
    // Tür parametresi yalnızca API'nin gerektirdiği yer tutucudur; kullanıcı
    // nesnesi karmaya dahil edilmez.
    private readonly PasswordHasher<object> _hasher = new();
    private static readonly object Yertutucu = new();

    public string Karmala(string sifre)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sifre);
        return _hasher.HashPassword(Yertutucu, sifre);
    }

    public SifreDogrulamaSonucu Dogrula(string? karma, string sifre)
    {
        if (string.IsNullOrEmpty(karma) || string.IsNullOrEmpty(sifre))
            return SifreDogrulamaSonucu.Basarisiz;

        // Bozuk/eski biçimli karmalar istisna fırlatabilir; bu bir kimlik
        // doğrulama başarısızlığıdır, sunucu hatası değil.
        PasswordVerificationResult sonuc;
        try
        {
            sonuc = _hasher.VerifyHashedPassword(Yertutucu, karma, sifre);
        }
        catch (FormatException)
        {
            return SifreDogrulamaSonucu.Basarisiz;
        }

        return sonuc switch
        {
            PasswordVerificationResult.Success => SifreDogrulamaSonucu.Basarili,
            PasswordVerificationResult.SuccessRehashNeeded => SifreDogrulamaSonucu.BasariliYenilenmeli,
            _ => SifreDogrulamaSonucu.Basarisiz
        };
    }
}
