using Core.Security;
using Business.Abstract;
using Dto.LisansTakipSorumlusu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize(Policy = Yetkiler.SistemYonet)]
public class LisansTakipSorumlulariController(ILisansTakipSorumlusuService lisansTakipSorumlusuService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var lisansTakipSorumlulari = await lisansTakipSorumlusuService.GetAllAsync();
        return View(lisansTakipSorumlulari);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateLisansTakipSorumlusuDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        await lisansTakipSorumlusuService.AddAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var lisansTakipSorumlusu = await lisansTakipSorumlusuService.GetByIdAsync(id);
        if (lisansTakipSorumlusu.Id == 0)
        {
            return NotFound();
        }

        return View(lisansTakipSorumlusu);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateLisansTakipSorumlusuDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        
        await lisansTakipSorumlusuService.UpdateAsync(dto);
        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Delete(int id)
    {
        await lisansTakipSorumlusuService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
