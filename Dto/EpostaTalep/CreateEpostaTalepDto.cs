using Microsoft.AspNetCore.Http;

namespace Dto.EpostaTalep;

public class CreateEpostaTalepDto
{
    public int KurumId { get; set; }
    public string? UcuncuTaraf { get; set; }
    public string? TalepEdilen { get; set; }
    public string? TalepEden { get; set; }
    public string? TalepNedeni { get; set; }
    public string? TalepSuresi { get; set; }
    public IFormFile? Dosya { get; set; }
    public string? DosyaYolu { get; set; }
}