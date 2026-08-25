using Core.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entity.Concrete;

public class KriptografiEnvanteri : IEntity<int>, IKiraciEntity
{
    public int Id { get; set; }

    /// <summary>Kaydın ait olduğu kiracı.</summary>
    public int OrganizasyonId { get; set; }
    [MaxLength(500)] public string VarlikSahibi { get; set; } = null!;
    public int VarlikSahibiId { get; set; }
    [MaxLength(500)] public string? VarlikSahibiAltDepartman { get; set; }
    public int? VarlikSahibiAltDepartmanId { get; set; }
    [MaxLength(500)] public string? VarlikAdi { get; set; } = null!;
    [MaxLength(500)] public string? UretimYeri { get; set; } = null!;
    [MaxLength(500)] public string? KullanimAmaci { get; set; } = null!;
    public DateTime? OlusturmaTarihi { get; set; }
    public int? KullanimSuresi { get; set; }
    [MaxLength(50)] public string? KullanimSuresiTip { get; set; } = null!;
    public int? AnahtarSorumlusuId { get; set; }
    [MaxLength(500)] public string? AnahtarSaklamaAlani { get; set; } = null!;
    [MaxLength(500)] public string? DestekAlinanTedarikci { get; set; } = null!;
    [MaxLength(500)] public string? DonanimYazilim { get; set; } = null!;
    [MaxLength(500)] public string? Algoritma { get; set; } = null!;
    [MaxLength(500)] public string? OrtakKriterler { get; set; } = null!;
    public int? KullanimSeviyesiId { get; set; }
    [MaxLength(500)] public string? KullanimKabiliyetleri { get; set; } = null!;
    [MaxLength(500)] public string? Notlar { get; set; }

    public bool? Aktif { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    public bool? SilinsinMi { get; set; }

    // Navigation Properties
    public virtual AnahtarSorumlusu? AnahtarSorumlusu { get; set; }
    public virtual KullanimSeviyesi? KullanimSeviyesi { get; set; }
}