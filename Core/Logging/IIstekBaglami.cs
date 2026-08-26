namespace Core.Logging;

/// <summary>
/// Log kayıtlarının ortak alanlarını tek yerden verir. Hem iş katmanındaki
/// işlem logu hem de web katmanındaki hata logu aynı bağlamı yazar; böylece
/// iki kayıt aynı istekte eşleştirilebilir.
/// </summary>
public interface IIstekBaglami
{
    /// <summary>Kaydın ait olduğu kiracı. Oturum yoksa 0.</summary>
    int OrganizasyonId { get; }

    /// <summary>Oturum açmış kullanıcı adı; yoksa "Anonim".</summary>
    string Kullanici { get; }

    string? IpAdresi { get; }
    string? Yol { get; }
    string? HttpYontemi { get; }

    /// <summary>ASP.NET Core'un istek kimliği; günlüklerle eşleştirmek için.</summary>
    string? IstekId { get; }

    /// <summary>
    /// İsteğin hata kodu. İlk çağrıda üretilir, aynı istek içindeki sonraki
    /// çağrılarda aynı değer döner — işlem logu ile hata logu böyle bağlanır.
    /// </summary>
    string HataKodu();
}
