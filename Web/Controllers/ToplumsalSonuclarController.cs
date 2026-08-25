using Core.Security;
using Business.Abstract;
using Dto.ToplumsalSonuc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class ToplumsalSonuclarController(IToplumsalSonucService toplumsalSonucService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var toplumsalSonuclar = await toplumsalSonucService.GetAllAsync();
            return View(toplumsalSonuclar);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateToplumsalSonucDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await toplumsalSonucService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await toplumsalSonucService.GetByIdAsync(id);
            if (dto.Id == 0)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateToplumsalSonucDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await toplumsalSonucService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await toplumsalSonucService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
