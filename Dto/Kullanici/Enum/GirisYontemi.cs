using System.ComponentModel.DataAnnotations;

namespace Dto.Kullanici.Enum;

/// <summary>
/// Bir kullanıcının kimliğinin nerede doğrulanacağı. Kullanıcı bazında seçilir;
/// aynı kurumda yerel ve dizin hesapları bir arada bulunabilir.
/// </summary>
public enum GirisYontemi
{
    /// <summary>Şifre uygulamanın kendi veritabanında karma olarak tutulur.</summary>
    [Display(Name = "Yerel")]
    Yerel = 0,

    /// <summary>Şifre doğrulaması Active Directory'ye (LDAP) devredilir; uygulamada şifre saklanmaz.</summary>
    [Display(Name = "Active Directory")]
    ActiveDirectory = 1
}

public static class GirisYontemiUzantilari
{
    public static string Ad(this GirisYontemi yontem) => yontem switch
    {
        GirisYontemi.ActiveDirectory => "Active Directory",
        _ => "Yerel"
    };
}
