using System.ComponentModel.DataAnnotations;

namespace Dto.Kullanici;

public class SifreDegistirDto
{
    [DataType(DataType.Password)]
    [Display(Name = "Mevcut şifre")]
    public string? MevcutSifre { get; set; }

    [Required(ErrorMessage = "Yeni şifre zorunludur.")]
    [DataType(DataType.Password)]
    [Display(Name = "Yeni şifre")]
    public string YeniSifre { get; set; } = null!;

    [Required(ErrorMessage = "Yeni şifre tekrarı zorunludur.")]
    [DataType(DataType.Password)]
    [Display(Name = "Yeni şifre (tekrar)")]
    [Compare(nameof(YeniSifre), ErrorMessage = "Şifreler eşleşmiyor.")]
    public string YeniSifreTekrar { get; set; } = null!;
}
