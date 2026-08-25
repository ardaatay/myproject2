using System.ComponentModel.DataAnnotations;

namespace Dto.IoTCihaz;

public class UpdateIoTCihazDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Varlık Kategorileri zorunludur.")]
    public int KategoriId { get; set; }

    [Required(ErrorMessage = "Varlık Grubu Adı zorunludur.")]
    public int AltKategoriId { get; set; }

    [Required(ErrorMessage = "Varlık adı bilgisi zorunludur.")]
    public string VarlikAdi { get; set; } = null!;

    [Required(ErrorMessage = "Kullanım amacı bilgisi zorunludur.")]
    public string KullanimAmaci { get; set; } = null!;

    [Required(ErrorMessage = "Miktar zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "Miktar 1'den küçük olamaz")]
    public int Miktar { get; set; } = 1;

    [Required(ErrorMessage = "Durum zorunludur.")]
    public int DurumId { get; set; }

    public string Konum { get; set; } = null!;

    [Required(ErrorMessage = "Konum bilgisi zorunludur.")]
    public int? KonumId { get; set; }

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
    [Range(1, 43200, ErrorMessage = "1-43200 Dakika arasında olmalıdır.")]
    public int Rpo { get; set; }

    [Required(ErrorMessage = "Rpo tip zorunludur.")]
    public string RpoTip { get; set; } = null!;

    [Range(1, 43200, ErrorMessage = "1-43200 Dakika arasında olmalıdır.")]
    [Required(ErrorMessage = "Rto zorunludur. 1-43200 Dakika arasında olmalıdır.")]
    public int Rto { get; set; }

    [Required(ErrorMessage = "Rto tip zorunludur.")]
    public string RtoTip { get; set; } = null!;

    [Required(ErrorMessage = "Mtpd zorunludur. 1-43200 Dakika arasında olmalıdır.")]
    [Range(1, 43200, ErrorMessage = "1-43200 Dakika arasında olmalıdır.")]
    public int Mtpd { get; set; }

    [Required(ErrorMessage = "Mtpd tip zorunludur.")]
    public string MtpdTip { get; set; } = null!;

    public string? KurtarmaPlanlari { get; set; }

    [Required(ErrorMessage = "Yedekleme tipi bilgisi zorunludur.")]
    public int YedeklemeTipiId { get; set; }

    public string? YedeklemeTuru { get; set; }
    public string? YedeklemeSikligi { get; set; }
    public string? YedeklerinSaklamaSuresi { get; set; }
    public string? YedeklemeAlani { get; set; }
    public string? YedektenDonusPlani { get; set; }
    public int? YedeklemeSorumlusuId { get; set; }

    [Required(ErrorMessage = "Kriptoloji bilgisi zorunludur.")]
    public bool Kriptoloji { get; set; }

    public int? KriptolojiTuruId { get; set; }
    public string? KullanilanKriptoloji { get; set; }
    public int? AnahtarSorumlusuId { get; set; }

    [Required(ErrorMessage = "Kişisel veri barındırma bilgisi zorunludur.")]
    public bool KisiselVeriBarindirma { get; set; }

    [Required(ErrorMessage = "Anlık mesajlaşma kullanımı bilgisi zorunludur.")]
    public bool AnlikMesajlasmaKullanimi { get; set; }

    [Required(ErrorMessage = "Bulut bilişim bilgisi zorunludur.")]
    public bool BulutBilisim { get; set; }

    [Required(ErrorMessage = "Yeni gelişmeler ve tedarik bilgisi zorunludur.")]
    public bool YeniGelismelerveTedarik { get; set; }

    [Required(ErrorMessage = "Kritik altyapı sistemi bilgisi zorunludur.")]
    public bool KritikAltyapiSistemi { get; set; }

    public string? IpAdresi { get; set; }
    public string? IsletimSistemi { get; set; }
    public int? LisansTakipSorumlusuId { get; set; }
    public string? MarkaModel { get; set; }
    public string? SeriNumarasi { get; set; }
    [MaxLength(500)] public string? ZimmetSahibi { get; set; }
    [MaxLength(500)] public string? Notlar { get; set; }
}