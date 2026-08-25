namespace Dto.Birim;

public class ListBirimDto
{
    public int Id { get; set; }
    public int? UstId { get; set; }
    public string Ad { get; set; } = null!;
    public string? Kod { get; set; }
    public string UstAd { get; set; } = "";
    public int Seviye { get; set; }
    public int Sira { get; set; }
    public bool Durum { get; set; }
    public string DurumStr { get; set; } = "";

    /// <summary>Ağaç görünümünde girinti için: kökten bu birime kadar olan ad zinciri.</summary>
    public string TamYol { get; set; } = "";
}
