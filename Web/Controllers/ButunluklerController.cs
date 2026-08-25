using Core.Security;
using Business.Abstract;
using Dto.Butunluk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class ButunluklerController(IButunlukService butunlukService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var butunluklar = await butunlukService.GetAllAsync();
            return View(butunluklar);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateButunlukDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await butunlukService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await butunlukService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateButunlukDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await butunlukService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await butunlukService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
