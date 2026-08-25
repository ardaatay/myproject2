using Dto.AgveSistem;
using Dto.FizikselMekan;
using Dto.IoTCihaz;
using Dto.Personel;
using Dto.TasinabilirCihazveOrtam;
using Dto.Uygulama;
using Dto.Veritabani;
using Dto.Surec;
using Dto.KriptografiEnvanteri;
using Dto.ElektronikBilgi;
using Dto.BasiliBilgi;
using Dto.Raporlama;
using Core.Entity;
using Core.Security;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace Repository.Context;

public class VarlikEnvanteriDbContext : DbContext
{
    /// <summary>
    /// Sorgu filtresi bu alan üzerinden kurulur. EF, alanı sorgu parametresine
    /// çevirdiği için derlenmiş model tüm değerler için yeniden kullanılabilir.
    /// </summary>
    private readonly IAktifOrganizasyon? _aktifOrganizasyon;

    public VarlikEnvanteriDbContext(
        DbContextOptions<VarlikEnvanteriDbContext> options,
        IAktifOrganizasyon? aktifOrganizasyon = null)
        : base(options)
    {
        // Değer yapıcıda okunmaz: context, kimlik doğrulama tamamlanmadan önce
        // de oluşturulabilir ve o anda claim henüz mevcut değildir.
        _aktifOrganizasyon = aktifOrganizasyon;
    }

    public DbSet<AgveSistem> AgveSistemler { get; set; }
    public DbSet<AnahtarSorumlusu> AnahtarSorumlulari { get; set; }
    public DbSet<BagimliVarlik> BagimliVarliklar { get; set; }
    public DbSet<BasiliBilgi> BasiliBilgiler { get; set; }
    public DbSet<BilgiSinifi> BilgiSiniflari { get; set; }
    public DbSet<Butunluk> Butunlukler { get; set; }
    public DbSet<DestekDurumu> DestekDurumlari { get; set; }
    public DbSet<Durum> Durumlar { get; set; }
    public DbSet<ElektronikBilgi> ElektronikBilgiler { get; set; }
    public DbSet<Erisilebilirlik> Erisilebilirlikler { get; set; }
    public DbSet<EtkilenenKisiSayisi> EtkilenenKisiSayilari { get; set; }
    public DbSet<FizikselMekan> FizikselMekanlar { get; set; }
    public DbSet<Gizlilik> Gizlilikler { get; set; }
    public DbSet<IoTCihaz> IoTCihazlari { get; set; }
    public DbSet<Kategori> Kategoriler { get; set; }
    public DbSet<Konum> Konumlar { get; set; }
    public DbSet<KriptolojiTuru> KriptolojiTurleri { get; set; }
    public DbSet<KriptografiEnvanteri> KriptografiEnvanterleri { get; set; }
    public DbSet<KullanimSeviyesi> KullanimSeviyeleri { get; set; }
    public DbSet<KurumsalSonuc> KurumsalSonuclar { get; set; }
    public DbSet<LisansTakipSorumlusu> LisansTakipSorumlulari { get; set; }
    public DbSet<Personel> Personeller { get; set; }
    public DbSet<SektorelEtki> SektorelEtkiler { get; set; }
    public DbSet<Surec> Surecler { get; set; }
    public DbSet<TasinabilirCihazveOrtam> TasinabilirCihazveOrtamlar { get; set; }
    public DbSet<ToplumsalSonuc> ToplumsalSonuclar { get; set; }
    public DbSet<Uygulama> Uygulamalar { get; set; }
    public DbSet<Veritabani> Veritabanlari { get; set; }
    public DbSet<YedeklemeSorumlusu> YedeklemeSorumlulari { get; set; }
    public DbSet<YedeklemeTipi> YedeklemeTipleri { get; set; }
    public DbSet<Kullanici> Kullanicilar { get; set; }
    public DbSet<Rol> Roller { get; set; }
    public DbSet<KullaniciRol> KullaniciRoller { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<KullaniciBirim> KullaniciBirimler { get; set; }
    public DbSet<GuvenlikModu> GuvenlikModu { get; set; }
    public DbSet<EpostaTalep> EpostaTalepleri { get; set; }
    public DbSet<Kurum> Kurumlar { get; set; }
    public DbSet<Birim> Birimler { get; set; }
    public DbSet<Organizasyon> Organizasyonlar { get; set; }

    // View'lar
    public DbSet<ListAgveSistemDto> AgveSistemDetay { get; set; }
    public DbSet<ListUygulamaDto> UygulamaDetay { get; set; }
    public DbSet<ListTasinabilirCihazveOrtamDto> TasinabilirCihazveOrtamDetay { get; set; }
    public DbSet<ListIoTCihazDto> IoTCihazDetay { get; set; }
    public DbSet<ListFizikselMekanDto> FizikselMekanDetay { get; set; }
    public DbSet<ListPersonelDto> PersonelDetay { get; set; }
    public DbSet<ListKriptografiEnvanteriDto> KriptografiEnvanteriDetay { get; set; }
    public DbSet<ListBasiliBilgiDto> BasiliBilgiDetay { get; set; }
    public DbSet<ListElektronikBilgiDto> ElektronikBilgiDetay { get; set; }
    public DbSet<ListVeritabaniDto> VeritabaniDetay { get; set; }
    public DbSet<ListSurecDto> SurecDetay { get; set; }
    public DbSet<ListRaporDto> RaporlamaDetay { get; set; }

    // Npgsql, DateTime'ı varsayılan olarak "timestamp with time zone"a eşler ve
    // DateTimeKind.Unspecified değerleri reddeder. Bu envanterdeki tarihler takvim
    // tarihi anlamı taşıdığı için tümü saat dilimsiz sütunlara eşlenir.
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveColumnType("timestamp without time zone");
        configurationBuilder.Properties<DateTime?>().HaveColumnType("timestamp without time zone");
    }

    /// <summary>
    /// Kiracıya ait her entity için global sorgu filtresi kurar. Filtre,
    /// <c>IKiraciEntity</c> uygulayan tüm entity'lere yansıma ile uygulanır;
    /// böylece yeni bir entity eklendiğinde filtre unutulmaz.
    /// </summary>
    private static readonly MethodInfo KiraciFiltresiKur = typeof(VarlikEnvanteriDbContext)
        .GetMethod(nameof(KiraciFiltresiUygula), BindingFlags.NonPublic | BindingFlags.Instance)!;

    private void KiraciFiltreleriniUygula(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IKiraciEntity).IsAssignableFrom(entityType.ClrType))
                KiraciFiltresiKur.MakeGenericMethod(entityType.ClrType).Invoke(this, [modelBuilder]);
        }
    }

    /// <summary>
    /// Filtre, context örneğinin <see cref="AktifOrganizasyonIdFiltre"/> özelliğine
    /// başvurur. EF bunu sorgu parametresine çevirir ve her istekte o isteğin
    /// context'inden okur; değer modele gömülmez.
    /// </summary>
    private void KiraciFiltresiUygula<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, IKiraciEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(
            e => AktifOrganizasyonIdFiltre == null || e.OrganizasyonId == AktifOrganizasyonIdFiltre);
    }

    /// <summary>Sorgu filtresinin okuduğu değer.</summary>
    public int? AktifOrganizasyonIdFiltre => _aktifOrganizasyon?.Id;

    /// <summary>
    /// Yeni eklenen kiracı kayıtlarına aktif organizasyonu atar. Değer zaten
    /// verilmişse (tohumlama, kurumlar arası işlemler) dokunulmaz.
    /// </summary>
    private void KiraciDamgasiVur()
    {
        if (AktifOrganizasyonIdFiltre is not { } organizasyonId)
            return;

        foreach (var giris in ChangeTracker.Entries<IKiraciEntity>())
        {
            if (giris.State == EntityState.Added && giris.Entity.OrganizasyonId == 0)
                giris.Entity.OrganizasyonId = organizasyonId;
        }
    }

    public override int SaveChanges()
    {
        KiraciDamgasiVur();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        KiraciDamgasiVur();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        KiraciDamgasiVur();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        KiraciDamgasiVur();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        KiraciFiltreleriniUygula(modelBuilder);

        // snake_case dönüştürücüsü, kelime sınırlarını büyük harften bulur:
        // "IoTCihazlari" → "io_t_cihazlari", "TasinabilirCihazveOrtamlar" →
        // "tasinabilir_cihazve_ortamlar". Bu ikisi okunur biçimde sabitlenir.
        modelBuilder.Entity<IoTCihaz>().ToTable("iot_cihazlari");
        modelBuilder.Entity<TasinabilirCihazveOrtam>().ToTable("tasinabilir_cihaz_ve_ortamlar");

        modelBuilder.Entity<Birim>(birim =>
        {
            // Kendine referans veren hiyerarşi: üst birim silinince alt ağacın
            // sessizce uçmaması için zincirleme silme kapalı.
            birim.HasOne(b => b.Ust)
                .WithMany(b => b.AltBirimler)
                .HasForeignKey(b => b.UstId)
                .OnDelete(DeleteBehavior.Restrict);

            birim.HasIndex(b => b.UstId);
            birim.HasIndex(b => b.Yol);
        });

        modelBuilder.Entity<ListAgveSistemDto>()
            .HasNoKey()
            .ToView("vw_agve_sistem_detay")
            .HasQueryFilter(v => AktifOrganizasyonIdFiltre == null || v.OrganizasyonId == AktifOrganizasyonIdFiltre);

        modelBuilder.Entity<ListUygulamaDto>()
            .HasNoKey()
            .ToView("vw_uygulama_detay")
            .HasQueryFilter(v => AktifOrganizasyonIdFiltre == null || v.OrganizasyonId == AktifOrganizasyonIdFiltre);

        modelBuilder.Entity<ListTasinabilirCihazveOrtamDto>()
            .HasNoKey()
            .ToView("vw_tasinabilir_cihaz_ve_ortam_detay")
            .HasQueryFilter(v => AktifOrganizasyonIdFiltre == null || v.OrganizasyonId == AktifOrganizasyonIdFiltre);

        modelBuilder.Entity<ListIoTCihazDto>()
            .HasNoKey()
            .ToView("vw_iot_cihaz_detay")
            .HasQueryFilter(v => AktifOrganizasyonIdFiltre == null || v.OrganizasyonId == AktifOrganizasyonIdFiltre);

        modelBuilder.Entity<ListFizikselMekanDto>()
            .HasNoKey()
            .ToView("vw_fiziksel_mekan_detay")
            .HasQueryFilter(v => AktifOrganizasyonIdFiltre == null || v.OrganizasyonId == AktifOrganizasyonIdFiltre);

        modelBuilder.Entity<ListPersonelDto>()
            .HasNoKey()
            .ToView("vw_personel_detay")
            .HasQueryFilter(v => AktifOrganizasyonIdFiltre == null || v.OrganizasyonId == AktifOrganizasyonIdFiltre);

        modelBuilder.Entity<ListKriptografiEnvanteriDto>()
            .HasNoKey()
            .ToView("vw_kriptografi_envanteri_detay")
            .HasQueryFilter(v => AktifOrganizasyonIdFiltre == null || v.OrganizasyonId == AktifOrganizasyonIdFiltre);

        modelBuilder.Entity<ListBasiliBilgiDto>()
            .HasNoKey()
            .ToView("vw_basili_bilgi_detay")
            .HasQueryFilter(v => AktifOrganizasyonIdFiltre == null || v.OrganizasyonId == AktifOrganizasyonIdFiltre);

        modelBuilder.Entity<ListElektronikBilgiDto>()
            .HasNoKey()
            .ToView("vw_elektronik_bilgi_detay")
            .HasQueryFilter(v => AktifOrganizasyonIdFiltre == null || v.OrganizasyonId == AktifOrganizasyonIdFiltre);

        modelBuilder.Entity<ListVeritabaniDto>()
            .HasNoKey()
            .ToView("vw_veritabani_detay")
            .HasQueryFilter(v => AktifOrganizasyonIdFiltre == null || v.OrganizasyonId == AktifOrganizasyonIdFiltre);

        modelBuilder.Entity<ListSurecDto>()
            .HasNoKey()
            .ToView("vw_surec_detay")
            .HasQueryFilter(v => AktifOrganizasyonIdFiltre == null || v.OrganizasyonId == AktifOrganizasyonIdFiltre);

        modelBuilder.Entity<ListRaporDto>()
            .HasNoKey()
            .ToView("vw_raporlama_genel")
            .HasQueryFilter(v => AktifOrganizasyonIdFiltre == null || v.OrganizasyonId == AktifOrganizasyonIdFiltre);
    }
}