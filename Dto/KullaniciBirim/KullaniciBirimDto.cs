using Dto.Kullanici;

namespace Dto.KullaniciBirim;

public class KullaniciBirimDto
{
    public int Id { get; set; }
    public int KullaniciId { get; set; }
    public int BirimId { get; set; }
    public string? BirimAd { get; set; }
}