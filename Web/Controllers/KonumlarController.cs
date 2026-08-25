using Core.Security;
using Business.Abstract;
using Dto.Konum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class KonumlarController(IKonumService konumService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var konumlar = await konumService.GetAllAsync();
            return View(konumlar);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateKonumDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            await konumService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await konumService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateKonumDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            await konumService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await konumService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
} 
