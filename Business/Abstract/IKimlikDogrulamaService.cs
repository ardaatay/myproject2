using Dto.Kullanici;

namespace Business.Abstract;

public interface IKimlikDogrulamaService
{
    /// <summary>
    /// Kullanıcı adı ve şifreyi doğrular; kilitleme sayaçlarını günceller.
    /// Başarısız durumlarda hangi bilginin yanlış olduğunu açıklamaz.
    /// </summary>
    Task<GirisSonucu> GirisYapAsync(string username, string sifre);

    /// <summary>
    /// Şifreyi değiştirir ve SecurityStamp'i yeniler; böylece açık kalan diğer
    /// oturumlar geçersizleşir. <paramref name="mevcutSifreDogrulansin"/> false ise
    /// mevcut şifre sorulmaz (ilk şifre belirleme akışı).
    /// </summary>
    Task<(bool Basarili, IReadOnlyList<string> Hatalar, string? YeniSecurityStamp)> SifreDegistirAsync(
        int kullaniciId,
        string? mevcutSifre,
        string yeniSifre,
        bool mevcutSifreDogrulansin = true);

    /// <summary>Oturum çerezindeki damganın hâlâ geçerli olup olmadığını söyler.</summary>
    Task<bool> SecurityStampGecerliMiAsync(int kullaniciId, string? securityStamp);

    /// <summary>
    /// Yönetici tarafından şifre sıfırlama. Tek kullanımlık bir şifre üretir,
    /// hesabı ilk girişte değiştirmeye zorlar ve varsa kilidi kaldırır.
    /// Üretilen şifre yalnızca bir kez döner; saklanmaz.
    /// </summary>
    Task<string?> SifreSifirlaAsync(int kullaniciId);
}
