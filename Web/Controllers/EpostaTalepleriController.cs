using Core.Security;
using AutoMapper;
using Business.Abstract;
using Dto.AgveSistem;
using Dto.DTOs;
using Dto.EpostaTalep;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Attributes;

namespace Web.Controllers;

[Authorize]
public class EpostaTalepleriController(
    IEpostaTalepService epostaTalepService,
    IKurumService kurumService,
    IExcelService excelService,
    IMapper mapper)
    : Controller
{
    [Authorize(Policy = Yetkiler.EpostaTalepListele)]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetEpostaTalepleri()
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

            var response = await epostaTalepService.GetAllAsync(request);

            return Json(response);
    }

    [Authorize(Policy = Yetkiler.EpostaTalepYonet)]
    [GuvenlikModuKontrol]
    public async Task<IActionResult> Create()
    {
        await LoadSelectLists();
        return View(new CreateEpostaTalepDto());
    }

    [Authorize(Policy = Yetkiler.EpostaTalepYonet)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(21 * 1024 * 1024)] // form overhead için 21MB
    [RequestFormLimits(MultipartBodyLengthLimit = 21 * 1024 * 1024)]
    [GuvenlikModuKontrol]
    public async Task<IActionResult> Create(CreateEpostaTalepDto dto)
    {
        if (ModelState.IsValid)
        {
            const long maxFileSize = 20 * 1024 * 1024; // 20MB
            var uploadRoot = Path.Combine("C:", "VarlikEnvanteri", "EpostaTalepleri");

            // 1) Temel kontroller
            if (dto.Dosya == null || dto.Dosya.Length == 0)
            {
                TempData["Error"] = "Dosya boş olamaz.";
                await LoadSelectLists();
                return View(dto);
            }

            if (!string.Equals(Path.GetExtension(dto.Dosya.FileName), ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Yalnızca PDF dosyaları yüklenebilir.";
                await LoadSelectLists();
                return View(dto);
            }

            if (!string.Equals(dto.Dosya.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Geçersiz içerik türü. Lütfen PDF yükleyin.";
                await LoadSelectLists();
                return View(dto);
            }

            if (dto.Dosya.Length > maxFileSize)
            {
                TempData["Error"] = "Dosya boyutu en fazla 20MB olmalıdır.";
                await LoadSelectLists();
                return View(dto);
            }

            try
            {
                // 2) PDF sihirli başlık kontrolü (%PDF)
                await using var readStream = dto.Dosya.OpenReadStream();
                var header = new byte[4];
                var read = await readStream.ReadAsync(header, 0, 4);
                var isPdfHeader = read == 4 && header[0] == 0x25 && header[1] == 0x50 && header[2] == 0x44 &&
                                  header[3] == 0x46; // %PDF

                if (!isPdfHeader)
                {
                    TempData["Error"] = "Geçersiz PDF dosyası.";
                    await LoadSelectLists();
                    return View(dto);
                }

                readStream.Position = 0; // Baştan yazmak için geri sar

                // 3) Klasörü oluştur ve dosyayı kaydet
                Directory.CreateDirectory(uploadRoot);

                var safeName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}.pdf";
                var savePath = Path.Combine(uploadRoot, safeName);

                await using (var target = System.IO.File.Create(savePath))
                {
                    await readStream.CopyToAsync(target);
                }

                TempData["Success"] = $"Dosya kaydedildi: {savePath}";
                
                dto.DosyaYolu = savePath;

                await epostaTalepService.AddAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Dosya kaydedilemedi: {ex.Message}";
                await LoadSelectLists();
                return View(dto);
            }
        }

        await LoadSelectLists();
        return View(dto);
    }

    [Authorize(Policy = Yetkiler.EpostaTalepYonet)]
    [GuvenlikModuKontrol]
    public async Task<IActionResult> Edit(int id)
    {
        var dto = await epostaTalepService.GetByIdAsync(id);
        await LoadSelectLists();

        return View(dto);
    }

    [Authorize(Policy = Yetkiler.EpostaTalepYonet)]
    [HttpPost]
    [GuvenlikModuKontrol]
    public async Task<IActionResult> Edit(UpdateEpostaTalepDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectLists();
            return View(dto);
        }

        const long maxFileSize = 20 * 1024 * 1024; // 20MB
        var uploadRoot = Path.Combine("E:", "VarlikEnvanteri", "EpostaTalepleri");

        // Eğer yeni dosya seçildiyse validasyon ve kayıt yap
        if (dto.Dosya != null && dto.Dosya.Length > 0)
        {
            if (!string.Equals(Path.GetExtension(dto.Dosya.FileName), ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Yalnızca PDF dosyaları yüklenebilir.";
                await LoadSelectLists();
                return View(dto);
            }

            if (!string.Equals(dto.Dosya.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Geçersiz içerik türü. Lütfen PDF yükleyin.";
                await LoadSelectLists();
                return View(dto);
            }

            if (dto.Dosya.Length > maxFileSize)
            {
                TempData["Error"] = "Dosya boyutu en fazla 20MB olmalıdır.";
                await LoadSelectLists();
                return View(dto);
            }

            try
            {
                await using var readStream = dto.Dosya.OpenReadStream();
                var header = new byte[4];
                var read = await readStream.ReadAsync(header, 0, 4);
                var isPdfHeader = read == 4 && header[0] == 0x25 && header[1] == 0x50 && header[2] == 0x44 && header[3] == 0x46; // %PDF
                if (!isPdfHeader)
                {
                    TempData["Error"] = "Geçersiz PDF dosyası.";
                    await LoadSelectLists();
                    return View(dto);
                }

                readStream.Position = 0;
                Directory.CreateDirectory(uploadRoot);
                var safeName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}.pdf";
                var savePath = Path.Combine(uploadRoot, safeName);

                await using (var target = System.IO.File.Create(savePath))
                {
                    await readStream.CopyToAsync(target);
                }

                dto.DosyaYolu = savePath; // yeni dosya yolu ata
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Dosya kaydedilemedi: {ex.Message}";
                await LoadSelectLists();
                return View(dto);
            }
        }
        else
        {
            // Yeni dosya seçilmedi: mevcut yol güvenli şekilde korunur (sunucudan okunur)
            var existing = await epostaTalepService.GetByIdAsync(dto.Id);
            if (existing != null)
            {
                dto.DosyaYolu = existing.DosyaYolu;
            }
        }

        await epostaTalepService.UpdateAsync(dto);
        TempData["Success"] = "Kayıt güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = Yetkiler.EpostaTalepYonet)]
    public async Task<IActionResult> Detail(int id)
    {
        var dto = await epostaTalepService.GetByIdAsync(id);
        if (dto.Id == 0)
            return NotFound();

        await LoadSelectLists();
        return View(dto);
    }

    [Authorize(Policy = Yetkiler.EpostaTalepYonet)]
    [GuvenlikModuKontrol]
    public async Task<IActionResult> Copy(int id)
    {
        var dto = await epostaTalepService.GetByIdAsync(id);
        if (dto.Id == 0)
            return NotFound();

        var newDto = mapper.Map<CreateEpostaTalepDto>(dto);

        await LoadSelectLists();

        return View("Create", newDto);
    }

    [Authorize(Policy = Yetkiler.EpostaTalepYonet)]
    [HttpPost]
    [GuvenlikModuKontrol]
    public async Task<IActionResult> Delete(int id)
    {
        await epostaTalepService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = Yetkiler.SistemYonet)]
    [HttpPost]
    [GuvenlikModuKontrol]
    public async Task<IActionResult> DeleteDatabase(int id)
    {
        await epostaTalepService.DeleteDatabaseAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadSelectLists()
    {
        var kurumlar = await kurumService.GetAllAsync();

        ViewBag.Kurumlar = kurumlar.Select(x => new SelectListItem { Text = x.Ad, Value = x.Id.ToString() }).ToList();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> DownloadExcel(string search, int varlikSahibi, int varlikSahibiAltDepartman)
    {
        var stream = await excelService.GenerateExcelEpostaTalepleri(search);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "EpostaTalepleri.xlsx");
    }

    [Authorize(Policy = Yetkiler.EpostaTalepYonet)]
    [HttpGet]
    [GuvenlikModuKontrol]
    public async Task<IActionResult> Download(int id)
    {
        try
        {
            var dto = await epostaTalepService.GetByIdAsync(id);
            if (dto == null || dto.Id == 0)
                return NotFound();

            if (string.IsNullOrWhiteSpace(dto.DosyaYolu))
            {
                TempData["Error"] = "Bu kayıtla ilişkili dosya bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var uploadRoot = Path.Combine("E:", "VarlikEnvanteri", "EpostaTalepleri");
            var fullPath = Path.GetFullPath(dto.DosyaYolu);
            var rootPath = Path.GetFullPath(uploadRoot + Path.DirectorySeparatorChar);

            // Yol güvenliği: yalnızca beklenen kökün altına izin ver
            if (!fullPath.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Dosya yolu geçersiz.";
                return RedirectToAction(nameof(Index));
            }

            if (!System.IO.File.Exists(fullPath))
            {
                TempData["Error"] = "Dosya bulunamadı veya silinmiş.";
                return RedirectToAction(nameof(Index));
            }

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);

            // PDF sihirli başlık doğrulama
            var header = new byte[4];
            var read = await stream.ReadAsync(header, 0, 4);
            var isPdf = read == 4 && header[0] == 0x25 && header[1] == 0x50 && header[2] == 0x44 && header[3] == 0x46; // %PDF
            if (!isPdf)
            {
                stream.Dispose();
                TempData["Error"] = "Dosya PDF formatında değil.";
                return RedirectToAction(nameof(Index));
            }
            stream.Position = 0; // Baştan başlat

            var downloadName = Path.GetFileName(fullPath);
            if (string.IsNullOrWhiteSpace(downloadName) || !downloadName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                downloadName = $"EpostaTalebi_{id}.pdf";

            return File(stream, "application/pdf", downloadName);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Dosya indirilemedi: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}
