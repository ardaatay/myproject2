namespace Core.Security;

/// <summary>
/// Oturum çerezinde kiracıyı taşıyan claim'in adı. Tek bir yerde tutulur:
/// değer hem çerezi yazan tarafta hem de okuyan tarafta aynı olmalıdır.
/// </summary>
public static class KiraciClaim
{
    public const string OrganizasyonId = "OrganizasyonId";
}
