namespace Dto.KullaniciBirim;

public class CreateKullaniciBirimDto
{
    public int KullaniciId { get; set; }
    public int BirimId { get; set; }
    public string? BirimAd { get; set; }
}