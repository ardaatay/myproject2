using Core.Security;
using Business.Abstract;
using Dto.YedeklemeTipi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize(Policy = Yetkiler.SistemYonet)]
public class YedeklemeTipleriController(IYedeklemeTipiService yedeklemeTipiService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var yedeklemeTipleri = await yedeklemeTipiService.GetAllAsync();
        return View(yedeklemeTipleri);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateYedeklemeTipiDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        
        await yedeklemeTipiService.AddAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var yedeklemeTipi = await yedeklemeTipiService.GetByIdAsync(id);
        if (yedeklemeTipi.Id==0)
            return NotFound();
        
        return View(yedeklemeTipi);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateYedeklemeTipiDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        
        await yedeklemeTipiService.UpdateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await yedeklemeTipiService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
