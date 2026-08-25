using System.Security.Claims;

namespace Web.Extensions
{
    public static class UserExtensions
    {
        public static bool HasReadPermission(this ClaimsPrincipal user)
        {
            return user.IsInRole("StandaRtokuma") ||
                   user.IsInRole("BSDOkuma") ||
                   user.IsInRole("ADMIN");
        }

        public static bool HasEditPermission(this ClaimsPrincipal user)
        {
            return user.IsInRole("StandartYazma") ||
                   user.IsInRole("BSDYazma") ||
                   user.IsInRole("ADMIN");
        }

        public static bool HasBsdReadPermission(this ClaimsPrincipal user)
        {
            return user.IsInRole("BSDOkuma") ||
                   user.IsInRole("ADMIN");
        }

        public static bool HasBsdEditPermission(this ClaimsPrincipal user)
        {
            return user.IsInRole("BSDYazma") ||
                   user.IsInRole("ADMIN");
        }
    }
}