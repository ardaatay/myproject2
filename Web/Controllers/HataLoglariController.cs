using Business.Abstract;
using Core.Logging;
using Core.Security;
using Dto.Loglar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers;

/// <summary>
/// Hata logları. Ekranın asıl işi, kullanıcının bildirdiği kodu tam kayda
/// çevirmektir; bu yüzden kod araması listeden ayrı ve öndedir.
/// </summary>
[Authorize(Policy = Yetkiler.SistemYonet)]
public class HataLoglariController(ILogService logService) : Controller
{
    public async Task<IActionResult> Index(LogFiltreDto filtre)
    {
        var (toplam, cozulmemis, sonGun) = await logService.HataOzetiAsync();

        ViewBag.Toplam = toplam;
        ViewBag.Cozulmemis = cozulmemis;
        ViewBag.SonYirmiDortSaat = sonGun;

        return View(filtre);
    }

    [HttpPost]
    public async Task<IActionResult> Liste(LogFiltreDto filtre)
    {
        var istek = Request.DataTablesIstegiOku();
        var yanit = await logService.HataLoglariAsync(istek, filtre);

        return Json(yanit);
    }

    /// <summary>
    /// Kullanıcının ilettiği kodla arama. Kod büyük/küçük harf, tire ve boşluk
    /// farklarına duyarsızdır; ön ek yazılmasa da bulunur.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Ara(string? kod)
    {
        if (string.IsNullOrWhiteSpace(kod))
            return RedirectToAction(nameof(Index));

        if (HataKodu.Duzelt(kod) is null)
        {
            TempData["ErrorMessage"] =
                $"\"{kod}\" geçerli bir hata kodu değil. Kod {HataKodu.Onek}-XXXX-XXXX biçimindedir.";

            return RedirectToAction(nameof(Index));
        }

        var kayit = await logService.HataKoduIleGetirAsync(kod);

        if (kayit is null)
        {
            TempData["ErrorMessage"] = $"\"{kod}\" koduyla bir hata kaydı bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Detay), new { id = kayit.Id });
    }

    public async Task<IActionResult> Detay(int id)
    {
        var detay = await logService.HataLogGetirAsync(id);

        if (detay is null)
            return NotFound();

        return View(detay);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cozum(HataCozumDto dto)
    {
        var basarili = await logService.CozumIsaretleAsync(dto.Id, dto.Cozuldu, dto.Not);

        if (!basarili)
            return NotFound();

        TempData["SuccessMessage"] = dto.Cozuldu
            ? "Hata çözüldü olarak işaretlendi."
            : "Hata yeniden açık duruma alındı.";

        return RedirectToAction(nameof(Detay), new { id = dto.Id });
    }
}
