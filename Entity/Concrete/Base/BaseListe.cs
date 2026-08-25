using Core.Entity;

namespace Entity.Concrete.Base;

public abstract class BaseListe : IEntity<int>
{
    public int Id { get; set; }
    public string Ad { get; set; } = default!;
    public bool Durum { get; set; }
}