using Dto.Kullanici.Enum;

namespace Dto.KullaniciBirim;

public class ListKullaniciBirimDto
{
    public int Id { get; set; }
    public int KullaniciId { get; set; }
    public string? Username { get; set; }
    public GirisYontemi GirisYontemi { get; set; }
    public string? GirisYontemiStr { get; set; }
    public string? AdSoyad { get; set; }
    public int BirimId { get; set; }
    public string? BirimAd { get; set; }
    public string? DurumStr { get; set; }
}
