using Business.Abstract;
using Business.Concrete;
using Business.DI;
using Core.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Configuration;

public static class BusinessExtensions
{
    public static IServiceCollection AddBusinessExt(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddProxiedScoped<IAgveSistemService, AgveSistemManager>();
        services.AddProxiedScoped<IAnahtarSorumlusuService, AnahtarSorumlusuManager>();
        services.AddProxiedScoped<IBagimliVarliklarService, BagimliVarliklarManager>();
        services.AddProxiedScoped<IBasiliBilgiService, BasiliBilgiManager>();
        services.AddProxiedScoped<IBilgiSinifiService, BilgiSinifiManager>();
        services.AddProxiedScoped<IButunlukService, ButunlukManager>();
        services.AddProxiedScoped<IDestekDurumuService, DestekDurumuManager>();
        services.AddProxiedScoped<IDurumService, DurumManager>();
        services.AddProxiedScoped<IElektronikBilgiService, ElektronikBilgiManager>();
        services.AddProxiedScoped<IErisilebilirlikService, ErisilebilirlikManager>();
        services.AddProxiedScoped<IEtkilenenKisiSayisiService, EtkilenenKisiSayisiManager>();
        services.AddProxiedScoped<IFizikselMekanService, FizikselMekanManager>();
        services.AddProxiedScoped<IGizlilikService, GizlilikManager>();
        services.AddProxiedScoped<IIoTCihazService, IoTCihazManager>();
        services.AddProxiedScoped<IKategoriService, KategoriManager>();
        services.AddProxiedScoped<IKonumService, KonumManager>();
        services.AddProxiedScoped<IKriptografiEnvanteriService, KriptografiEnvanteriManager>();
        services.AddProxiedScoped<IKriptolojiTuruService, KriptolojiTuruManager>();
        services.AddProxiedScoped<IKullanimSeviyesiService, KullanimSeviyesiManager>();
        services.AddProxiedScoped<IKurumsalSonucService, KurumsalSonucManager>();
        services.AddProxiedScoped<ILisansTakipSorumlusuService, LisansTakipSorumlusuManager>();
        services.AddProxiedScoped<IPersonelService, PersonelManager>();
        services.AddProxiedScoped<ISektorelEtkiService, SektorelEtkiManager>();
        services.AddProxiedScoped<ISurecService, SurecManager>();
        services.AddProxiedScoped<ITasinabilirCihazveOrtamService, TasinabilirCihazveOrtamManager>();
        services.AddProxiedScoped<IToplumsalSonucService, ToplumsalSonucManager>();
        services.AddProxiedScoped<IUygulamaService, UygulamaManager>();
        services.AddProxiedScoped<IVeritabaniService, VeritabaniManager>();
        services.AddProxiedScoped<IYedeklemeSorumlusuService, YedeklemeSorumlusuManager>();
        services.AddProxiedScoped<IYedeklemeTipiService, YedeklemeTipiManager>();
        services.AddProxiedScoped<IKullaniciService, KullaniciManager>();
        services.AddProxiedScoped<IRolService, RolManager>();
        services.AddProxiedScoped<IKullaniciRolService, KullaniciRolManager>();
        services.AddScoped<ILogService, LogManager>();
        services.AddProxiedScoped<IExcelService, ExcelManager>();
        services.AddProxiedScoped<IKullaniciBirimService, KullaniciBirimManager>();
        services.AddProxiedScoped<IRaporlamaService, RaporlamaManager>();
        services.AddProxiedScoped<IGuvenlikModuService, GuvenlikModuManager>();
        services.AddProxiedScoped<IEpostaTalepService, EpostaTalepManager>();
        services.AddProxiedScoped<IKurumService, KurumManager>();
        services.AddProxiedScoped<IBirimService, BirimManager>();

        // Kimlik doğrulama, aspect proxy'si olmadan kaydedilir: giriş denemeleri
        // düz metin şifre taşıdığı için LogAspect'in parametreleri kaydetmesi istenmez.
        services.AddSingleton<ISifreKoruyucu, SifreKoruyucu>();
        services.AddScoped<IKimlikDogrulamaService, KimlikDogrulamaManager>();

        return services;
    }
}