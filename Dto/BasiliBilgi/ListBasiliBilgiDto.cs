using Dto.BagimliVarliklar;
using Dto.BilgiSinifi;
using Dto.Butunluk;
using Dto.Durum;
using Dto.Erisilebilirlik;
using Dto.EtkilenenKisiSayisi;
using Dto.Gizlilik;
using Dto.Kategori;
using Dto.KurumsalSonuc;
using Dto.SektorelEtki;
using Dto.ToplumsalSonuc;
using Dto.YedeklemeSorumlusu;
using Dto.YedeklemeTipi;

namespace Dto.BasiliBilgi;

public class ListBasiliBilgiDto
{
    public int Id { get; set; }
    public int OrganizasyonId { get; set; }
    public string? Kategori { get; set; }
    public string? AltKategori { get; set; }
    public string? VarlikAdi { get; set; }
    public string? KullanimAmaci { get; set; }
    public int? Miktar { get; set; }
    public string? Durum { get; set; }
    public int? DurumId { get; set; }
    public string? Konum { get; set; }
    public int? KonumId { get; set; }
    public string? VarlikSahibi { get; set; }
    public int? VarlikSahibiId { get; set; }
    public string? VarlikSahibiAltDepartman { get; set; }
    public int? VarlikSahibiAltDepartmanId { get; set; }
    public string? OperasyonelSahibi { get; set; }
    public int? OperasyonelSahibiId { get; set; }
    public string? BilgiSinifi { get; set; }
    public string? Gizlilik { get; set; }
    public string? Butunluk { get; set; }
    public string? Erisilebilirlik { get; set; }
    public string? EtkilenenKisiSayisi { get; set; }
    public string? ToplumsalSonuc { get; set; }
    public string? KurumsalSonuc { get; set; }
    public string? SektorelEtki { get; set; }
    public string? BagimliVarlik { get; set; }
    public string? Rpo { get; set; }
    public string? RpoTip { get; set; }
    public string? Rto { get; set; }
    public string? RtoTip { get; set; }
    public string? Mtpd { get; set; }
    public string? MtpdTip { get; set; }
    public string? KurtarmaPlanlari { get; set; }
    public string? KisiselVeriBarindirma { get; set; }
    public string? SaklamaSuresi { get; set; }
    public string? SaklamaSuresiTip { get; set; }
    public string? Notlar { get; set; }
    public DateTime? EnvantereGirisTarihi { get; set; }
    public DateTime? EnvanterGuncellemeTarihi { get; set; }
    public DateTime? EnvanterdenCikisTarihi { get; set; }
}