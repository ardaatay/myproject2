using Core.Security;
using Business.Abstract;
using Dto.Durum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class DurumlarController(IDurumService durumService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var durumlar = await durumService.GetAllAsync();
            return View(durumlar);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDurumDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await durumService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await durumService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateDurumDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await durumService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await durumService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
