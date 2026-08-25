using Core.Security;
using Business.Abstract;
using Dto.DTOs;
using Dto.Raporlama;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Util.Query;

namespace Web.Controllers;

[Authorize(Policy = Yetkiler.SistemYonet)]
public class RaporlamaController(
    IKategoriService kategoriService,
    IBirimService birimService,
    IKonumService konumService,
    IRaporlamaService raporlamaService,
    IExcelService excelService,
    IDurumService durumService
) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.Kategoriler = new SelectList(await kategoriService.GetAllAsync(), "Id", "Ad");
        ViewBag.Birimler = new SelectList(await birimService.GetUstBirimlerAsync(), "Id", "Ad");
        ViewBag.Iller = new SelectList(await konumService.GetAllAsync(), "Id", "Ad");
        ViewBag.Durumlar = new SelectList(await durumService.GetAllAsync(), "Id", "Ad");
        return View();
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

    [HttpPost]
    public async Task<IActionResult> GetRaporlama(
        string? searchValue = null,
        int kategoriId = 0,
        int altKategoriId = 0,
        int varlikSahibi = 0,
        int varlikSahibiAltDepartman = 0,
        int sehirId = 0,
        int durumId = 0)
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

        if (!string.IsNullOrEmpty(searchValue))
            request.Searchs.Value = searchValue;

        // Kolon adlarını veritabanı alan adlarıyla eşleştir

        if (kategoriId != 0)
            request.FilterBag.WithFilter<ListRaporDto>(x => x.KategoriId == kategoriId);
        if (altKategoriId != 0)
            request.FilterBag.WithFilter<ListRaporDto>(x => x.AltKategoriId == altKategoriId);
        if (varlikSahibi != 0)
            request.FilterBag.WithFilter<ListRaporDto>(x => x.VarlikSahibiId == varlikSahibi);
        if (varlikSahibiAltDepartman != 0)
            request.FilterBag.WithFilter<ListRaporDto>(x => x.VarlikSahibiAltDepartmanId == varlikSahibiAltDepartman);
        if (sehirId != 0)
            request.FilterBag.WithFilter<ListRaporDto>(x => x.KonumId == sehirId);
        if (durumId != 0)
            request.FilterBag.WithFilter<ListRaporDto>(x => x.DurumId == durumId);


        var response = await raporlamaService.GetAllAsync(request);

        return Json(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> DownloadExcel(string? search = null,
        int kategoriId = 0,
        int altKategoriId = 0,
        int varlikSahibi = 0,
        int varlikSahibiAltDepartman = 0,
        int sehirId = 0,
        int durumId = 0)
    {

        FilterBag filterBag = new FilterBag();

        if (kategoriId != 0)
            filterBag.WithFilter<ListRaporDto>(x => x.KategoriId == kategoriId);
        if (altKategoriId != 0)
            filterBag.WithFilter<ListRaporDto>(x => x.AltKategoriId == altKategoriId);
        if (varlikSahibi != 0)
            filterBag.WithFilter<ListRaporDto>(x => x.VarlikSahibiId == varlikSahibi);
        if (varlikSahibiAltDepartman != 0)
            filterBag.WithFilter<ListRaporDto>(x => x.VarlikSahibiAltDepartmanId == varlikSahibiAltDepartman);
        if (sehirId != 0)
            filterBag.WithFilter<ListRaporDto>(x => x.KonumId == sehirId);
        if (durumId != 0)
            filterBag.WithFilter<ListRaporDto>(x => x.DurumId == durumId);


        var stream = await excelService.GenerateExcelRaporlama(search, filterBag);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Raporlama.xlsx");
    }
}
