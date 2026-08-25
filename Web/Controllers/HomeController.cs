using System.Diagnostics;
using Business.Abstract;
using Dto.AgveSistem;
using Dto.Rapor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Repository.Context;
using Web.Models;
using Web.Services;

namespace Web.Controllers;

[Authorize]
public class HomeController(
    IWebHostEnvironment environment,
    VarlikEnvanteriDbContext context,
    IAgveSistemService agveSistemService,
    IUygulamaService uygulamaService,
    ITasinabilirCihazveOrtamService tasinabilirCihazveOrtamService,
    IIoTCihazService ioTCihazService,
    IFizikselMekanService fizikselMekanService,
    IPersonelService personelService,
    KullaniciIstatistikService istatistikService) : Controller
{
    public async Task<IActionResult> Index()
    {
        // Varlık sayılarını veritabanından çek
        ViewBag.AgveSistemlerSayisi = context.AgveSistemler.Count(x => (x.SilinsinMi == false || x.SilinsinMi == null));
        ViewBag.UygulamalarSayisi = context.Uygulamalar.Count(x => (x.SilinsinMi == false || x.SilinsinMi == null));
        ViewBag.TasinabilirCihazlarSayisi = context.TasinabilirCihazveOrtamlar.Count(x => (x.SilinsinMi == false || x.SilinsinMi == null));
        ViewBag.IoTCihazlariSayisi = context.IoTCihazlari.Count(x => (x.SilinsinMi == false || x.SilinsinMi == null));
        ViewBag.FizikselMekanlarSayisi = context.FizikselMekanlar.Count(x => (x.SilinsinMi == false || x.SilinsinMi == null));
        ViewBag.PersonelSayisi = context.Personeller.Count(x => (x.SilinsinMi == false || x.SilinsinMi == null));

        // Son eklenen varlıkları getir (örnek olarak)
        // Bu kısmı kendi veri modelinize göre düzenlemeniz gerekecek
        ViewBag.SonEklenenVarliklar = await GetSonEklenenVarliklar();

        ViewBag.AktifKullaniciSayisi = istatistikService.GetAktifKullaniciSayisi();
        ViewBag.ToplamGirisSayisi = istatistikService.GetToplamGirisSayisi();

        return View();
    }

    private async Task<List<RaporAnasayfa>> GetSonEklenenVarliklar()
    {
        // Bu örnek metot, son eklenen varlıkları temsil eden bir liste döndürür
        // Gerçek uygulamada, veritabanından son eklenen varlıkları çekmeniz gerekir
        var sonEklenenler = new List<RaporAnasayfa>();

        // Burada veritabanından son eklenen varlıkları çekip listeye ekleyin
        // Örnek:
        var agSistemler = await agveSistemService.RaporAsync();
        sonEklenenler.AddRange(agSistemler);
        var uygulamalar = await uygulamaService.RaporAsync();
        sonEklenenler.AddRange(uygulamalar);
        var tasinabilirCihazlar = await tasinabilirCihazveOrtamService.RaporAsync();
        sonEklenenler.AddRange(tasinabilirCihazlar);
        var ioTCihazlari = await ioTCihazService.RaporAsync();
        sonEklenenler.AddRange(ioTCihazlari);
        var fizikselMekanlar = await fizikselMekanService.RaporAsync();
        sonEklenenler.AddRange(fizikselMekanlar);
        var personeller = await personelService.RaporAsync();
        sonEklenenler.AddRange(personeller);

        // Tüm kategorilerden son eklenen varlıkları alıp birleştirin ve tarihe göre sıralayın
        // sonEklenenler = sonEklenenler.OrderByDescending(v => v.EklenmeTarihi).Take(5).ToList();

        return sonEklenenler.OrderByDescending(x => x.EklenmeTarihi).ToList();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string? message = null, int? statusCode = null)
    {
        // Query string'den gelen parametreleri kontrol et
        if (!string.IsNullOrEmpty(message))
        {
            TempData["ErrorMessage"] = message;
        }

        if (statusCode.HasValue)
        {
            TempData["ErrorStatusCode"] = statusCode.Value;
        }

        // Eğer TempData'da zaten hata mesajı varsa onu kullan
        ViewBag.ErrorMessage = TempData["ErrorMessage"]?.ToString();
        ViewBag.ErrorStatusCode = TempData["ErrorStatusCode"]?.ToString();

        // İstisna bilgilerini al
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        // Eğer mesaj boşsa ve istisna varsa, istisna mesajını kullan
        if (string.IsNullOrEmpty(message) && exception != null)
        {
            message = exception.Message;
        }

        var errorViewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            ErrorMessage = message,
            StatusCode = statusCode ?? 500,
            // Sadece geliştirme ortamında stack trace'i göster
            StackTrace = environment.IsDevelopment() ? exception?.StackTrace : null
        };

        return View(errorViewModel);
    }
}
