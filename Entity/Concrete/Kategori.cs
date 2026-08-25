using Core.Entity;

namespace Entity.Concrete;

public class Kategori : IEntity<int>
{
    public int Id { get; set; }
    public string Ad { get; set; } = default!;
    public int? UstId { get; set; }
    public bool Durum { get; set; }

    public virtual Kategori? Ust { get; set; }
}