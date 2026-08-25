using Entity.Concrete.Base;
using Core.Entity;

namespace Entity.Concrete;

public class EpostaTalep : BaseListe, IKiraciEntity
{
    /// <summary>Kaydın ait olduğu kiracı. Aşağıdaki KurumId ile karıştırılmamalıdır.</summary>
    public int OrganizasyonId { get; set; }

    /// <summary>Talebin ilgili olduğu kurum — ortak referans listesinden seçilir.</summary>
    public int KurumId { get; set; }
    public string? UcuncuTaraf { get; set; }
    public string? TalepEdilen { get; set; }
    public string? TalepEden { get; set; }
    public string? TalepNedeni { get; set; }
    public string? TalepSuresi { get; set; }
    public string? DosyaYolu { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }

    public virtual Kurum Kurum { get; set; } = null!;
}