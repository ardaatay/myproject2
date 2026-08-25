namespace Dto.Kullanici;

public class KullaniciRolAtamaDto
{
    public int KullaniciId { get; set; }
    public string? KullaniciAdi { get; set; }
    public List<RolSecimDto>? Roller { get; set; }
}