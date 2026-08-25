using Core.Security;
using Business.Abstract;
using Dto.BilgiSinifi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class BilgiSiniflariController(IBilgiSinifiService bilgiSinifiService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var bilgiSiniflari = await bilgiSinifiService.GetAllAsync();
            return View(bilgiSiniflari);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBilgiSinifiDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await bilgiSinifiService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await bilgiSinifiService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateBilgiSinifiDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await bilgiSinifiService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await bilgiSinifiService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
