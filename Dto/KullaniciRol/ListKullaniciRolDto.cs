using Dto.Kullanici;
using Dto.Rol;

namespace Dto.KullaniciRol
{
    public class ListKullaniciRolDto
    {
        public int Id { get; set; }
        public UpdateKullaniciDto Kullanici { get; set; } = default!;
        public CreateRolDto Rol { get; set; } = default!;
        public bool Durum { get; set; }
    }
}