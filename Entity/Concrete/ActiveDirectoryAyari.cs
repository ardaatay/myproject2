using System.ComponentModel.DataAnnotations;
using Core.Entity;
using Dto.ActiveDirectory;

namespace Entity.Concrete;

/// <summary>
/// Bir kiracının Active Directory bağlantı ayarları. Her organizasyon için en
/// fazla bir kayıt bulunur; kayıt yoksa o kiracıda dizin girişi kapalıdır.
///
/// Servis hesabının şifresi burada geri döndürülebilir biçimde korunarak
/// saklanır (bkz. <c>IGizliVeriKoruyucu</c>) — karma kullanılamaz, çünkü
/// değerin dizine sunulması gerekir.
/// </summary>
public class ActiveDirectoryAyari : IEntity<int>, IKiraciEntity
{
    public int Id { get; set; }

    public int OrganizasyonId { get; set; }

    /// <summary>Kapalıyken hiçbir kullanıcı dizin üzerinden giriş yapamaz.</summary>
    public bool Aktif { get; set; }

    [MaxLength(255)] public string? Sunucu { get; set; }

    public int Port { get; set; } = ActiveDirectoryVarsayilan.Port;

    /// <summary>Bağlantı en baştan TLS ile kurulur (LDAPS, tipik olarak 636).</summary>
    public bool SslKullan { get; set; }

    /// <summary>Düz bağlantı kurulduktan sonra TLS'e yükseltilir (tipik olarak 389).</summary>
    public bool StartTlsKullan { get; set; }

    /// <summary>
    /// Kurum içi sertifika otoritesi tanınmıyorsa gerekebilir. Açıkken bağlantı
    /// araya girme saldırılarına açıktır; yalnızca kapalı ağlarda kullanılmalıdır.
    /// </summary>
    public bool SertifikaDogrulamasiAtla { get; set; }

    /// <summary>UPN son eki: <c>kullanici@kurum.local</c> biçiminde bağlanmak için.</summary>
    [MaxLength(255)] public string? AlanAdi { get; set; }

    /// <summary>Eski biçim bağlanma adı: <c>KURUM\kullanici</c>.</summary>
    [MaxLength(100)] public string? NetBiosAdi { get; set; }

    /// <summary>Aramaların başladığı düğüm, örneğin <c>DC=kurum,DC=local</c>.</summary>
    [MaxLength(500)] public string? TabanDn { get; set; }

    /// <summary>
    /// Dizinde arama yapmak için kullanılan hesap. Boşsa arama, giriş yapan
    /// kullanıcının kendi kimliğiyle denenir.
    /// </summary>
    [MaxLength(255)] public string? ServisHesabi { get; set; }

    /// <summary>Korunmuş (şifrelenmiş) servis hesabı şifresi. Asla düz metin tutulmaz.</summary>
    [MaxLength(1000)] public string? ServisHesabiSifresiKorunmus { get; set; }

    /// <summary>Kullanıcı adının <c>{0}</c> yerine geçtiği LDAP arama filtresi.</summary>
    [MaxLength(500)]
    public string KullaniciAramaFiltresi { get; set; } = ActiveDirectoryVarsayilan.AramaFiltresi;

    [MaxLength(100)]
    public string KullaniciAdiOzniteligi { get; set; } = ActiveDirectoryVarsayilan.KullaniciAdiOzniteligi;

    [MaxLength(100)]
    public string AdSoyadOzniteligi { get; set; } = ActiveDirectoryVarsayilan.AdSoyadOzniteligi;

    [MaxLength(100)]
    public string EpostaOzniteligi { get; set; } = ActiveDirectoryVarsayilan.EpostaOzniteligi;

    /// <summary>Doluysa yalnızca bu grubun üyeleri giriş yapabilir.</summary>
    [MaxLength(500)] public string? ZorunluGrupDn { get; set; }

    public int ZamanAsimiSn { get; set; } = ActiveDirectoryVarsayilan.ZamanAsimiSn;

    /// <summary>Her başarılı girişte ad soyad ve e-posta dizinden tazelenir.</summary>
    public bool ProfilBilgileriniGuncelle { get; set; } = true;

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
