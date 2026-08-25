using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Web.Services;
using System;

namespace Web.Middleware
{
    public class KullaniciIstatistikMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly string[] _excludedPaths = new[] { "/css/", "/js/", "/lib/", "/images/", "/favicon.ico" };

        public KullaniciIstatistikMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, KullaniciIstatistikService istatistikService)
        {
            try
            {
                // Statik dosyaları ve API isteklerini kontrol etme
                string? path = context.Request.Path.Value?.ToLower();
                
                // Eğer bu bir statik dosya isteği ise veya hariç tutulan bir yol ise, işlemi atla
                if (path != null && _excludedPaths.Any(excluded => path.StartsWith(excluded, StringComparison.OrdinalIgnoreCase)))
                {
                    await _next(context);
                    return;
                }

                // Kullanıcı kimliğini al
                var kullaniciId = context.User?.Identity?.Name;
                
                if (!string.IsNullOrEmpty(kullaniciId))
                {
                    // Kullanıcı aktivitesini güncelle
                    istatistikService.KullaniciAktivitesiGuncelle(kullaniciId);
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama yapabilirsiniz
                // Ancak hatayı yutmayın, işleme devam edin
                System.Diagnostics.Debug.WriteLine($"KullaniciIstatistikMiddleware hatası: {ex.Message}");
            }

            // Her durumda sonraki middleware'e geç
            await _next(context);
        }
    }
}