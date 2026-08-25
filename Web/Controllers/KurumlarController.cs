using Core.Security;
using Business.Abstract;
using Dto.AnahtarSorumlusu;
using Dto.Kurum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class KurumlarController(IKurumService kurumService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var kurumlar = await kurumService.GetAllAsync();
            return View(kurumlar);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateKurumDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await kurumService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await kurumService.GetByIdAsync(id);

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateKurumDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await kurumService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await kurumService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
