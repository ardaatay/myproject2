namespace Dto.ActiveDirectory;

public enum AdDogrulamaDurumu
{
    Basarili,

    /// <summary>Kullanıcı adı ya da şifre dizin tarafından kabul edilmedi.</summary>
    HataliKimlik,

    /// <summary>Kimlik doğru ama hesap dizinde devre dışı, kilitli veya süresi dolmuş.</summary>
    HesapKullanilamaz,

    /// <summary>Kimlik doğru ama kullanıcı zorunlu grubun üyesi değil.</summary>
    GrupUyeligiYok,

    /// <summary>Sunucuya ulaşılamadı, TLS kurulamadı veya ayarlar eksik.</summary>
    SunucuHatasi
}

/// <summary>
/// Dizin doğrulamasının sonucu. <see cref="Mesaj"/> yalnızca yöneticiye
/// gösterilecek tanılama metnini taşır; son kullanıcıya giriş ekranında
/// hangi bilginin yanlış olduğu açıklanmaz.
/// </summary>
public class AdDogrulamaSonucu
{
    public AdDogrulamaDurumu Durum { get; set; }
    public string? Mesaj { get; set; }
    public AdKullaniciBilgisi? Kullanici { get; set; }

    public bool Basarili => Durum == AdDogrulamaDurumu.Basarili;

    public static AdDogrulamaSonucu Hata(AdDogrulamaDurumu durum, string mesaj) =>
        new() { Durum = durum, Mesaj = mesaj };
}

/// <summary>Dizinden okunan profil alanları.</summary>
public class AdKullaniciBilgisi
{
    public string KullaniciAdi { get; set; } = string.Empty;
    public string? AdSoyad { get; set; }
    public string? Eposta { get; set; }
    public string? DistinguishedName { get; set; }
}

/// <summary>Yönetim ekranındaki "bağlantıyı sına" düğmesinin çıktısı.</summary>
public class ActiveDirectoryTestSonucu
{
    public bool Basarili { get; set; }
    public string Mesaj { get; set; } = string.Empty;

    /// <summary>Test kullanıcısı verildiyse dizinden okunan bilgileri.</summary>
    public AdKullaniciBilgisi? Kullanici { get; set; }
}
