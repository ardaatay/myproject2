using Core.Entity;

namespace Entity.Concrete;

public class GuvenlikModu : IEntity<int>, IKiraciEntity
{
    public int Id { get; set; }
    public int OrganizasyonId { get; set; }
    public bool Durum { get; set; }
}