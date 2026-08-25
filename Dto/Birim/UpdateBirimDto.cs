using System.ComponentModel.DataAnnotations;

namespace Dto.Birim;

public class UpdateBirimDto
{
    public int Id { get; set; }

    public int? UstId { get; set; }

    [Required(ErrorMessage = "Birim adı zorunludur.")]
    [MaxLength(500, ErrorMessage = "Birim adı en fazla 500 karakter olabilir.")]
    public string Ad { get; set; } = null!;

    [MaxLength(50, ErrorMessage = "Birim kodu en fazla 50 karakter olabilir.")]
    public string? Kod { get; set; }

    public int Sira { get; set; }
}
