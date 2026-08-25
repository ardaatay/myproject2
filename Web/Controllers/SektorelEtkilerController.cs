using Core.Security;
using Business.Abstract;
using Dto.SektorelEtki;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class SektorelEtkilerController(ISektorelEtkiService sektorelEtkiService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var sektorelEtkiler = await sektorelEtkiService.GetAllAsync();
            return View(sektorelEtkiler);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSektorelEtkiDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            await sektorelEtkiService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await sektorelEtkiService.GetByIdAsync(id);
            if (dto.Id == 0)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateSektorelEtkiDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            await sektorelEtkiService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await sektorelEtkiService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
