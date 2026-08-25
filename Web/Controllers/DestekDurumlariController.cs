using Core.Security;
using Business.Abstract;
using Dto.DestekDurumu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class DestekDurumlariController(IDestekDurumuService destekDurumuService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var destekDurumlari = await destekDurumuService.GetAllAsync();
            return View(destekDurumlari);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDestekDurumuDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await destekDurumuService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await destekDurumuService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateDestekDurumuDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await destekDurumuService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await destekDurumuService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
