using Core.Entity;

namespace Entity.Concrete
{
    public class Rol : IEntity<int>
    {
        public int Id { get; set; }
        public string Ad { get; set; } = default!;
        public bool Durum { get; set; }
        
        public ICollection<KullaniciRol> KullaniciRoller { get; set; } = new List<KullaniciRol>();
    }
}
