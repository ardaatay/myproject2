using Dto.ActiveDirectory;

namespace Business.Abstract;

/// <summary>
/// Active Directory ile konuşan tek yer. LDAP ayrıntıları burada kalır;
/// kimlik doğrulama akışı yalnızca sonuç türlerini bilir.
/// </summary>
public interface IActiveDirectoryService
{
    /// <summary>
    /// Kullanıcı adı ve şifreyi dizine bağlanarak doğrular. Yapılandırmada
    /// zorunlu grup varsa üyelik de denetlenir; üyelik doğrulanamıyorsa giriş
    /// bilinçli olarak reddedilir (kapalı tarafta kalınır).
    /// </summary>
    Task<AdDogrulamaSonucu> DogrulaAsync(
        ActiveDirectoryBaglantiAyari ayar,
        string kullaniciAdi,
        string sifre,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Yönetim ekranındaki sınama. Test kullanıcısı verilirse onun kimliğiyle,
    /// verilmezse servis hesabıyla bağlanmayı dener.
    /// </summary>
    Task<ActiveDirectoryTestSonucu> BaglantiTestEtAsync(
        ActiveDirectoryBaglantiAyari ayar,
        string? testKullaniciAdi,
        string? testSifre,
        CancellationToken cancellationToken = default);
}
