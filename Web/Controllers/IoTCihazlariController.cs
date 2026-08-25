using Core.Security;
﻿using AutoMapper;
using Business.Abstract;
using Dto.DTOs;
using Dto.IoTCihaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Util.Query;
using Web.Attributes;

namespace Web.Controllers
{
    [Authorize]
    public class IoTCihazlariController(
        IIoTCihazService ioTCihazService,
        IKategoriService kategoriService,
        IBilgiSinifiService bilgiSinifiService,
        IDurumService durumService,
        IGizlilikService gizlilikService,
        IButunlukService butunlukService,
        IErisilebilirlikService erisilebilirlikService,
        IEtkilenenKisiSayisiService etkilenenKisiSayisiService,
        IToplumsalSonucService toplumsalSonucService,
        IKurumsalSonucService kurumsalSonucService,
        ISektorelEtkiService sektorelEtkiService,
        IBagimliVarliklarService bagimliVarliklarService,
        IYedeklemeTipiService yedeklemeTipiService,
        IYedeklemeSorumlusuService yedeklemeSorumlusuService,
        IKriptolojiTuruService kriptolojiTuruService,
        IAnahtarSorumlusuService anahtarSorumlusuService,
        ILisansTakipSorumlusuService lisansTakipSorumlusuService,
        IBirimService birimService,
        IKonumService konumService,
        IExcelService excelService,
        IMapper mapper)
        : Controller
    {
        [Authorize(Policy = Yetkiler.TeknikVarlikListele)]
        public async Task<IActionResult> Index()
        {
            var birimId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value);
            var birimAdi = User.Claims.FirstOrDefault(c => c.Type == "BirimAdi")?.Value!;

            if (User.IsInRole("ADMIN"))
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

            ViewBag.Durumlar = new SelectList(await durumService.GetAllAsync(), "Id", "Ad");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetIoTCihazlaris(int varlikSahibi = 0, int varlikSahibiAltDepartman = 0, int durumId = 0)
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

            if(durumId != 0)
                request.FilterBag.WithFilter<ListIoTCihazDto>(x => x.DurumId == durumId);
            if (varlikSahibi != 0)
                request.FilterBag.WithFilter<ListIoTCihazDto>(x => x.VarlikSahibiId == varlikSahibi);
            if (varlikSahibiAltDepartman != 0)
                request.FilterBag.WithFilter<ListIoTCihazDto>(x => x.VarlikSahibiAltDepartmanId == varlikSahibiAltDepartman);


            var result = await ioTCihazService.GetAllAsync(request);

            return Json(result);
        }

        [Authorize(Policy = Yetkiler.TeknikVarlikOlustur)]
        [GuvenlikModuKontrol]
        public async Task<IActionResult> Create()
        {
            await LoadSelectLists();
            return View(new CreateIoTCihazDto());
        }

        [Authorize(Policy = Yetkiler.TeknikVarlikOlustur)]
        [HttpPost]
        [GuvenlikModuKontrol]
        public async Task<IActionResult> Create(CreateIoTCihazDto dto)
        {
            if (ModelState.IsValid)
            {
                await ioTCihazService.AddAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            await LoadSelectLists();
            return View(dto);
        }

        [Authorize(Policy = Yetkiler.TeknikVarlikDuzenle)]
        [GuvenlikModuKontrol]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await ioTCihazService.GetByIdAsync(id);
            if (dto.Id == 0)
                return NotFound();

            var birimId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value);

            if (User.IsInRole("ADMIN"))
            {
                await LoadSelectLists(dto.KategoriId, dto.VarlikSahibiId, dto.VarlikSahibi);
                return View(dto);
            }
            else if (dto.VarlikSahibiId != birimId && dto.OperasyonelSahibiId == birimId && User.IsInRole("OPOWNERS"))
            {
                await LoadSelectLists(dto.KategoriId, dto.VarlikSahibiId, dto.VarlikSahibi);
                return View(dto);
            }
            else if (dto.VarlikSahibiId == birimId)
            {
                await LoadSelectLists(dto.KategoriId, dto.VarlikSahibiId, dto.VarlikSahibi);
                return View(dto);
            }
            else
            {
                TempData["ErrorMessage"] = "Bu işlemi yapmaya yetkiniz yok!";
                TempData["ErrorStatusCode"] = "403";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Policy = Yetkiler.TeknikVarlikDuzenle)]
        [HttpPost]
        [GuvenlikModuKontrol]
        public async Task<IActionResult> Edit(UpdateIoTCihazDto dto)
        {
            if (ModelState.IsValid)
            {
                await ioTCihazService.UpdateAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            await LoadSelectLists();
            return View(dto);
        }

        [Authorize(Policy = Yetkiler.TeknikVarlikGoruntule)]
        public async Task<IActionResult> Detail(int id)
        {
            var dto = await ioTCihazService.GetByIdAsync(id);
            if (dto.Id == 0)
                return NotFound();

            await LoadSelectLists(dto.KategoriId, dto.VarlikSahibiId, dto.VarlikSahibi);
            return View(dto);
        }

        [Authorize(Policy = Yetkiler.TeknikVarlikOlustur)]
        [GuvenlikModuKontrol]
        public async Task<IActionResult> Copy(int id)
        {
            var dto = await ioTCihazService.GetByIdAsync(id);
            if (dto.Id == 0)
                return NotFound();

            var newDto = mapper.Map<CreateIoTCihazDto>(dto);

            await LoadSelectLists(newDto.KategoriId);

            return View("Create", newDto);
        }

        [Authorize(Policy = Yetkiler.TeknikVarlikOlustur)]
        [HttpPost]
        [GuvenlikModuKontrol]
        public async Task<IActionResult> Delete(int id)
        {
            await ioTCihazService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = Yetkiler.SistemYonet)]
        [HttpPost]
        [GuvenlikModuKontrol]
        public async Task<IActionResult> DeleteDatabase(int id)
        {
            await ioTCihazService.DeleteDatabaseAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadSelectLists(int? kategoriId = null, int? varlikSahibiId = null,
            string? varlikSahibi = null)
        {
            ViewBag.Kategoriler = new SelectList(await kategoriService.GetAllByUstIdAsync(4), "Id", "Ad");
            ViewBag.BilgiSiniflari = new SelectList(await bilgiSinifiService.GetAllAsync(), "Id", "Ad");
            ViewBag.Durumlar = new SelectList(await durumService.GetAllAsync(), "Id", "Ad");
            ViewBag.Gizlilikler = new SelectList(await gizlilikService.GetAllAsync(), "Id", "Ad");
            ViewBag.Butunlukler = new SelectList(await butunlukService.GetAllAsync(), "Id", "Ad");
            ViewBag.Erisilebilirlikler = new SelectList(await erisilebilirlikService.GetAllAsync(), "Id", "Ad");
            ViewBag.EtkilenenKisiSayilari = new SelectList(await etkilenenKisiSayisiService.GetAllAsync(), "Id", "Ad");
            ViewBag.ToplumsalSonuclar = new SelectList(await toplumsalSonucService.GetAllAsync(), "Id", "Ad");
            ViewBag.KurumsalSonuclar = new SelectList(await kurumsalSonucService.GetAllAsync(), "Id", "Ad");
            ViewBag.SektorelEtkiler = new SelectList(await sektorelEtkiService.GetAllAsync(), "Id", "Ad");
            ViewBag.BagimliVarliklar = new SelectList(await bagimliVarliklarService.GetAllAsync(), "Id", "Ad");
            ViewBag.YedeklemeTipleri = new SelectList(await yedeklemeTipiService.GetAllAsync(), "Id", "Ad");
            ViewBag.YedeklemeSorumlulari = new SelectList(await yedeklemeSorumlusuService.GetAllAsync(), "Id", "Ad");
            ViewBag.KriptolojiTurleri = new SelectList(await kriptolojiTuruService.GetAllAsync(), "Id", "Ad");
            ViewBag.AnahtarSorumlulari = new SelectList(await anahtarSorumlusuService.GetAllAsync(), "Id", "Ad");
            ViewBag.Konumlar = new SelectList(await konumService.GetAllAsync(), "Id", "Ad");
            ViewBag.LisansTakipSorumlulari =
                new SelectList(await lisansTakipSorumlusuService.GetAllAsync(), "Id", "Ad");

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

            ViewBag.AltKategoriler = kategoriId.HasValue
                ? new SelectList(await kategoriService.GetAllByUstIdAsync(kategoriId.Value), "Id", "Ad")
                : null;

            ViewBag.OperasyonelBirimler =
                new SelectList(await birimService.GetUstBirimlerAsync(), "Id", "Ad");
        }

        [HttpGet]
        public async Task<IActionResult> GetAltKategoriler(int kategoriId)
        {
            var kategoriler = await kategoriService.GetAllAsync();
            var altKategoriler = kategoriler.Where(k => k.UstKategoriId == kategoriId).ToList();
            return Json(new SelectList(altKategoriler, "Id", "Ad"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAltBirimler(int birimId)
        {
            var altBirimler = await birimService.GetAltBirimByParentIdAsync(birimId);
            return Json(new SelectList(altBirimler, "Id", "Ad"));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DownloadExcel(string search, int varlikSahibi, int varlikSahibiAltDepartman, int durumId)
        {
            FilterBag filterBag = new FilterBag();

            if (durumId != 0)
                filterBag.WithFilter<ListIoTCihazDto>(x => x.DurumId == durumId);
            if (varlikSahibi != 0)
                filterBag.WithFilter<ListIoTCihazDto>(x => x.VarlikSahibiId == varlikSahibi);
            if (varlikSahibiAltDepartman != 0)
                filterBag.WithFilter<ListIoTCihazDto>(x => x.VarlikSahibiAltDepartmanId == varlikSahibiAltDepartman);

            var stream = await excelService.GenerateExcelIoT(search, filterBag);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "IoTCihazlari.xlsx");
        }
    }
}
