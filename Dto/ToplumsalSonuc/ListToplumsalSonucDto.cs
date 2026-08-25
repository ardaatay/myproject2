namespace Dto.ToplumsalSonuc;

public class ListToplumsalSonucDto
{
    public int Id { get; set; }
    public string? Ad { get; set; }
    public DateTime OlusturmaTarihi { get; set; }
    public DateTime? GuncellemeTarihi { get; set; }
    public bool Aktif { get; set; }
} 