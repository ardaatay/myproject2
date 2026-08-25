using Core.Security;
﻿using Business.Abstract;
using Dto.Kullanici;
using Dto.KullaniciRol;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class KullaniciRollerController(
        IKullaniciService kullaniciService,
        IRolService rolService,
        IKullaniciRolService kullaniciRolService,
        IGuvenlikModuService guvenlikModuService)
        : Controller
    {
        public async Task<IActionResult> Index()
        {
            var kullanicilar = await kullaniciService.KullanicilariGetirAsync();
            return View(kullanicilar);
        }

        public async Task<IActionResult> Create()
        {
            await LoadSelectLists();
            return View(new CreateKullaniciRolDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateKullaniciRolDto dto)
        {
            if (ModelState.IsValid)
            {
                await kullaniciRolService.AddAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            await LoadSelectLists();

            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await kullaniciRolService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            await LoadSelectLists();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateKullaniciRolDto dto)
        {
            if (ModelState.IsValid)
            {
                await kullaniciRolService.UpdateAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            await LoadSelectLists();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await kullaniciRolService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadSelectLists()
        {
            ViewBag.Kullanicilar = new SelectList(await kullaniciService.GetAllAsync(), "Id", "Username");
            ViewBag.Roller = new SelectList(await rolService.GetAllAsync(), "Id", "Ad");
        }

        public async Task<IActionResult> RolAta(int id)
        {
            var model = await kullaniciRolService.KullaniciRolleriniGetirAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RolAta(KullaniciRolAtamaDto model)
        {
            if (ModelState.IsValid)
            {
                await rolService.RolleriKaydetAsync(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GuvenlikModuAyarla(bool aktif)
        {
            var durum = await guvenlikModuService.UpdateGuvenlikModu(aktif);
            return Json(new { success = durum });
        }

        [HttpGet]
        public async Task<IActionResult> GuvenlikModuDurumu()
        {
            var durum = await guvenlikModuService.GetGuvenlikModuDurumu();
            return Json(new { aktif = durum });
        }
    }
}
