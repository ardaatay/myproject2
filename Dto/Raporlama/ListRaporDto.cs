namespace Dto.Raporlama;

public class ListRaporDto
{
    public int Id { get; set; }
    public int OrganizasyonId { get; set; }
    public string? KategoriAd { get; set; }
    public int? KategoriId { get; set; }
    public string? AltKategoriAd { get; set; }
    public int? AltKategoriId { get; set; }
    public string? VarlikAdi { get; set; }
    public string? KullanimAmaci { get; set; }
    public int? Miktar { get; set; }
    public string? DurumAd { get; set; }
    public int? DurumId { get; set; }
    public string? Konum { get; set; }
    public int? KonumId { get; set; }
    public string? VarlikSahibi { get; set; }
    public int? VarlikSahibiId { get; set; }
    public string? VarlikSahibiAltDepartman { get; set; }
    public int? VarlikSahibiAltDepartmanId { get; set; }
    public string? OperasyonelSahibi { get; set; }
    public DateTime? EnvantereGirisTarihi { get; set; }
    public DateTime? EnvanterGuncellemeTarihi { get; set; }
    public DateTime? EnvanterdenCikisTarihi { get; set; }
}