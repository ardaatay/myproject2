using Core.Security;
using Business.Abstract;
using Dto.YedeklemeSorumlusu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize(Policy = Yetkiler.SistemYonet)]
public class YedeklemeSorumlulariController(IYedeklemeSorumlusuService yedeklemeSorumlusuService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var yedeklemeSorumlulari = await yedeklemeSorumlusuService.GetAllAsync();
        return View(yedeklemeSorumlulari);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateYedeklemeSorumlusuDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        
        await yedeklemeSorumlusuService.AddAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var yedeklemeSorumlusu = await yedeklemeSorumlusuService.GetByIdAsync(id);
        if (yedeklemeSorumlusu.Id==0)
            return NotFound();
        
        return View(yedeklemeSorumlusu);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateYedeklemeSorumlusuDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        
        await yedeklemeSorumlusuService.UpdateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await yedeklemeSorumlusuService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
