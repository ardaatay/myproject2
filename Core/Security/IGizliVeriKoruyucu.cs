namespace Core.Security;

/// <summary>
/// Geri döndürülebilir biçimde saklanması gereken gizli değerler için —
/// şifre karmalarından farklı olarak, dizin servis hesabının şifresi gibi
/// değerlerin sonradan kullanılması gerekir, bu yüzden karmalanamaz.
/// </summary>
public interface IGizliVeriKoruyucu
{
    /// <summary>Değeri saklanabilir bir metne çevirir.</summary>
    string Koru(string acikMetin);

    /// <summary>
    /// Korunmuş değeri açar. Anahtarlar yenilendiği ya da kaybedildiği için
    /// çözülemezse <c>null</c> döner — çağıran bunu "yeniden girilmeli" olarak
    /// yorumlamalıdır.
    /// </summary>
    string? Coz(string? korunmusMetin);
}
