using Core.Security;
using Business.Abstract;
using Dto.KullanimSeviyesi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize(Policy = Yetkiler.SistemYonet)]
public class KullanimSeviyeleriController(IKullanimSeviyesiService kullanimSeviyesiService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var kullanimSeviyeleri = await kullanimSeviyesiService.GetAllAsync();
        return View(kullanimSeviyeleri);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateKullanimSeviyesiDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        
        await kullanimSeviyesiService.AddAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var kullanimSeviyesi = await kullanimSeviyesiService.GetByIdAsync(id);
        if (kullanimSeviyesi == null)
        {
            return NotFound();
        }
        return View(kullanimSeviyesi);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateKullanimSeviyesiDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        
        await kullanimSeviyesiService.UpdateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await kullanimSeviyesiService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
