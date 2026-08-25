using Core.Security;
using Business.Abstract;
using Dto.AnahtarSorumlusu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class AnahtarSorumlulariController(IAnahtarSorumlusuService anahtarSorumlusuService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var anahtarSorumlulari = await anahtarSorumlusuService.GetAllAsync();
            return View(anahtarSorumlulari);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAnahtarSorumlusuDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            await anahtarSorumlusuService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await anahtarSorumlusuService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateAnahtarSorumlusuDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            await anahtarSorumlusuService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await anahtarSorumlusuService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
} 
