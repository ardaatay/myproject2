using Business.Abstract;
using Core.Security;
using Dto.ActiveDirectory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

/// <summary>
/// Kiracının Active Directory bağlantı ayarları. Hangi kullanıcının dizin
/// üzerinden giriş yapacağı burada değil, kullanıcı kaydında belirlenir
/// (bkz. <see cref="KullanicilarController"/>).
/// </summary>
[Authorize(Policy = Yetkiler.SistemYonet)]
public class ActiveDirectoryController(
    IActiveDirectoryAyarService ayarService,
    IActiveDirectoryService activeDirectoryService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var dto = await ayarService.GetirAsync();
        ViewBag.DizinKullanicisiSayisi = await ayarService.DizinKullanicisiSayisiAsync();

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ActiveDirectoryAyarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.DizinKullanicisiSayisi = await ayarService.DizinKullanicisiSayisiAsync();
            return View(dto);
        }

        await ayarService.KaydetAsync(dto);

        TempData["SuccessMessage"] = "Active Directory ayarları kaydedildi.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Formdaki değerlerle bağlantıyı sınar. Kaydetmeden önce çalıştırılabildiği
    /// için doğrulama sonucuna bakılmaz: eksik alanlar zaten sınama mesajında
    /// bildirilir.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Test(
        ActiveDirectoryAyarDto dto,
        string? testKullaniciAdi,
        string? testSifre,
        CancellationToken cancellationToken)
    {
        var ayar = await ayarService.SinamaAyariUretAsync(dto);

        var sonuc = await activeDirectoryService.BaglantiTestEtAsync(
            ayar, testKullaniciAdi, testSifre, cancellationToken);

        // Yanıtta yalnızca sonuç ve dizinden okunan profil alanları döner;
        // hiçbir kimlik bilgisi geri gönderilmez.
        return Json(new
        {
            basarili = sonuc.Basarili,
            mesaj = sonuc.Mesaj,
            kullaniciAdi = sonuc.Kullanici?.KullaniciAdi,
            adSoyad = sonuc.Kullanici?.AdSoyad,
            eposta = sonuc.Kullanici?.Eposta,
            dn = sonuc.Kullanici?.DistinguishedName
        });
    }
}
