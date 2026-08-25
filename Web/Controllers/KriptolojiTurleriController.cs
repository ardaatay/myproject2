using Core.Security;
using Business.Abstract;
using Dto.KriptolojiTuru;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class KriptolojiTurleriController(IKriptolojiTuruService kriptolojiTuruService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var kriptolojiTurleri = await kriptolojiTuruService.GetAllAsync();
            return View(kriptolojiTurleri);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateKriptolojiTuruDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            await kriptolojiTuruService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await kriptolojiTuruService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateKriptolojiTuruDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            await kriptolojiTuruService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await kriptolojiTuruService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
