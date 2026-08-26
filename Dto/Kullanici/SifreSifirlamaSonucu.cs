namespace Dto.Kullanici;

/// <summary>
/// Yönetici şifre sıfırlamasının sonucu. Üretilen şifre yalnızca bir kez döner
/// ve hiçbir yerde saklanmaz. Dizine bağlı hesaplarda şifre uygulamada
/// tutulmadığı için sıfırlama yapılamaz; bu durumda <see cref="Hata"/> dolar.
/// </summary>
public class SifreSifirlamaSonucu
{
    public bool Basarili { get; set; }
    public string? Sifre { get; set; }
    public string? Hata { get; set; }

    public static SifreSifirlamaSonucu Basarisiz(string hata) => new() { Hata = hata };
    public static SifreSifirlamaSonucu Olustu(string sifre) => new() { Basarili = true, Sifre = sifre };
}
