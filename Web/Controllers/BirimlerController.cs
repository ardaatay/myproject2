using Core.Security;
using Business.Abstract;
using Dto.Birim;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class BirimlerController(IBirimService birimService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var agac = await birimService.GetAgacAsync();
            return View(agac);
        }

        public async Task<IActionResult> Create(int? ustId = null)
        {
            await UstBirimListesiHazirlaAsync();
            return View(new CreateBirimDto { UstId = ustId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBirimDto dto)
        {
            if (!ModelState.IsValid)
            {
                await UstBirimListesiHazirlaAsync();
                return View(dto);
            }

            await birimService.AddAsync(dto);
            TempData["SuccessMessage"] = "Birim eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await birimService.GetByIdAsync(id);
            if (dto is null)
                return NotFound();

            // Bir birim kendi alt ağacına taşınamaz; o dal listeden çıkarılır.
            await UstBirimListesiHazirlaAsync(id);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateBirimDto dto)
        {
            if (!ModelState.IsValid)
            {
                await UstBirimListesiHazirlaAsync(dto.Id);
                return View(dto);
            }

            await birimService.UpdateAsync(dto);
            TempData["SuccessMessage"] = "Birim güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await birimService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Varlık formlarındaki iki kademeli açılır listenin ikinci kademesini besler.</summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAltBirimler(int birimId)
        {
            var altBirimler = await birimService.GetAltBirimByParentIdAsync(birimId);
            return Json(new SelectList(altBirimler, "Id", "Ad"));
        }

        private async Task UstBirimListesiHazirlaAsync(int? haricId = null)
        {
            var secenekler = await birimService.GetUstBirimSecenekleriAsync(haricId);
            ViewBag.UstBirimler = new SelectList(secenekler, "Id", "Ad");
        }
    }
}
