namespace Dto.KurumsalSonuc;

public class ListKurumsalSonucDto
{
    public int Id { get; set; }
    public string? Ad { get; set; }
    public DateTime OlusturmaTarihi { get; set; }
    public DateTime? GuncellemeTarihi { get; set; }
    public bool Aktif { get; set; }
} 