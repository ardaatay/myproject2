using Business.Abstract;
using Core.Security;
using Dto.Organizasyon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

/// <summary>
/// Kurulumun kendi kurum kaydı: uygulamayı kullanan kurumun adı, kısa kodu ve
/// logosu. Başlık ve giriş ekranı bu kayıttan beslenir.
///
/// Menüdeki <c>İlgili Kurumlar</c> ekranıyla karıştırılmamalıdır — o, e-posta
/// taleplerinde referans verilen ve üçüncü tarafları da içerebilen bir listedir.
/// </summary>
[Authorize(Policy = Yetkiler.SistemYonet)]
public class KurumBilgileriController(IKurumBilgileriService kurumBilgileriService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index() => View(await kurumBilgileriService.GetirAsync());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(KurumBilgileriDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await kurumBilgileriService.GuncelleAsync(dto);

        TempData["SuccessMessage"] = "Kurum bilgileri güncellendi.";
        return RedirectToAction(nameof(Index));
    }
}
