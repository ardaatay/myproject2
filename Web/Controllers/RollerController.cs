using Core.Security;
﻿using Business.Abstract;
using Dto.Rol;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class RollerController(IRolService rolService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var list = await rolService.GetAllAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            return View(new CreateRolDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRolDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            await rolService.AddAsync(dto);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await rolService.GetByIdAsync(id);
            if (dto.Id == 0)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateRolDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            await rolService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await rolService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
