namespace Dto.KullaniciRol
{
    public class UpdateKullaniciRolDto
    {
        public int Id { get; set; }
        public int KullaniciId { get; set; }
        public int RolId { get; set; }
        public bool Durum { get; set; }
    }
}
