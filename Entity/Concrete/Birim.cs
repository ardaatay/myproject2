using System.ComponentModel.DataAnnotations;
using Core.Entity;

namespace Entity.Concrete;

public class Birim : IEntity<int>, IKiraciEntity
{
    public int Id { get; set; }

    /// <summary>Kaydın ait olduğu kiracı.</summary>
    public int OrganizasyonId { get; set; }

    /// <summary>Üst birim. Kök birimlerde null.</summary>
    public int? UstId { get; set; }

    [MaxLength(500)] public string Ad { get; set; } = null!;

    /// <summary>Kurum içi birim kodu. Opsiyonel, kurumun kendi kodlama şeması.</summary>
    [MaxLength(50)]
    public string? Kod { get; set; }

    /// <summary>
    /// Materialized path: "/1/5/12/" biçiminde, kökten bu birime kadar olan kimlikler.
    /// Alt ağaç sorguları bu sütun üzerinden StartsWith ile yapılır.
    /// </summary>
    [MaxLength(900)]
    public string Yol { get; set; } = null!;

    /// <summary>Kök birimlerde 0, her alt kademede bir artar.</summary>
    public int Seviye { get; set; }

    /// <summary>Aynı seviyedeki kardeşler arasında görüntüleme sırası.</summary>
    public int Sira { get; set; }

    public bool Durum { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }

    // Navigation Properties
    public virtual Birim? Ust { get; set; }
    public virtual ICollection<Birim> AltBirimler { get; set; } = new List<Birim>();
}
