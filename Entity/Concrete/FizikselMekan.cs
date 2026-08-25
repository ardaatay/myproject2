using Core.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entity.Concrete;

public class FizikselMekan : IEntity<int>, IKiraciEntity
{
    public int Id { get; set; }

    /// <summary>Kaydın ait olduğu kiracı.</summary>
    public int OrganizasyonId { get; set; }
    public int KategoriId { get; set; }
    public int AltKategoriId { get; set; }

    [MaxLength(500)] public string VarlikAdi { get; set; } = null!;

    [MaxLength(500)] public string KullanimAmaci { get; set; } = null!;
    public int Miktar { get; set; }
    public int DurumId { get; set; }

    [MaxLength(500)] public string Konum { get; set; } = null!;
    public int KonumId { get; set; }

    [MaxLength(500)] public string VarlikSahibi { get; set; } = null!;
    public int? VarlikSahibiId { get; set; }

    [MaxLength(500)] public string? VarlikSahibiAltDepartman { get; set; }
    public int? VarlikSahibiAltDepartmanId { get; set; }

    [MaxLength(500)] public string OperasyonelSahibi { get; set; } = null!;
    public int? OperasyonelSahibiId { get; set; }
    public int? BilgiSinifiId { get; set; }
    public int? GizlilikId { get; set; }
    public int? ButunlukId { get; set; }
    public int? ErisilebilirlikId { get; set; }
    public int? EtkilenenKisiSayisiId { get; set; }
    public int? ToplumsalSonucId { get; set; }
    public int? KurumsalSonucId { get; set; }
    public int? SektorelEtkiId { get; set; }
    public int? BagimliVarlikId { get; set; }
    public int? Rpo { get; set; }
    [MaxLength(50)] public string? RpoTip { get; set; } = null!;
    public int? Rto { get; set; }
    [MaxLength(50)] public string? RtoTip { get; set; } = null!;
    public int? Mtpd { get; set; }
    [MaxLength(50)] public string? MtpdTip { get; set; } = null!;

    [MaxLength(500)] public string? KurtarmaPlanlari { get; set; }
    public bool? KisiselVeriBarindirma { get; set; }

    public bool? BasiliBilgi { get; set; }
    [MaxLength(500)] public string? Notlar { get; set; }
    public DateTime? EnvantereGirisTarihi { get; set; }
    public DateTime? EnvanterGuncellemeTarihi { get; set; }
    public DateTime? EnvanterdenCikisTarihi { get; set; }
    public bool? SilinsinMi { get; set; }

    // Navigation Properties
    public virtual Kategori? Kategori { get; set; }
    public virtual Kategori? AltKategori { get; set; }
    public virtual BagimliVarlik? BagimliVarlik { get; set; }
    public virtual BilgiSinifi? BilgiSinifi { get; set; }
    public virtual Butunluk? Butunluk { get; set; }
    public virtual Durum? Durum { get; set; }
    public virtual Erisilebilirlik? Erisilebilirlik { get; set; }
    public virtual EtkilenenKisiSayisi? EtkilenenKisiSayisi { get; set; }
    public virtual Gizlilik? Gizlilik { get; set; }
    public virtual KurumsalSonuc? KurumsalSonuc { get; set; }
    public virtual SektorelEtki? SektorelEtki { get; set; }
    public virtual ToplumsalSonuc? ToplumsalSonuc { get; set; }
}