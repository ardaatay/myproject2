using Core.Security;
using Business.Abstract;
using Dto.EtkilenenKisiSayisi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class EtkilenenKisiSayilariController(IEtkilenenKisiSayisiService etkilenenKisiSayisiService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var etkilenenKisiSayilari = await etkilenenKisiSayisiService.GetAllAsync();
            return View(etkilenenKisiSayilari);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEtkilenenKisiSayisiDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await etkilenenKisiSayisiService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await etkilenenKisiSayisiService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateEtkilenenKisiSayisiDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await etkilenenKisiSayisiService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await etkilenenKisiSayisiService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
