using Core.Security;
using Business.Abstract;
using Dto.Gizlilik;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class GizliliklerController(IGizlilikService gizlilikService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var gizlilikler = await gizlilikService.GetAllAsync();
            return View(gizlilikler);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGizlilikDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            await gizlilikService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await gizlilikService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateGizlilikDto dto)
        {
            if (ModelState.IsValid)
            {
                await gizlilikService.UpdateAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await gizlilikService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
