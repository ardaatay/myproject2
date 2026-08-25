namespace Dto.KullaniciBirim;

public class ListKullaniciBirimDto
{
    public int Id { get; set; }
    public int KullaniciId { get; set; }
    public string? Username { get; set; }
    public int BirimId { get; set; }
    public string? BirimAd { get; set; }
    public string? DurumStr { get; set; }
}