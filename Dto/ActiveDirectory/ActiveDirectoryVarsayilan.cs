namespace Dto.ActiveDirectory;

/// <summary>
/// Active Directory kurulumlarının ezici çoğunluğunda geçerli olan değerler.
/// Yönetici ekranda hepsini değiştirebilir; buradakiler yalnızca başlangıç noktasıdır.
/// </summary>
public static class ActiveDirectoryVarsayilan
{
    public const int Port = 389;
    public const int SslPort = 636;
    public const int ZamanAsimiSn = 10;

    public const string AramaFiltresi = "(&(objectCategory=person)(objectClass=user)(sAMAccountName={0}))";
    public const string KullaniciAdiOzniteligi = "sAMAccountName";
    public const string AdSoyadOzniteligi = "displayName";
    public const string EpostaOzniteligi = "mail";
}
