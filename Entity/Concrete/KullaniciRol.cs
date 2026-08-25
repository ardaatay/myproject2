using Core.Entity;

namespace Entity.Concrete
{
    public class KullaniciRol : IEntity<int>, IKiraciEntity
    {
        public int Id { get; set; }
        public int OrganizasyonId { get; set; }
        public int KullaniciId { get; set; }
        public int RolId { get; set; }
        public bool Durum { get; set; }

        public virtual Kullanici Kullanici { get; set; } = default!;
        public virtual Rol Rol { get; set; } = default!;
    }
}
