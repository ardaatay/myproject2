using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;

namespace Web.Middleware
{
    public class BirimSecMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            // Kullanıcı giriş yapmış mı kontrol et
            if (context.User.Identity?.IsAuthenticated == true)
            {
                // BirimId claim'ini al
                var birimIdClaim = context.User.FindFirst("BirimId");

                // Muaf tutulacak yollar. Şifre değiştirme de muaftır: şifresini
                // değiştirmesi zorunlu bir kullanıcı henüz birim seçemediyse
                // iki yönlendirme birbirini tetikleyip döngü oluştururdu.
                var exemptPaths = new[]
                {
                    "/Account/Login",
                    "/Account/BirimSec",
                    "/Account/SifreDegistir",
                    "/Account/AccessDenied",
                    "/Account/Logout"
                };

                // BirimId claim'i yoksa veya değeri 0 ise ve istek muaf yollara değilse
                if ((birimIdClaim == null || birimIdClaim.Value == "0") &&
                    !exemptPaths.Any(path => context.Request.Path.StartsWithSegments(path)))
                {
                    // BirimSec sayfasına yönlendir
                    context.Response.Redirect("/Account/BirimSec");
                    return;
                }
            }

            // Sonraki middleware'e geç
            await next(context);
        }
    }

    // Extension metodu
    public static class BirimSecMiddlewareExtensions
    {
        public static IApplicationBuilder UseBirimSecMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BirimSecMiddleware>();
        }
    }
}