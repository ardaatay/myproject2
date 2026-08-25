using Core.Security;
using Business.Abstract;
using Dto.BagimliVarliklar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class BagimliVarliklarController(IBagimliVarliklarService bagimliVarliklarService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var bagimliVarliklar = await bagimliVarliklarService.GetAllAsync();
            return View(bagimliVarliklar);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBagimliVarliklarDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await bagimliVarliklarService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await bagimliVarliklarService.GetByIdAsync(id);

            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateBagimliVarliklarDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            await bagimliVarliklarService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await bagimliVarliklarService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
