using System.ComponentModel.DataAnnotations;

namespace Dto.Personel;

public class CreatePersonelDto
{
    [Required(ErrorMessage = "Varlık Kategorileri zorunludur.")]
    public int KategoriId { get; set; }

    [Required(ErrorMessage = "Varlık Grubu Adı zorunludur.")]
    public int AltKategoriId { get; set; }

    [Required(ErrorMessage = "Varlık adı bilgisi zorunludur.")]
    public string VarlikAdi { get; set; } = null!;

    [Required(ErrorMessage = "Kullanım amacı bilgisi zorunludur.")]
    public string KullanimAmaci { get; set; } = null!;

    [Required(ErrorMessage = "Miktar bilgisi zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "Miktar 1'den küçük olamaz")]
    public int Miktar { get; set; } = 1;

    [Required(ErrorMessage = "Durum bilgisi zorunludur.")]
    public int DurumId { get; set; }

    public string Konum { get; set; } = null!;

    [Required(ErrorMessage = "Konum bilgisi zorunludur.")]
    public int KonumId { get; set; }

    public string VarlikSahibi { get; set; } = null!;

    [Required(ErrorMessage = "Varlık sahibi bilgisi zorunludur.")]
    public int VarlikSahibiId { get; set; }

    public string? VarlikSahibiAltDepartman { get; set; }
    public int? VarlikSahibiAltDepartmanId { get; set; }

    public string OperasyonelSahibi { get; set; } = null!;

    [Required(ErrorMessage = "Operasyonel sahibi bilgisi zorunludur.")]
    public int OperasyonelSahibiId { get; set; }

    [Required(ErrorMessage = "Bilgi sınıfı bilgisi zorunludur.")]
    public int BilgiSinifiId { get; set; }

    [Required(ErrorMessage = "Gizlilik bilgisi zorunludur.")]
    public int GizlilikId { get; set; }

    [Required(ErrorMessage = "Bütünlük bilgisi zorunludur.")]
    public int ButunlukId { get; set; }

    [Required(ErrorMessage = "Erişilebilirlik bilgisi zorunludur.")]
    public int ErisilebilirlikId { get; set; }

    [Required(ErrorMessage = "Etkilenen kişi sayısı bilgisi zorunludur.")]
    public int EtkilenenKisiSayisiId { get; set; }

    [Required(ErrorMessage = "Toplumsal sonuç bilgisi zorunludur.")]
    public int ToplumsalSonucId { get; set; }

    [Required(ErrorMessage = "Kurumsal sonuç bilgisi zorunludur.")]
    public int KurumsalSonucId { get; set; }

    [Required(ErrorMessage = "Sektörel etki bilgisi zorunludur.")]
    public int SektorelEtkiId { get; set; }

    [Required(ErrorMessage = "Bağımlı varlık bilgisi zorunludur.")]
    public int BagimliVarlikId { get; set; }

    [Required(ErrorMessage = "Rpo zorunludur. 1-43200 Dakika arasında olmalıdır.")]
    [Range(1, 43200, ErrorMessage = "Miktar 1 ile 43200 arası olmalıdır.")]
    public int Rpo { get; set; }

    [Required(ErrorMessage = "Rpo tip zorunludur.")]
    public string RpoTip { get; set; } = null!;

    [Required(ErrorMessage = "Mtpd zorunludur. 1-43200 Dakika arasında olmalıdır.")]
    [Range(1, 43200, ErrorMessage = "Miktar 1 ile 43200 arası olmalıdır.")]
    public int Mtpd { get; set; }

    [Required(ErrorMessage = "Mtpd tip zorunludur.")]
    public string MtpdTip { get; set; } = null!;

    public string? KurtarmaPlanlari { get; set; }
    public bool VekaletEdilmeDurumu { get; set; }
    [MaxLength(500)] public string? Notlar { get; set; }
}