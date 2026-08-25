using AutoMapper;
using Business.Abstract;
using Business.Configuration;
using Business.Mapping;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Core.Configuration;
using Core.Security;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;
using Repository.Context;
using System.Globalization;
using System.Security.Claims;
using Web.Controllers;
using Web.Extensions;
using Web.Middleware;
using Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Kestrel Header Limit Arttırma (HTTP 400 Hatası için)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestHeadersTotalSize = 65536; // 64KB
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// Kuruma göre değişen ayarlar. Yapılandırma sırasında değerlere doğrudan
// ihtiyaç duyulduğu için bir kez okunup nesneye bağlanır.
var ayarlarBolumu = builder.Configuration.GetSection(UygulamaAyarlari.BolumAdi);
var ayarlar = ayarlarBolumu.Get<UygulamaAyarlari>() ?? new UygulamaAyarlari();

builder.Services.Configure<UygulamaAyarlari>(ayarlarBolumu);
builder.Services.Configure<SifrePolitikasi>(ayarlarBolumu.GetSection(nameof(UygulamaAyarlari.SifrePolitikasi)));

// Kültür bilgisini ayarla
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var kultur = (CultureInfo)new CultureInfo(ayarlar.Kultur).Clone();

    if (!string.IsNullOrWhiteSpace(ayarlar.TarihFormati))
        kultur.DateTimeFormat.ShortDatePattern = ayarlar.TarihFormati;

    options.DefaultRequestCulture = new RequestCulture(kultur);
    options.SupportedCultures = [kultur];
    options.SupportedUICultures = [kultur];
});

// DbContext'i servis olarak ekle. Bağlantı dizesi ConnectionStrings bölümünden
// ya da barındırıcının verdiği DATABASE_URL'den çözülür.
builder.Services.AddRepositoryExt(VeritabaniBaglantisi.Coz(builder.Configuration));
builder.Services.AddBusinessExt();
builder.Services.AddSingleton<KullaniciIstatistikService>();
builder.Services.AddScoped<IAktifOrganizasyon, AktifOrganizasyon>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(typeof(MappingProfile));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Kullanıcı giriı yapmadüçünda yınlendirilecek sayfa
        options.AccessDeniedPath = "/Account/AccessDenied"; // Yetkilendirme hatası durumunda yınlendirilecek sayfa
        options.Cookie.Name = "VarlikEnvanteriAuth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(ayarlar.OturumSuresiDk);
        options.SlidingExpiration = true;

        // Çerezdeki damga veritabanındakiyle eşleşmiyorsa oturum düşürülür.
        // Şifre değiştiğinde açık kalan diğer oturumlar böyle geçersizleşir.
        options.Events.OnValidatePrincipal = async ctx =>
        {
            var kullaniciId = ctx.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var damga = ctx.Principal?.FindFirst(AccountController.SecurityStampClaim)?.Value;

            if (!int.TryParse(kullaniciId, out var id))
            {
                ctx.RejectPrincipal();
                return;
            }

            var kimlik = ctx.HttpContext.RequestServices.GetRequiredService<IKimlikDogrulamaService>();

            if (!await kimlik.SecurityStampGecerliMiAsync(id, damga))
            {
                ctx.RejectPrincipal();
                await ctx.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        };
    });

// Anahtarlar kalıcı bir dizine yazılmazsa konteyner her yeniden oluşturulduğunda
// tüm oturum çerezleri çözülemez hale gelir ve kullanıcılar dışarı atılır.
if (!string.IsNullOrWhiteSpace(ayarlar.VeriDizini))
{
    var anahtarDizini = Path.Combine(ayarlar.VeriDizini, "DataProtection-Keys");
    Directory.CreateDirectory(anahtarDizini);

    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(anahtarDizini))
        .SetApplicationName("VarlikEnvanteri");
}

builder.Services.AddYetkilendirmeExt(builder.Configuration);

// Konteyner orkestrasyonunun uygulamanın hazır olup olmadığını anlaması için.
builder.Services.AddHealthChecks()
    .AddDbContextCheck<VarlikEnvanteriDbContext>("veritabani");

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(ayarlar.OturumSuresiDk);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Temiz kurulumda ve konteyner ortamında şemayı uygulamaya kendisi kurar.
// Üretimde kapalı tutulup migration'lar dağıtım adımında elle çalıştırılabilir.
var migrateEt = app.Configuration.GetValue<bool>("Veritabani:BaslangictaMigrateEt");
var gorunumleriUygula = app.Configuration.GetValue<bool>("Veritabani:BaslangictaGorunumleriUygula");

if (migrateEt || gorunumleriUygula)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<VarlikEnvanteriDbContext>();

    if (migrateEt)
        await db.Database.MigrateAsync();

    // Görünümler tablolara dayandığı için migration'lardan sonra uygulanır.
    if (gorunumleriUygula)
        await VeritabaniGorunumleri.UygulaAsync(db);
}

// Hiç kullanıcı yoksa sisteme girilemez; roller ve bir yönetici hesabı kurulur.
if (app.Configuration.GetValue<bool>("Veritabani:BaslangicVerisiniKur"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<VarlikEnvanteriDbContext>();
    var sifreKoruyucu = scope.ServiceProvider.GetRequiredService<ISifreKoruyucu>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    var yoneticiAdi = app.Configuration.GetValue("Veritabani:YoneticiKullaniciAdi", "admin")!;
    var yoneticiSifresi = app.Configuration.GetValue<string?>("Veritabani:YoneticiSifresi");

    var uretilenSifre = await BaslangicVerisi.UygulaAsync(db, sifreKoruyucu, yoneticiAdi, yoneticiSifresi);

    if (uretilenSifre is not null && string.IsNullOrWhiteSpace(yoneticiSifresi))
    {
        logger.LogWarning(
            "Yönetici hesabı oluşturuldu. Kullanıcı adı: {KullaniciAdi} — geçici şifre: {Sifre} " +
            "Bu şifre yalnızca bir kez gösterilir ve ilk girişte değiştirilmesi zorunludur.",
            yoneticiAdi, uretilenSifre);
    }
}

// Permissions-Policy Header - Custom Middleware
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Permissions-Policy", "geolocation=(), microphone=(), camera=()");
    await next();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Konteyner arkasında TLS'i ters vekil sonlandırır; uygulamanın kendisi yalnızca
// HTTP dinler. Böyle bir kurulumda yönlendirme her istekte uyarı üretir.
if (app.Configuration.GetValue("HttpsYonlendirmesiAcik", true))
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();

// Yapılandırılan kültürü isteğe uygular. Bu çağrı olmadan
// RequestLocalizationOptions hiçbir etki etmez ve iş parçacığı işletim
// sisteminin kültürüne düşer — konteynerde bu değişmez kültürdür, yani
// tarihler yanlış biçimlenir ve Türkçe harf büyütme bozulur.
app.UseRequestLocalization();

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// BirimSec middleware'ini ekleyin
app.UseBirimSecMiddleware();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<KullaniciIstatistikMiddleware>();

// Sağlık ucu kimlik doğrulaması gerektirmez; yalnızca ayakta olup olmadığını
// ve veritabanı bağlantısını bildirir, hiçbir veri sızdırmaz.
app.MapHealthChecks("/health").AllowAnonymous();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();