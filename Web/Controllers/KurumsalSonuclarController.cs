using Core.Security;
using Business.Abstract;
using Dto.KurumsalSonuc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class KurumsalSonuclarController(IKurumsalSonucService kurumsalSonucService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var kurumsalSonuclar = await kurumsalSonucService.GetAllAsync();
            return View(kurumsalSonuclar);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateKurumsalSonucDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            await kurumsalSonucService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await kurumsalSonucService.GetByIdAsync(id);
            if (dto.Id == 0)
                return NotFound();
        
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateKurumsalSonucDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            await kurumsalSonucService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await kurumsalSonucService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
