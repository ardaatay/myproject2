namespace Dto.Kategori;

public class ListKategoriDto
{
    public int Id { get; set; }
    public string? Ad { get; set; }
    public int? UstKategoriId { get; set; }
    public bool Durum { get; set; }
    public CreateKategoriDto? UstKategori { get; set; }
}