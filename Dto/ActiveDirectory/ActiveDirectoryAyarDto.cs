using System.ComponentModel.DataAnnotations;

namespace Dto.ActiveDirectory;

/// <summary>
/// Yönetim ekranının form modeli. Servis hesabı şifresi buraya asla çözülmüş
/// olarak doldurulmaz: alan boş gelirse kayıtlı şifreye dokunulmaz,
/// <see cref="ServisSifresiKayitli"/> yalnızca ekranda "kayıtlı" bilgisini gösterir.
/// </summary>
public class ActiveDirectoryAyarDto : IValidatableObject
{
    public int Id { get; set; }

    [Display(Name = "Active Directory girişi etkin")]
    public bool Aktif { get; set; }

    [Display(Name = "Sunucu")]
    [MaxLength(255)]
    public string? Sunucu { get; set; }

    [Display(Name = "Port")]
    [Range(1, 65535, ErrorMessage = "Port 1-65535 aralığında olmalıdır.")]
    public int Port { get; set; } = 389;

    [Display(Name = "LDAPS (SSL) kullan")]
    public bool SslKullan { get; set; }

    [Display(Name = "StartTLS kullan")]
    public bool StartTlsKullan { get; set; }

    [Display(Name = "Sunucu sertifikasını doğrulama")]
    public bool SertifikaDogrulamasiAtla { get; set; }

    [Display(Name = "Alan adı (UPN son eki)")]
    [MaxLength(255)]
    public string? AlanAdi { get; set; }

    [Display(Name = "NetBIOS alan adı")]
    [MaxLength(100)]
    public string? NetBiosAdi { get; set; }

    [Display(Name = "Taban DN")]
    [MaxLength(500)]
    public string? TabanDn { get; set; }

    [Display(Name = "Servis hesabı")]
    [MaxLength(255)]
    public string? ServisHesabi { get; set; }

    [Display(Name = "Servis hesabı şifresi")]
    [DataType(DataType.Password)]
    [MaxLength(500)]
    public string? ServisHesabiSifresi { get; set; }

    /// <summary>Kayıtlı bir servis şifresi bulunup bulunmadığı. Yalnızca gösterim amaçlıdır.</summary>
    public bool ServisSifresiKayitli { get; set; }

    /// <summary>
    /// Kayıtlı şifre çözülemediğinde (DataProtection anahtarları yenilendiğinde)
    /// yöneticinin şifreyi yeniden girmesi gerektiğini bildirir.
    /// </summary>
    public bool ServisSifresiCozulemedi { get; set; }

    [Display(Name = "Kullanıcı arama filtresi")]
    [MaxLength(500)]
    public string KullaniciAramaFiltresi { get; set; } = ActiveDirectoryVarsayilan.AramaFiltresi;

    [Display(Name = "Kullanıcı adı özniteliği")]
    [MaxLength(100)]
    public string KullaniciAdiOzniteligi { get; set; } = ActiveDirectoryVarsayilan.KullaniciAdiOzniteligi;

    [Display(Name = "Ad soyad özniteliği")]
    [MaxLength(100)]
    public string AdSoyadOzniteligi { get; set; } = ActiveDirectoryVarsayilan.AdSoyadOzniteligi;

    [Display(Name = "E-posta özniteliği")]
    [MaxLength(100)]
    public string EpostaOzniteligi { get; set; } = ActiveDirectoryVarsayilan.EpostaOzniteligi;

    [Display(Name = "Zorunlu grup DN")]
    [MaxLength(500)]
    public string? ZorunluGrupDn { get; set; }

    [Display(Name = "Zaman aşımı (sn)")]
    [Range(1, 120, ErrorMessage = "Zaman aşımı 1-120 saniye aralığında olmalıdır.")]
    public int ZamanAsimiSn { get; set; } = ActiveDirectoryVarsayilan.ZamanAsimiSn;

    [Display(Name = "Her girişte ad soyad ve e-postayı dizinden güncelle")]
    public bool ProfilBilgileriniGuncelle { get; set; } = true;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Kapalıyken eksik yapılandırma sorun değildir; yönetici ayarları
        // parça parça girip sonunda etkinleştirebilsin.
        if (!Aktif)
            yield break;

        if (string.IsNullOrWhiteSpace(Sunucu))
            yield return new ValidationResult("Sunucu adresi zorunludur.", [nameof(Sunucu)]);

        if (string.IsNullOrWhiteSpace(AlanAdi) && string.IsNullOrWhiteSpace(NetBiosAdi))
        {
            yield return new ValidationResult(
                "Alan adı veya NetBIOS alan adından en az biri girilmelidir; " +
                "kullanıcı adı bunlardan biriyle tamamlanarak dizine gönderilir.",
                [nameof(AlanAdi), nameof(NetBiosAdi)]);
        }

        if (string.IsNullOrWhiteSpace(KullaniciAramaFiltresi) || !KullaniciAramaFiltresi.Contains("{0}"))
        {
            yield return new ValidationResult(
                "Arama filtresi, kullanıcı adının yerleşeceği {0} yer tutucusunu içermelidir.",
                [nameof(KullaniciAramaFiltresi)]);
        }

        // Grup denetimi ve profil güncellemesi dizinde arama gerektirir; arama
        // da bir taban DN olmadan yapılamaz.
        var aramaGerekli = !string.IsNullOrWhiteSpace(ZorunluGrupDn) || ProfilBilgileriniGuncelle;

        if (aramaGerekli && string.IsNullOrWhiteSpace(TabanDn))
        {
            yield return new ValidationResult(
                "Zorunlu grup denetimi veya profil güncellemesi için taban DN girilmelidir.",
                [nameof(TabanDn)]);
        }

        if (SslKullan && StartTlsKullan)
        {
            yield return new ValidationResult(
                "LDAPS ve StartTLS birlikte kullanılamaz; birini seçin.",
                [nameof(SslKullan), nameof(StartTlsKullan)]);
        }
    }
}
