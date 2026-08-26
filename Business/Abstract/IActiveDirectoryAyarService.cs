using Dto.ActiveDirectory;

namespace Business.Abstract;

/// <summary>
/// Kiracının dizin ayarlarının okunması ve yazılması. Servis hesabı şifresi
/// yalnızca burada korunur ve çözülür; dışarıya hiçbir zaman düz metin sızmaz.
/// </summary>
public interface IActiveDirectoryAyarService
{
    /// <summary>Yönetim ekranı için ayarlar. Şifre alanı her zaman boş döner.</summary>
    Task<ActiveDirectoryAyarDto> GetirAsync();

    /// <summary>
    /// Ayarları kaydeder. <see cref="ActiveDirectoryAyarDto.ServisHesabiSifresi"/>
    /// boş bırakılırsa kayıtlı şifre korunur.
    /// </summary>
    Task KaydetAsync(ActiveDirectoryAyarDto dto);

    /// <summary>
    /// Giriş akışının kullandığı, şifresi çözülmüş ayarlar. Kiracı kimliği
    /// açıkça verilir: giriş anında henüz oturum açılmadığı için aktif
    /// organizasyon claim'i mevcut değildir.
    /// </summary>
    Task<ActiveDirectoryBaglantiAyari?> BaglantiAyariGetirAsync(int organizasyonId);

    /// <summary>
    /// Sınama için, formdaki değerlerle çalışan ayar nesnesi. Şifre alanı boşsa
    /// kayıtlı şifre kullanılır; böylece yönetici şifreyi yeniden yazmadan da
    /// bağlantıyı sınayabilir.
    /// </summary>
    Task<ActiveDirectoryBaglantiAyari> SinamaAyariUretAsync(ActiveDirectoryAyarDto dto);

    /// <summary>
    /// Kiracıda kaç kullanıcının dizin üzerinden giriş yaptığı. Ayarlar kapatılmak
    /// üzereyken kaç kişinin etkileneceğini göstermek için kullanılır.
    /// </summary>
    Task<int> DizinKullanicisiSayisiAsync();
}
