using Business.Abstract;
using Core.Security;
using Dto.Loglar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers;

/// <summary>
/// İşlem logları: <c>[LogAspect]</c> ile işaretlenmiş iş katmanı çağrılarının
/// kaydı. Salt okunurdur — log düzenlenemez ya da silinemez, aksi halde
/// denetim değeri kalmaz.
/// </summary>
[Authorize(Policy = Yetkiler.SistemYonet)]
public class IslemLoglariController(ILogService logService) : Controller
{
    public IActionResult Index(LogFiltreDto filtre) => View(filtre);

    [HttpPost]
    public async Task<IActionResult> Liste(LogFiltreDto filtre)
    {
        var istek = Request.DataTablesIstegiOku();
        var yanit = await logService.IslemLoglariAsync(istek, filtre);

        return Json(yanit);
    }

    public async Task<IActionResult> Detay(int id)
    {
        var detay = await logService.IslemLogGetirAsync(id);

        if (detay is null)
            return NotFound();

        return View(detay);
    }
}
