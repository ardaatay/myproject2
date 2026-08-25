namespace Dto.Kategori;

public class UpdateKategoriDto
{
    public int Id { get; set; }
    public string? Ad { get; set; }
    public int? UstId { get; set; }
    public bool Durum { get; set; }
}