using Core.Security;
﻿using AutoMapper;
using Business.Abstract;
using Dto.DTOs;
using Dto.KriptografiEnvanteri;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Attributes;

namespace Web.Controllers
{
    [Authorize]
    public class KriptografiEnvanterleriController(
        IKriptografiEnvanteriService kriptografiEnvanteriService,
        IAnahtarSorumlusuService anahtarSorumlusuService,
        IKullanimSeviyesiService kullanimSeviyesiService,
        IBirimService birimService,
        IExcelService excelService,
        IMapper mapper)
        : Controller
    {
        [Authorize(Policy = Yetkiler.KriptoGoruntule)]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetKriptografiEnvanterleris()
        {
                // Form verilerini DataTablesRequest nesnesine dönüştür
                var request = new DataTablesRequest
                {
                    Draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                    Start = Convert.ToInt32(Request.Form["start"].FirstOrDefault()),
                    Length = Convert.ToInt32(Request.Form["length"].FirstOrDefault()),
                    Searchs = new DataTablesRequest.Search
                    {
                        Value = Request.Form["search[value]"].FirstOrDefault(),
                        Regex = Convert.ToBoolean(Request.Form["search[regex]"].FirstOrDefault())
                    },
                    Orders = new List<DataTablesRequest.Order>()
                };

                // Kolon bilgilerini doldur
                request.Columns = new List<DataTablesRequest.Column>();
                for (int i = 0; i < Request.Form.Keys.Count(k => k.StartsWith("columns")); i++)
                {
                    if (Request.Form.ContainsKey($"columns[{i}][data]"))
                    {
                        request.Columns.Add(new DataTablesRequest.Column
                        {
                            Data = Request.Form[$"columns[{i}][data]"].FirstOrDefault(),
                            Name = Request.Form[$"columns[{i}][name]"].FirstOrDefault(),
                            Searchable = Convert.ToBoolean(Request.Form[$"columns[{i}][searchable]"].FirstOrDefault()),
                            Orderable = Convert.ToBoolean(Request.Form[$"columns[{i}][orderable]"].FirstOrDefault()),
                            Search = new DataTablesRequest.Search
                            {
                                Value = Request.Form[$"columns[{i}][search][value]"].FirstOrDefault(),
                                Regex = Convert.ToBoolean(Request.Form[$"columns[{i}][search][regex]"].FirstOrDefault())
                            }
                        });
                    }
                }

                // Sıralama bilgilerini doldur
                for (int i = 0; i < Request.Form.Keys.Count(k => k.StartsWith("order")); i++)
                {
                    if (Request.Form.ContainsKey($"order[{i}][column]"))
                    {
                        request.Orders.Add(new DataTablesRequest.Order
                        {
                            Column = Convert.ToInt32(Request.Form[$"order[{i}][column]"].FirstOrDefault()),
                            Dir = Request.Form[$"order[{i}][dir]"].FirstOrDefault()
                        });
                    }
                }

                // Kolon adlarını veritabanı alan adlarıyla eşleştir

                var response = await kriptografiEnvanteriService.GetAllAsync(request);

                return Json(response);
        }

        [Authorize(Policy = Yetkiler.KriptoOlustur)]
        [GuvenlikModuKontrol]
        public async Task<IActionResult> Create()
        {
            await LoadSelectLists();
            return View();
        }

        [Authorize(Policy = Yetkiler.KriptoOlustur)]
        [HttpPost]
        [GuvenlikModuKontrol]
        public async Task<IActionResult> Create(CreateKriptografiEnvanteriDto dto)
        {
            if (ModelState.IsValid)
            {
                await kriptografiEnvanteriService.AddAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            // Geçersiz alanları bulma ve loglama
            var invalidFields = ModelState
                .Where(x => x.Value!.Errors.Count > 0)
                .Select(x => new
                {
                    Field = x.Key,
                    ErrorMessages = x.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                })
                .ToList();

            // Hata mesajlarını konsola yazdırma (geliştirme aşamasında)
            foreach (var field in invalidFields)
            {
                Console.WriteLine($"Geçersiz Alan: {field.Field}");
                foreach (var error in field.ErrorMessages)
                {
                    Console.WriteLine($" - Hata: {error}");
                }
            }

            // Hata mesajlarını TempData'ya ekleyerek view'da gösterebilirsiniz
            TempData["ValidationErrors"] = string.Join("<br>",
                invalidFields.SelectMany(f => f.ErrorMessages.Select(e => $"{f.Field}: {e}")));

            await LoadSelectLists();
            return View(dto);
        }

        [Authorize(Policy = Yetkiler.KriptoDuzenle)]
        [GuvenlikModuKontrol]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await kriptografiEnvanteriService.GetByIdAsync(id);
            if (dto.Id == 0)
                return NotFound();

            
            var birimId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value);

            if (User.IsInRole("ADMIN") || dto.VarlikSahibiId != birimId && User.IsInRole("OPOWNERS") || dto.VarlikSahibiId == birimId)
            {
                await LoadSelectLists(dto.VarlikSahibiId, dto.VarlikSahibi);
            }
            else
            {
                TempData["ErrorMessage"] = "Bu işlemi yapmaya yetkiniz yok!";
                TempData["ErrorStatusCode"] = "403";
                return RedirectToAction("Index");
            }

            return View(dto);
        }

        [Authorize(Policy = Yetkiler.KriptoDuzenle)]
        [HttpPost]
        [GuvenlikModuKontrol]
        public async Task<IActionResult> Edit(UpdateKriptografiEnvanteriDto dto)
        {
            if (ModelState.IsValid)
            {
                await kriptografiEnvanteriService.UpdateAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            await LoadSelectLists();
            return View(dto);
        }

        [Authorize(Policy = Yetkiler.KriptoGoruntule)]
        public async Task<IActionResult> Detail(int id)
        {
            var dto = await kriptografiEnvanteriService.GetByIdAsync(id);
            if (dto.Id == 0)
                return NotFound();

            await LoadSelectLists(dto.VarlikSahibiId, dto.VarlikSahibi);
            
            return View(dto);
        }

        [Authorize(Policy = Yetkiler.KriptoOlustur)]
        [GuvenlikModuKontrol]
        public async Task<IActionResult> Copy(int id)
        {
            var dto = await kriptografiEnvanteriService.GetByIdAsync(id);
            if (dto.Id == 0)
                return NotFound();

            var newDto = mapper.Map<CreateKriptografiEnvanteriDto>(dto);

            await LoadSelectLists(dto.VarlikSahibiId, dto.VarlikSahibi);

            return View("Create", newDto);
        }

        [Authorize(Policy = Yetkiler.KriptoOlustur)]
        [HttpPost]
        [GuvenlikModuKontrol]
        public async Task<IActionResult> Delete(int id)
        {
            await kriptografiEnvanteriService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        
        [Authorize(Policy = Yetkiler.SistemYonet)]
        [HttpPost]
        [GuvenlikModuKontrol]
        public async Task<IActionResult> DeleteDatabase(int id)
        {
            await kriptografiEnvanteriService.DeleteDatabaseAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadSelectLists(int? varlikSahibiId = null,
            string? varlikSahibi = null)
        {
            ViewBag.AnahtarSorumlulari = new SelectList(await anahtarSorumlusuService.GetAllAsync(), "Id", "Ad");
            ViewBag.KullanimSeviyeleri = new SelectList(await kullanimSeviyesiService.GetAllAsync(), "Id", "Ad");
            var birimId = varlikSahibiId ??
                          Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value);
            var birimAdi = varlikSahibi ?? User.Claims.FirstOrDefault(c => c.Type == "BirimAdi")?.Value;

            if (User.IsInRole("ADMIN"))
            {
                ViewBag.Birimler = new SelectList(await birimService.GetUstBirimlerAsync(), "Id", "Ad");
                ViewBag.AltBirimler = new SelectList(await birimService.GetAltBirimByParentIdAsync(birimId),
                    "Id", "Ad");
            }
            else if (User.IsInRole("VERIGIRIS"))
            {
                ViewBag.Birimler = new SelectList(await birimService.GetUstBirimlerAsync(), "Id", "Ad");
            }
            else
            {
                ViewBag.Birimler = new SelectList(new[]
                {
                    new { Id = birimId, Ad = birimAdi }
                }, "Id", "Ad");

                ViewBag.AltBirimler = new SelectList(await birimService.GetAltBirimByParentIdAsync(birimId),
                    "Id", "Ad");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DownloadExcel(string search)
        {
            var stream = await excelService.GenerateExcelKriptografiEnvanteri(search);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "KriptografiEnvanteri.xlsx");
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAltBirimler(int birimId)
        {
            var altBirimler = await birimService.GetAltBirimByParentIdAsync(birimId);
            return Json(new SelectList(altBirimler, "Id", "Ad"));
        }
    }
}
