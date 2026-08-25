using Core.Security;
using Business.Abstract;
using Dto.Erisilebilirlik;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class ErisilebilirliklerController(IErisilebilirlikService erisilebilirlikService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var erisilebilirlikler = await erisilebilirlikService.GetAllAsync();
            return View(erisilebilirlikler);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateErisilebilirlikDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            await erisilebilirlikService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await erisilebilirlikService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateErisilebilirlikDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await erisilebilirlikService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await erisilebilirlikService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
