using AutoMapper;
using Dto.AgveSistem;
using Dto.AnahtarSorumlusu;
using Dto.BagimliVarliklar;
using Dto.BasiliBilgi;
using Dto.BilgiSinifi;
using Dto.Butunluk;
using Dto.DestekDurumu;
using Dto.Durum;
using Dto.ElektronikBilgi;
using Dto.EpostaTalep;
using Dto.Erisilebilirlik;
using Dto.EtkilenenKisiSayisi;
using Dto.FizikselMekan;
using Dto.Gizlilik;
using Dto.IoTCihaz;
using Dto.Kategori;
using Dto.Konum;
using Dto.KriptografiEnvanteri;
using Dto.KriptolojiTuru;
using Dto.Kullanici;
using Dto.KullaniciBirim;
using Dto.KullaniciRol;
using Dto.KullanimSeviyesi;
using Dto.Birim;
using Dto.Kurum;
using Dto.KurumsalSonuc;
using Dto.LisansTakipSorumlusu;
using Dto.Personel;
using Dto.Rol;
using Dto.SektorelEtki;
using Dto.Surec;
using Dto.TasinabilirCihazveOrtam;
using Dto.ToplumsalSonuc;
using Dto.Uygulama;
using Dto.Veritabani;
using Dto.YedeklemeSorumlusu;
using Dto.YedeklemeTipi;
using Entity.Concrete;

namespace Business.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AgveSistem, CreateAgveSistemDto>();
        CreateMap<CreateAgveSistemDto, AgveSistem>()
            .ForMember(dest => dest.EnvantereGirisTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<AgveSistem, UpdateAgveSistemDto>();
        CreateMap<UpdateAgveSistemDto, AgveSistem>()
            .ForMember(dest => dest.EnvanterGuncellemeTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<AgveSistem, ListAgveSistemDto>().ReverseMap();
        CreateMap<UpdateAgveSistemDto, CreateAgveSistemDto>();

        CreateMap<AnahtarSorumlusu, CreateAnahtarSorumlusuDto>();
        CreateMap<CreateAnahtarSorumlusuDto, AnahtarSorumlusu>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<AnahtarSorumlusu, UpdateAnahtarSorumlusuDto>();
        CreateMap<UpdateAnahtarSorumlusuDto, AnahtarSorumlusu>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<AnahtarSorumlusu, ListAnahtarSorumlusuDto>().ReverseMap();

        CreateMap<BagimliVarlik, CreateBagimliVarliklarDto>();
        CreateMap<CreateBagimliVarliklarDto, BagimliVarlik>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<BagimliVarlik, UpdateBagimliVarliklarDto>();
        CreateMap<UpdateBagimliVarliklarDto, BagimliVarlik>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<BagimliVarlik, ListBagimliVarliklarDto>().ReverseMap();

        CreateMap<BasiliBilgi, CreateBasiliBilgiDto>();
        CreateMap<CreateBasiliBilgiDto, BasiliBilgi>()
            .ForMember(dest => dest.EnvantereGirisTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<BasiliBilgi, UpdateBasiliBilgiDto>();
        CreateMap<UpdateBasiliBilgiDto, BasiliBilgi>()
            .ForMember(dest => dest.EnvanterGuncellemeTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<BasiliBilgi, ListBasiliBilgiDto>().ReverseMap();
        CreateMap<UpdateBasiliBilgiDto, CreateBasiliBilgiDto>();

        CreateMap<BilgiSinifi, CreateBilgiSinifiDto>();
        CreateMap<CreateBilgiSinifiDto, BilgiSinifi>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<BilgiSinifi, UpdateBilgiSinifiDto>();
        CreateMap<UpdateBilgiSinifiDto, BilgiSinifi>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<BilgiSinifi, ListBilgiSinifiDto>().ReverseMap();

        CreateMap<Butunluk, CreateButunlukDto>();
        CreateMap<CreateButunlukDto, Butunluk>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Butunluk, UpdateButunlukDto>();
        CreateMap<UpdateButunlukDto, Butunluk>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Butunluk, ListButunlukDto>().ReverseMap();

        CreateMap<DestekDurumu, CreateDestekDurumuDto>();
        CreateMap<CreateDestekDurumuDto, DestekDurumu>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<DestekDurumu, UpdateDestekDurumuDto>();
        CreateMap<UpdateDestekDurumuDto, DestekDurumu>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<DestekDurumu, ListDestekDurumuDto>().ReverseMap();

        CreateMap<Durum, CreateDurumDto>();
        CreateMap<CreateDurumDto, Durum>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Durum, UpdateDurumDto>();
        CreateMap<UpdateDurumDto, Durum>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Durum, ListDurumDto>().ReverseMap();

        CreateMap<ElektronikBilgi, CreateElektronikBilgiDto>();
        CreateMap<CreateElektronikBilgiDto, ElektronikBilgi>()
            .ForMember(dest => dest.EnvantereGirisTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<ElektronikBilgi, UpdateElektronikBilgiDto>();
        CreateMap<UpdateElektronikBilgiDto, ElektronikBilgi>()
            .ForMember(dest => dest.EnvanterGuncellemeTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<ElektronikBilgi, ListElektronikBilgiDto>().ReverseMap();
        CreateMap<UpdateElektronikBilgiDto, CreateElektronikBilgiDto>();

        CreateMap<Erisilebilirlik, CreateErisilebilirlikDto>();
        CreateMap<CreateErisilebilirlikDto, Erisilebilirlik>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Erisilebilirlik, UpdateErisilebilirlikDto>();
        CreateMap<UpdateErisilebilirlikDto, Erisilebilirlik>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Erisilebilirlik, ListErisilebilirlikDto>().ReverseMap();

        CreateMap<EtkilenenKisiSayisi, CreateEtkilenenKisiSayisiDto>();
        CreateMap<CreateEtkilenenKisiSayisiDto, EtkilenenKisiSayisi>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<EtkilenenKisiSayisi, UpdateEtkilenenKisiSayisiDto>();
        CreateMap<UpdateEtkilenenKisiSayisiDto, EtkilenenKisiSayisi>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<EtkilenenKisiSayisi, ListEtkilenenKisiSayisiDto>().ReverseMap();

        CreateMap<FizikselMekan, CreateFizikselMekanDto>();
        CreateMap<CreateFizikselMekanDto, FizikselMekan>()
            .ForMember(dest => dest.EnvantereGirisTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<FizikselMekan, UpdateFizikselMekanDto>();
        CreateMap<UpdateFizikselMekanDto, FizikselMekan>()
            .ForMember(dest => dest.EnvanterGuncellemeTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<FizikselMekan, ListFizikselMekanDto>().ReverseMap();
        CreateMap<UpdateFizikselMekanDto, CreateFizikselMekanDto>();

        CreateMap<Gizlilik, CreateGizlilikDto>();
        CreateMap<CreateGizlilikDto, Gizlilik>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Gizlilik, UpdateGizlilikDto>();
        CreateMap<UpdateGizlilikDto, Gizlilik>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Gizlilik, ListGizlilikDto>().ReverseMap();

        CreateMap<IoTCihaz, CreateIoTCihazDto>();
        CreateMap<CreateIoTCihazDto, IoTCihaz>()
            .ForMember(dest => dest.EnvantereGirisTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<IoTCihaz, UpdateIoTCihazDto>();
        CreateMap<UpdateIoTCihazDto, IoTCihaz>()
            .ForMember(dest => dest.EnvanterGuncellemeTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<IoTCihaz, ListIoTCihazDto>()
            .ForMember(dest => dest.Kategori, src => src.MapFrom(x => x.Kategori != null ? x.Kategori.Ad : ""))
            .ForMember(dest => dest.AltKategori, src => src.MapFrom(x => x.AltKategori != null ? x.AltKategori.Ad : ""))
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => x.Durum != null ? x.Durum.Ad : ""))
            .ForMember(dest => dest.BilgiSinifi, src => src.MapFrom(x => x.BilgiSinifi != null ? x.BilgiSinifi.Ad : ""))
            .ForMember(dest => dest.Gizlilik, src => src.MapFrom(x => x.Gizlilik != null ? x.Gizlilik.Ad : ""))
            .ForMember(dest => dest.Butunluk, src => src.MapFrom(x => x.Butunluk != null ? x.Butunluk.Ad : ""))
            .ForMember(dest => dest.Erisilebilirlik, src => src.MapFrom(x => x.Erisilebilirlik != null ? x.Erisilebilirlik.Ad : ""))
            .ForMember(dest => dest.EtkilenenKisiSayisi, src => src.MapFrom(x => x.EtkilenenKisiSayisi != null ? x.EtkilenenKisiSayisi.Ad : ""))
            .ForMember(dest => dest.ToplumsalSonuc, src => src.MapFrom(x => x.ToplumsalSonuc != null ? x.ToplumsalSonuc.Ad : ""))
            .ForMember(dest => dest.KurumsalSonuc, src => src.MapFrom(x => x.KurumsalSonuc != null ? x.KurumsalSonuc.Ad : ""))
            .ForMember(dest => dest.SektorelEtki, src => src.MapFrom(x => x.SektorelEtki != null ? x.SektorelEtki.Ad : ""))
            .ForMember(dest => dest.BagimliVarlik, src => src.MapFrom(x => x.BagimliVarlik != null ? x.BagimliVarlik.Ad : ""))
            .ForMember(dest => dest.YedeklemeTipi, src => src.MapFrom(x => x.YedeklemeTipi != null ? x.YedeklemeTipi.Ad : ""))
            .ForMember(dest => dest.YedeklemeSorumlusu, src => src.MapFrom(x => x.YedeklemeSorumlusu != null ? x.YedeklemeSorumlusu.Ad : ""))
            .ForMember(dest => dest.Kriptoloji,
                src => src.MapFrom(x => x.Kriptoloji == true ? "Gerekli" : "Gerekli Değil"))
            .ForMember(dest => dest.KriptolojiTuru, src => src.MapFrom(x => x.KriptolojiTuru != null ? x.KriptolojiTuru.Ad : ""))
            .ForMember(dest => dest.AnahtarSorumlusu, src => src.MapFrom(x => x.AnahtarSorumlusu != null ? x.AnahtarSorumlusu.Ad : ""))
            .ForMember(dest => dest.KisiselVeriBarindirma,
                src => src.MapFrom(x => x.KisiselVeriBarindirma == true ? "Var" : "Yok"))
            .ForMember(dest => dest.AnlikMesajlasmaKullanimi,
                src => src.MapFrom(x => x.AnlikMesajlasmaKullanimi == true ? "Evet" : "Hayır"))
            .ForMember(dest => dest.BulutBilisim, src => src.MapFrom(x => x.BulutBilisim == true ? "Evet" : "Hayır"))
            .ForMember(dest => dest.YeniGelismelerveTedarik,
                src => src.MapFrom(x => x.YeniGelistirmelerveTedarik == true ? "Evet" : "Hayır"))
            .ForMember(dest => dest.KritikAltyapiSistemi,
                src => src.MapFrom(x => x.KritikAltyapiSistemi == true ? "Evet" : "Hayır"))
            .ForMember(dest => dest.LisansTakipSorumlusu, src => src.MapFrom(x => x.LisansTakipSorumlusu != null ? x.LisansTakipSorumlusu.Ad : ""));

        CreateMap<UpdateIoTCihazDto, CreateIoTCihazDto>();

        CreateMap<Kategori, CreateKategoriDto>();
        CreateMap<CreateKategoriDto, Kategori>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Kategori, UpdateKategoriDto>();
        CreateMap<UpdateKategoriDto, Kategori>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Kategori, ListKategoriDto>()
            .ForMember(dest => dest.UstKategori, src => src.MapFrom(x => x.Ust))
            .ForMember(dest => dest.UstKategoriId, src => src.MapFrom(x => x.UstId))
            .ReverseMap();

        CreateMap<Konum, CreateKonumDto>();
        CreateMap<CreateKonumDto, Konum>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Konum, UpdateKonumDto>();
        CreateMap<UpdateKonumDto, Konum>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Konum, ListKonumDto>().ReverseMap();

        CreateMap<KriptolojiTuru, CreateKriptolojiTuruDto>();
        CreateMap<CreateKriptolojiTuruDto, KriptolojiTuru>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<KriptolojiTuru, UpdateKriptolojiTuruDto>();
        CreateMap<UpdateKriptolojiTuruDto, KriptolojiTuru>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<KriptolojiTuru, ListKriptolojiTuruDto>().ReverseMap();

        CreateMap<KriptografiEnvanteri, CreateKriptografiEnvanteriDto>().ReverseMap();
        CreateMap<KriptografiEnvanteri, UpdateKriptografiEnvanteriDto>().ReverseMap();
        CreateMap<KriptografiEnvanteri, ListKriptografiEnvanteriDto>()
            .ForMember(dest => dest.AnahtarSorumlusu, opt => opt.MapFrom(src => src.AnahtarSorumlusu))
            .ForMember(dest => dest.KullanimSeviyesi, opt => opt.MapFrom(src => src.KullanimSeviyesi))
            .ReverseMap();
        CreateMap<UpdateKriptografiEnvanteriDto, CreateKriptografiEnvanteriDto>();

        CreateMap<KullanimSeviyesi, CreateKullanimSeviyesiDto>();
        CreateMap<CreateKullanimSeviyesiDto, KullanimSeviyesi>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<KullanimSeviyesi, UpdateKullanimSeviyesiDto>();
        CreateMap<UpdateKullanimSeviyesiDto, KullanimSeviyesi>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<KullanimSeviyesi, ListKullanimSeviyesiDto>().ReverseMap();

        CreateMap<KurumsalSonuc, CreateKurumsalSonucDto>();
        CreateMap<CreateKurumsalSonucDto, KurumsalSonuc>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<KurumsalSonuc, UpdateKurumsalSonucDto>();
        CreateMap<UpdateKurumsalSonucDto, KurumsalSonuc>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<KurumsalSonuc, ListKurumsalSonucDto>().ReverseMap();

        CreateMap<LisansTakipSorumlusu, CreateLisansTakipSorumlusuDto>();
        CreateMap<CreateLisansTakipSorumlusuDto, LisansTakipSorumlusu>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<LisansTakipSorumlusu, UpdateLisansTakipSorumlusuDto>();
        CreateMap<UpdateLisansTakipSorumlusuDto, LisansTakipSorumlusu>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<LisansTakipSorumlusu, ListLisansTakipSorumlusuDto>().ReverseMap();

        CreateMap<Personel, CreatePersonelDto>();
        CreateMap<CreatePersonelDto, Personel>()
            .ForMember(dest => dest.EnvantereGirisTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<Personel, UpdatePersonelDto>();
        CreateMap<UpdatePersonelDto, Personel>()
            .ForMember(dest => dest.EnvanterGuncellemeTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<Personel, ListPersonelDto>().ReverseMap();
        CreateMap<UpdatePersonelDto, CreatePersonelDto>();

        CreateMap<SektorelEtki, CreateSektorelEtkiDto>();
        CreateMap<CreateSektorelEtkiDto, SektorelEtki>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<SektorelEtki, UpdateSektorelEtkiDto>();
        CreateMap<UpdateSektorelEtkiDto, SektorelEtki>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<SektorelEtki, ListSektorelEtkiDto>().ReverseMap();

        CreateMap<Surec, CreateSurecDto>();
        CreateMap<CreateSurecDto, Surec>()
            .ForMember(dest => dest.EnvantereGirisTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<Surec, UpdateSurecDto>();
        CreateMap<UpdateSurecDto, Surec>()
            .ForMember(dest => dest.EnvanterGuncellemeTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<Surec, ListSurecDto>().ReverseMap();
        CreateMap<UpdateSurecDto, CreateSurecDto>();

        CreateMap<TasinabilirCihazveOrtam, CreateTasinabilirCihazveOrtamDto>();
        CreateMap<CreateTasinabilirCihazveOrtamDto, TasinabilirCihazveOrtam>()
            .ForMember(dest => dest.EnvantereGirisTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<TasinabilirCihazveOrtam, UpdateTasinabilirCihazveOrtamDto>();
        CreateMap<UpdateTasinabilirCihazveOrtamDto, TasinabilirCihazveOrtam>()
            .ForMember(dest => dest.EnvanterGuncellemeTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<TasinabilirCihazveOrtam, ListTasinabilirCihazveOrtamDto>().ReverseMap();
        CreateMap<UpdateTasinabilirCihazveOrtamDto, CreateTasinabilirCihazveOrtamDto>();

        CreateMap<ToplumsalSonuc, CreateToplumsalSonucDto>();
        CreateMap<CreateToplumsalSonucDto, ToplumsalSonuc>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<ToplumsalSonuc, UpdateToplumsalSonucDto>();
        CreateMap<UpdateToplumsalSonucDto, ToplumsalSonuc>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<ToplumsalSonuc, ListToplumsalSonucDto>().ReverseMap();

        CreateMap<Uygulama, CreateUygulamaDto>();
        CreateMap<CreateUygulamaDto, Uygulama>()
            .ForMember(dest => dest.EnvantereGirisTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<Uygulama, UpdateUygulamaDto>();
        CreateMap<UpdateUygulamaDto, Uygulama>()
            .ForMember(dest => dest.EnvanterGuncellemeTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<Uygulama, ListUygulamaDto>().ReverseMap();
        CreateMap<UpdateUygulamaDto, CreateUygulamaDto>();

        CreateMap<Veritabani, CreateVeritabaniDto>();
        CreateMap<CreateVeritabaniDto, Veritabani>()
            .ForMember(dest => dest.EnvantereGirisTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<Veritabani, UpdateVeritabaniDto>();
        CreateMap<UpdateVeritabaniDto, Veritabani>()
            .ForMember(dest => dest.EnvanterGuncellemeTarihi, src => src.MapFrom(x => DateTime.Now));
        CreateMap<Veritabani, ListVeritabaniDto>().ReverseMap();
        CreateMap<UpdateVeritabaniDto, CreateVeritabaniDto>();

        CreateMap<YedeklemeSorumlusu, CreateYedeklemeSorumlusuDto>();
        CreateMap<CreateYedeklemeSorumlusuDto, YedeklemeSorumlusu>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<YedeklemeSorumlusu, UpdateYedeklemeSorumlusuDto>();
        CreateMap<UpdateYedeklemeSorumlusuDto, YedeklemeSorumlusu>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<YedeklemeSorumlusu, ListYedeklemeSorumlusuDto>().ReverseMap();

        CreateMap<YedeklemeTipi, CreateYedeklemeTipiDto>();
        CreateMap<CreateYedeklemeTipiDto, YedeklemeTipi>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<YedeklemeTipi, UpdateYedeklemeTipiDto>();
        CreateMap<UpdateYedeklemeTipiDto, YedeklemeTipi>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<YedeklemeTipi, ListYedeklemeTipiDto>().ReverseMap();

        CreateMap<KullaniciRol, CreateKullaniciRolDto>();
        CreateMap<CreateKullaniciRolDto, KullaniciRol>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<KullaniciRol, UpdateKullaniciRolDto>();
        CreateMap<UpdateKullaniciRolDto, KullaniciRol>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<KullaniciRol, ListKullaniciRolDto>().ReverseMap();

        CreateMap<Kullanici, CreateKullaniciDto>();
        CreateMap<CreateKullaniciDto, Kullanici>()
            .ForMember(dest => dest.Username, src => src.MapFrom(x => x.Username.Trim().ToLower()))
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Kullanici, UpdateKullaniciDto>();
        CreateMap<UpdateKullaniciDto, Kullanici>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Kullanici, ListKullaniciDto>().ReverseMap();

        CreateMap<Rol, CreateRolDto>();
        CreateMap<CreateRolDto, Rol>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Rol, UpdateRolDto>();
        CreateMap<UpdateRolDto, Rol>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));
        CreateMap<Rol, ListRolDto>().ReverseMap();

        CreateMap<KullaniciBirim, ListKullaniciBirimDto>()
            .ForMember(dest => dest.Username, src => src.MapFrom(x => x.Kullanici != null ? x.Kullanici.Username : ""));

        CreateMap<KullaniciBirim, KullaniciBirimDto>();
        CreateMap<KullaniciBirim, UpdateKullaniciBirimDto>().ReverseMap();
        CreateMap<CreateKullaniciBirimDto, KullaniciBirim>()
            .ForMember(dest => dest.Durum, src => src.MapFrom(x => true));

        CreateMap<CreateEpostaTalepDto, EpostaTalep>().ReverseMap();
        CreateMap<UpdateEpostaTalepDto, EpostaTalep>().ReverseMap();
        CreateMap<UpdateEpostaTalepDto, CreateEpostaTalepDto>().ReverseMap();
        CreateMap<EpostaTalep, ListEpostaTalepDto>();

        CreateMap<CreateKurumDto, Kurum>().ReverseMap();
        CreateMap<UpdateKurumDto, Kurum>().ReverseMap();
        CreateMap<UpdateKurumDto, CreateKurumDto>().ReverseMap();
        CreateMap<Kurum, ListKurumDto>();

        CreateMap<CreateBirimDto, Birim>().ReverseMap();
        CreateMap<UpdateBirimDto, Birim>().ReverseMap();
        CreateMap<UpdateBirimDto, CreateBirimDto>().ReverseMap();
        CreateMap<Birim, ListBirimDto>();
    }
}