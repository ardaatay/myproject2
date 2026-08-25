using System.ComponentModel.DataAnnotations;

namespace Dto.KriptografiEnvanteri;

public class UpdateKriptografiEnvanteriDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Lütfen varlık adı girin")]
    public string VarlikAdi { get; set; } = null!;
    
    public string VarlikSahibi { get; set; } = null!;

    [Required(ErrorMessage = "Varlık sahibi bilgisi zorunludur.")]
    public int VarlikSahibiId { get; set; }

    public string? VarlikSahibiAltDepartman { get; set; }
    
    public int? VarlikSahibiAltDepartmanId { get; set; }

    [Required(ErrorMessage = "Lütfen üretim yeri girin")]
    public string UretimYeri { get; set; } = null!;

    [Required(ErrorMessage = "Lütfen kullanım amacı girin")]
    public string KullanimAmaci { get; set; } = null!;

    [Required(ErrorMessage = "Oluşturma tarihi girin")]
    public DateTime OlusturmaTarihi { get; set; }

    [Required(ErrorMessage = "Lütfen kullanım süresi girin")]
    public int KullanimSuresi { get; set; }

    [Required(ErrorMessage = "Lütfen kullanım süresi tipi girin")]
    public string KullanimSuresiTip { get; set; } = null!;

    [Required(ErrorMessage = "Lütfen anahtar sorumlusu girin")]
    public int AnahtarSorumlusuId { get; set; }

    [Required(ErrorMessage = "Anahtar saklama alanı girin")]
    public string AnahtarSaklamaAlani { get; set; } = null!;

    [Required(ErrorMessage = "Destek alınan tedarikçi girin")]
    public string DestekAlinanTedarikci { get; set; } = null!;

    [Required(ErrorMessage = "Donanım yazılımı girin")]
    public string DonanimYazilim { get; set; } = null!;

    [Required(ErrorMessage = "Algoritma girin")]
    public string Algoritma { get; set; } = null!;

    [Required(ErrorMessage = "Ortak kriterler girin")]
    public string OrtakKriterler { get; set; } = null!;

    [Required(ErrorMessage = "Kullanım seviyesi girin")]
    public int KullanimSeviyesiId { get; set; }

    [Required(ErrorMessage = "Kullanım kabiliyetleri girin")]
    public string KullanimKabiliyetleri { get; set; } = null!;
    [MaxLength(500)] public string? Notlar { get; set; }
}