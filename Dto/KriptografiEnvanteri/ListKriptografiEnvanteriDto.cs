using Dto.AnahtarSorumlusu;
using Dto.KullanimSeviyesi;

namespace Dto.KriptografiEnvanteri;

public class ListKriptografiEnvanteriDto
{
    public int Id { get; set; }
    public int OrganizasyonId { get; set; }
    public string VarlikSahibi { get; set; } = null!;
    public int VarlikSahibiId { get; set; }
    public string? VarlikSahibiAltDepartman { get; set; }
    public int? VarlikSahibiAltDepartmanId { get; set; }
    public string? VarlikAdi { get; set; } = null!;
    public string? UretimYeri { get; set; } = null!;
    public string? KullanimAmaci { get; set; } = null!;
    public DateTime? OlusturmaTarihi { get; set; }
    public string? KullanimSuresi { get; set; }
    public string? AnahtarSorumlusu { get; set; } = null!;
    public string? AnahtarSaklamaAlani { get; set; } = null!;
    public string? DestekAlinanTedarikci { get; set; } = null!;
    public string? DonanimYazilim { get; set; } = null!;
    public string? Algoritma { get; set; } = null!;
    public string? OrtakKriterler { get; set; } = null!;
    public string? KullanimSeviyesi { get; set; } = null!;
    public string? KullanimKabiliyetleri { get; set; } = null!;
    public string? Notlar { get; set; }
    public bool? Aktif { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
}