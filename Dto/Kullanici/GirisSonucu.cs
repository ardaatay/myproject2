namespace Dto.Kullanici;

public enum GirisDurumu
{
    Basarili,
    HataliKimlik,
    Kilitli,
    Pasif,
    SifreBelirlenmemis,
    RolTanimlanmamis
}

/// <summary>
/// Giriş denemesinin sonucu. Başarısız durumlarda <see cref="Mesaj"/> kullanıcıya
/// gösterilecek metni taşır; hangi bilginin yanlış olduğu (kullanıcı adı mı şifre mi)
/// bilinçli olarak açıklanmaz.
/// </summary>
public class GirisSonucu
{
    public GirisDurumu Durum { get; set; }
    public string? Mesaj { get; set; }
    public ListKullaniciDto? Kullanici { get; set; }
    public List<string> Roller { get; set; } = [];
    public bool SifreDegistirmeliMi { get; set; }
    public string? SecurityStamp { get; set; }
}
