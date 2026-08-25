using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Abstract;
using Repository.Concrete;
using Repository.Context;
using Repository.UnitOfWork;

namespace Repository.Configuration;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositoryExt(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<VarlikEnvanteriDbContext>(options =>
            options.UseNpgsql(connectionString)
                   .UseSnakeCaseNamingConvention());

        services.AddScoped<IVarlikEnvanteriUnitOfWork, VarlikEnvanteriUnitOfWork>();

        services.AddScoped<IAgveSistemRepository, AgveSistemRepository>();
        services.AddScoped<IAnahtarSorumlusuRepository, AnahtarSorumlusuRepository>();
        services.AddScoped<IBagimliVarliklarRepository, BagimliVarliklarRepository>();
        services.AddScoped<IBasiliBilgiRepository, BasiliBilgiRepository>();
        services.AddScoped<IBilgiSinifiRepository, BilgiSinifiRepository>();
        services.AddScoped<IButunlukRepository, ButunlukRepository>();
        services.AddScoped<IDestekDurumuRepository, DestekDurumuRepository>();
        services.AddScoped<IDurumRepository, DurumRepository>();
        services.AddScoped<IElektronikBilgiRepository, ElektronikBilgiRepository>();
        services.AddScoped<IErisilebilirlikRepository, ErisilebilirlikRepository>();
        services.AddScoped<IEtkilenenKisiSayisiRepository, EtkilenenKisiSayisiRepository>();
        services.AddScoped<IFizikselMekanRepository, FizikselMekanRepository>();
        services.AddScoped<IGizlilikRepository, GizlilikRepository>();
        services.AddScoped<IIoTCihazRepository, IoTCihazRepository>();
        services.AddScoped<IKategoriRepository, KategoriRepository>();
        services.AddScoped<IKonumRepository, KonumRepository>();
        services.AddScoped<IKriptografiEnvanteriRepository, KriptografiEnvanteriRepository>();
        services.AddScoped<IKriptolojiTuruRepository, KriptolojiTuruRepository>();
        services.AddScoped<IKullaniciRepository, KullaniciRepository>();
        services.AddScoped<IKullaniciRolRepository, KullaniciRolRepository>();
        services.AddScoped<IKullanimSeviyesiRepository, KullanimSeviyesiRepository>();
        services.AddScoped<IKurumsalSonucRepository, KurumsalSonucRepository>();
        services.AddScoped<ILisansTakipSorumlusuRepository, LisansTakipSorumlusuRepository>();
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddScoped<IPersonelRepository, PersonelRepository>();
        services.AddScoped<IRolRepository, RolRepository>();
        services.AddScoped<ISektorelEtkiRepository, SektorelEtkiRepository>();
        services.AddScoped<ISurecRepository, SurecRepository>();
        services.AddScoped<ITasinabilirCihazveOrtamRepository, TasinabilirCihazveOrtamRepository>();
        services.AddScoped<IToplumsalSonucRepository, ToplumsalSonucRepository>();
        services.AddScoped<IUygulamaRepository, UygulamaRepository>();
        services.AddScoped<IVeritabaniRepository, VeritabaniRepository>();
        services.AddScoped<IYedeklemeSorumlusuRepository, YedeklemeSorumlusuRepository>();
        services.AddScoped<IYedeklemeTipiRepository, YedeklemeTipiRepository>();
        services.AddScoped<IKullaniciBirimRepository, KullaniciBirimRepository>();
        services.AddScoped<IRaporlamaRepository, RaporlamaRepository>();
        services.AddScoped<IGuvenlikModuRepository, GuvenlikModuRepository>();
        services.AddScoped<IEpostaTalepRepository, EpostaTalepRepository>();
        services.AddScoped<IKurumRepository, KurumRepository>();
        services.AddScoped<IBirimRepository, BirimRepository>();

        return services;
    }
}