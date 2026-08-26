using Business.Abstract;
using Core.Security;
using Dto.DTOs;
using Dto.Kullanici;
using Dto.Kullanici.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers
{
    [Authorize(Policy = Yetkiler.SistemYonet)]
    public class KullanicilarController(
        IKullaniciService kullaniciService,
        IKullaniciBirimService kullaniciBirimService,
        IBirimService orbisBirimService,
        IActiveDirectoryAyarService activeDirectoryAyarService,
        IKimlikDogrulamaService kimlikDogrulamaService)
        : Controller
    {
        public async Task<IActionResult> Index()
        {
            // Dizin girişi kapalıyken Active Directory seçili kullanıcılar giriş
            // yapamaz; liste ekranında bunu görünür kılmak için taşınır.
            ViewBag.DizinAktif = await DizinAktifMiAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetKullanicilar()
        {
                // Form verilerini DataTablesRequest nesnesine dönüştür
                var request = new DataTablesRequest
                {
                    Draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                    Start = Convert.ToInt32(Request.Form["start"].FirstOrDefault()),
                    Length = Convert.ToInt32(Request.Form["length"].FirstOrDefault()),
                    Searchs = new DataTablesRequest.Search
                    {
                        Value = Request.Form["search[value]"].FirstOrDefault() ?? "",
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
                            Data = Request.Form[$"columns[{i}][data]"].FirstOrDefault() ?? "",
                            Name = Request.Form[$"columns[{i}][name]"].FirstOrDefault() ?? "",
                            Searchable = Convert.ToBoolean(Request.Form[$"columns[{i}][searchable]"].FirstOrDefault()),
                            Orderable = Convert.ToBoolean(Request.Form[$"columns[{i}][orderable]"].FirstOrDefault()),
                            Search = new DataTablesRequest.Search
                            {
                                Value = Request.Form[$"columns[{i}][search][value]"].FirstOrDefault() ?? "",
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
                            Dir = Request.Form[$"order[{i}][dir]"].FirstOrDefault() ?? "asc"
                        });
                    }
                }

                // Kolon adlarını veritabanı alan adlarıyla eşleştir

                var response = await kullaniciBirimService.GetAllAsync(request);

                return Json(response);
        }

        public async Task<IActionResult> Create()
        {
            await LoadSelectLists();
            return View(new CreateKullaniciDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateKullaniciDto dto)
        {
            if (ModelState.IsValid)
            {
                await kullaniciService.AddAsync(dto);

                TempData["SuccessMessage"] = dto.GirisYontemi == GirisYontemi.ActiveDirectory
                    ? "Kullanıcı oluşturuldu. Şifresi Active Directory üzerinde yönetilir."
                    : "Kullanıcı oluşturuldu. Giriş yapabilmesi için listeden şifre sıfırlaması yapın.";

                return RedirectToAction(nameof(Index));
            }

            await LoadSelectLists();

            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await kullaniciService.DuzenlemeIcinGetirAsync(id);
            if (dto == null)
                return NotFound();

            await LoadSelectLists();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(KullaniciDuzenleDto dto)
        {
            if (ModelState.IsValid)
            {
                await kullaniciService.DuzenleAsync(dto);

                // Yöntem değiştiyse kullanıcının yerel şifresi ve açık oturumları
                // düşer; yöneticinin bunu bilmesi gerekir.
                if (dto.GirisYontemi != dto.MevcutGirisYontemi)
                {
                    TempData["SuccessMessage"] = dto.GirisYontemi == GirisYontemi.ActiveDirectory
                        ? "Kullanıcı Active Directory girişine geçirildi. Yerel şifresi silindi ve açık oturumları kapatıldı."
                        : "Kullanıcı yerel girişe geçirildi. Giriş yapabilmesi için şifre sıfırlaması yapın.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Kullanıcı güncellendi.";
                }

                return RedirectToAction(nameof(Index));
            }

            await LoadSelectLists();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await kullaniciBirimService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Şifresini unutan kullanıcı için yönetici sıfırlaması. E-posta
        /// altyapısı gerektirmez: üretilen tek kullanımlık şifre yöneticiye
        /// bir kez gösterilir, kullanıcı ilk girişte değiştirmek zorundadır.
        /// Dizine bağlı hesaplarda şifre uygulamada tutulmadığı için işlem reddedilir.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SifreSifirla(int id)
        {
            var sonuc = await kimlikDogrulamaService.SifreSifirlaAsync(id);

            if (!sonuc.Basarili)
            {
                TempData["ErrorMessage"] = sonuc.Hata ?? "Şifre sıfırlanamadı.";
                return RedirectToAction(nameof(Index));
            }

            TempData["GeciciSifre"] = sonuc.Sifre;
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadSelectLists()
        {
            ViewBag.Birimler = new SelectList(await orbisBirimService.GetUstBirimlerAsync(), "Id", "Ad");
            ViewBag.DizinAktif = await DizinAktifMiAsync();
        }

        /// <summary>
        /// Kiracıda dizin girişinin açık olup olmadığı. Kapalıysa formda uyarı
        /// gösterilir; seçim yine de yapılabilir, çünkü ayarlar sonradan
        /// tamamlanabilir.
        /// </summary>
        private async Task<bool> DizinAktifMiAsync()
        {
            var ayar = await activeDirectoryAyarService.GetirAsync();
            return ayar.Aktif && !string.IsNullOrWhiteSpace(ayar.Sunucu);
        }
    }
}
