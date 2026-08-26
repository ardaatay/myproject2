namespace Core.Logging;

/// <summary>
/// İşlem logunda oturum ve şifre olaylarını ayırt eden adlar.
///
/// Bu olaylar aspect ile değil elle yazılır: giriş ve şifre akışlarının
/// parametreleri düz metin şifre taşır ve serileştirilmemelidir.
/// </summary>
public static class OturumOlaylari
{
    /// <summary>Liste ekranında "Modül" sütununda görünen ad.</summary>
    public const string Modul = "Oturum";

    public const string Giris = "Giris";
    public const string Cikis = "Cikis";
    public const string SifreDegistir = "SifreDegistir";
    public const string SifreSifirla = "SifreSifirla";
}
