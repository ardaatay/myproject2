namespace Dto.EpostaTalep;

public class ListEpostaTalepDto
{
    public int Id { get; set; }
    public string KurumAd { get; set; } = null!;
    public int KurumId { get; set; }
    public string? UcuncuTaraf { get; set; }
    public string? TalepEdilen { get; set; }
    public string? TalepEden { get; set; }
    public string? TalepNedeni { get; set; }
    public string? TalepSuresi { get; set; }
    public string? DosyaYolu { get; set; }
    public string? Durum { get; set; }
}