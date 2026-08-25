namespace Dto.Rol
{
    public class UpdateRolDto
    {
        public int Id { get; set; }
        public string Ad { get; set; } = default!;
        public bool Durum { get; set; }
    }
}
