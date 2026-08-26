using Business.Abstract;
using Core.Logging;
using Core.Configuration;
using Core.Util;
using Dto.Kullanici;
using Dto.Kullanici.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Web.Services;

namespace Web.Controllers
{
    public class AccountController(
        IKimlikDogrulamaService kimlikDogrulamaService,
        ILogService logService,
        IIstekBaglami istekBaglami,
        KullaniciIstatistikService istatistikService,
        IKullaniciBirimService kullaniciBirimService,
        IKullaniciService kullaniciService,
        IOptions<UygulamaAyarlari> uygulamaAyarlari) : Controller
    {
        /// <summary>Oturum çerezinde tutulan damga; şifre değişince oturumu düşürmek için.</summary>
        public const string SecurityStampClaim = "SecurityStamp";

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null, string? error = null, string? kod = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            // Giriş sırasında oluşan bir hatadan geri dönülmüş olabilir; mesaj
            // ve referans kodu kullanıcıya burada gösterilir.
            if (!string.IsNullOrWhiteSpace(error))
            {
                ModelState.AddModelError(string.Empty, error);
                ViewBag.HataKodu = kod;
            }

            return View(new GirisDto { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(GirisDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var sonuc = await kimlikDogrulamaService.GirisYapAsync(dto.Username.Trim(), dto.Sifre);

            if (sonuc.Durum != GirisDurumu.Basarili)
            {
                ModelState.AddModelError(string.Empty, sonuc.Mesaj ?? "Giriş yapılamadı.");
                return View(new GirisDto { Username = dto.Username, ReturnUrl = dto.ReturnUrl });
            }

            var kullanici = sonuc.Kullanici!;
            await OturumAcAsync(kullanici, sonuc.Roller, sonuc.SecurityStamp);

            istatistikService.KullaniciGirisYapti(kullanici.Username);

            // Yönetici sıfırlaması veya ilk giriş: başka hiçbir sayfaya
            // geçmeden şifre değiştirilmelidir.
            if (sonuc.SifreDegistirmeliMi)
            {
                TempData["Uyari"] = "Devam etmeden önce şifrenizi değiştirmelisiniz.";
                return RedirectToAction(nameof(SifreDegistir));
            }

            if (!string.IsNullOrEmpty(dto.ReturnUrl) && Url.IsLocalUrl(dto.ReturnUrl))
                return Redirect(dto.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (!string.IsNullOrEmpty(User.Identity?.Name))
            {
                istatistikService.KullaniciCikisYapti(User.Identity.Name);

                logService.OturumOlayiEkle(
                    OturumOlaylari.Cikis,
                    User.Identity.Name,
                    istekBaglami.OrganizasyonId,
                    basarili: true,
                    "Oturum kapatıldı.");
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() => View();

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SifreDegistir()
        {
            var kullaniciId = KullaniciIdAl();
            if (kullaniciId is null)
                return RedirectToAction(nameof(Logout));

            // Dizine bağlı hesabın şifresi uygulamada tutulmadığı için burada
            // değiştirilemez; kullanıcı boş yere form doldurmasın.
            var kullanici = await kullaniciService.GetByIdAsync(kullaniciId.Value);

            if (kullanici?.GirisYontemi == GirisYontemi.ActiveDirectory)
            {
                TempData["ErrorMessage"] =
                    "Şifreniz Active Directory üzerinde yönetiliyor ve buradan değiştirilemez.";

                return RedirectToAction("Index", "Home");
            }

            return View(new SifreDegistirDto());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SifreDegistir(SifreDegistirDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var kullaniciId = KullaniciIdAl();
            if (kullaniciId is null)
                return RedirectToAction(nameof(Logout));

            var (basarili, hatalar, yeniDamga) = await kimlikDogrulamaService.SifreDegistirAsync(
                kullaniciId.Value, dto.MevcutSifre, dto.YeniSifre);

            if (!basarili)
            {
                foreach (var hata in hatalar)
                    ModelState.AddModelError(string.Empty, hata);

                return View(dto);
            }

            // Damga değiştiği için mevcut çerez artık geçersiz; oturum,
            // yenilenen damgayla yeniden kurulur.
            await DamgayiTazeleAsync(yeniDamga);

            TempData["SuccessMessage"] = "Şifreniz güncellendi.";
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> BirimSec()
        {
            var kullaniciId = KullaniciIdAl();
            if (kullaniciId is null)
                return RedirectToAction(nameof(AccessDenied));

            var birimler = await kullaniciBirimService.GetByKullaniciIdAsync(kullaniciId.Value);
            return View(birimler);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BirimSec(int id)
        {
            var aktifBirim = await kullaniciBirimService.GetByIdAsync(id);
            if (aktifBirim == null)
                return NotFound();

            var mevcutClaims = User.Claims
                .Where(c => c.Type is not ("BirimId" or "BirimAdi"))
                .Select(c => new Claim(c.Type, c.Value))
                .ToList();

            mevcutClaims.Add(new Claim("BirimId", aktifBirim.BirimId.ToString()));
            mevcutClaims.Add(new Claim("BirimAdi", TextUtil.ToTitleCase(aktifBirim.BirimAd ?? "")));

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await CerezYazAsync(mevcutClaims);

            var kullanici = await kullaniciService.GetByIdAsync(aktifBirim.KullaniciId);
            kullanici.BirimAd = aktifBirim.BirimAd ?? "";
            kullanici.BirimId = aktifBirim.BirimId;
            await kullaniciService.UpdateAsync(kullanici);

            return RedirectToAction("Index", "Home");
        }

        private int? KullaniciIdAl()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.TryParse(claim?.Value, out var id) ? id : null;
        }

        private async Task OturumAcAsync(ListKullaniciDto kullanici, List<string> roller, string? securityStamp)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, kullanici.Username),
                new(ClaimTypes.NameIdentifier, kullanici.Id.ToString()),
                new("BirimId", kullanici.BirimId.ToString()),
                new("BirimAdi", TextUtil.ToTitleCase(kullanici.BirimAd ?? "")),
                new(AktifOrganizasyon.ClaimTuru, kullanici.OrganizasyonId.ToString()),
                new(SecurityStampClaim, securityStamp ?? string.Empty)
            };

            claims.AddRange(roller.Select(rol => new Claim(ClaimTypes.Role, rol)));

            await CerezYazAsync(claims);
        }

        private async Task DamgayiTazeleAsync(string? yeniDamga)
        {
            var claims = User.Claims
                .Where(c => c.Type != SecurityStampClaim)
                .Select(c => new Claim(c.Type, c.Value))
                .ToList();

            claims.Add(new Claim(SecurityStampClaim, yeniDamga ?? string.Empty));

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await CerezYazAsync(claims);
        }

        private async Task CerezYazAsync(IEnumerable<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(uygulamaAyarlari.Value.OturumSuresiDk),
                    AllowRefresh = true
                });
        }
    }
}
