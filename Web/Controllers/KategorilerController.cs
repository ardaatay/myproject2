using Core.Security;
using Business.Abstract;
using Dto.Kategori;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class KategorilerController(IKategoriService kategoriService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var kategoriler = await kategoriService.GetAllAsync();
            return View(kategoriler);
        }

        public async Task<IActionResult> Create()
        {
            await LoadSelectLists();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateKategoriDto dto)
        {
            if (ModelState.IsValid)
            {
                await kategoriService.AddAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            await LoadSelectLists();
            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await kategoriService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            await LoadSelectLists();
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateKategoriDto dto)
        {
            if (ModelState.IsValid)
            {
                await kategoriService.UpdateAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            await LoadSelectLists();
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await kategoriService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadSelectLists()
        {
            var kategoriler = await kategoriService.GetAllAsync();
            ViewBag.UstKategoriler = new SelectList(kategoriler, "Id", "Ad");
        }
    }
}
