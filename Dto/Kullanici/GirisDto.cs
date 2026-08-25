using System.ComponentModel.DataAnnotations;

namespace Dto.Kullanici;

public class GirisDto
{
    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [MaxLength(100)]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [DataType(DataType.Password)]
    public string Sifre { get; set; } = null!;

    public string? ReturnUrl { get; set; }
}
