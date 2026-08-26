namespace Dto.ActiveDirectory;

/// <summary>
/// Dizine bağlanmak için gereken, çözülmüş hâldeki ayarlar. Yalnızca iş
/// katmanında dolaşır; görünümlere hiçbir zaman verilmez.
/// </summary>
public class ActiveDirectoryBaglantiAyari
{
    public bool Aktif { get; set; }
    public string Sunucu { get; set; } = string.Empty;
    public int Port { get; set; } = ActiveDirectoryVarsayilan.Port;
    public bool SslKullan { get; set; }
    public bool StartTlsKullan { get; set; }
    public bool SertifikaDogrulamasiAtla { get; set; }

    /// <summary>UPN son eki, örneğin <c>kurum.local</c>.</summary>
    public string? AlanAdi { get; set; }

    /// <summary>Eski biçim bağlanma adı için, örneğin <c>KURUM</c>.</summary>
    public string? NetBiosAdi { get; set; }

    public string? TabanDn { get; set; }

    public string? ServisHesabi { get; set; }
    public string? ServisHesabiSifresi { get; set; }

    public string KullaniciAramaFiltresi { get; set; } = ActiveDirectoryVarsayilan.AramaFiltresi;
    public string KullaniciAdiOzniteligi { get; set; } = ActiveDirectoryVarsayilan.KullaniciAdiOzniteligi;
    public string AdSoyadOzniteligi { get; set; } = ActiveDirectoryVarsayilan.AdSoyadOzniteligi;
    public string EpostaOzniteligi { get; set; } = ActiveDirectoryVarsayilan.EpostaOzniteligi;

    public string? ZorunluGrupDn { get; set; }
    public int ZamanAsimiSn { get; set; } = ActiveDirectoryVarsayilan.ZamanAsimiSn;
    public bool ProfilBilgileriniGuncelle { get; set; } = true;

    /// <summary>Dizine bağlanmak için yeterli bilgi var mı.</summary>
    public bool Yapilandirilmis =>
        !string.IsNullOrWhiteSpace(Sunucu) &&
        (!string.IsNullOrWhiteSpace(AlanAdi) || !string.IsNullOrWhiteSpace(NetBiosAdi));

    /// <summary>
    /// Servis hesabıyla arama yapılabiliyor mu. Yapılamıyorsa arama, kullanıcının
    /// kendi kimliğiyle denenir.
    /// </summary>
    public bool ServisHesabiVar =>
        !string.IsNullOrWhiteSpace(ServisHesabi) && !string.IsNullOrWhiteSpace(ServisHesabiSifresi);
}
