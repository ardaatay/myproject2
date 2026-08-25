using Business.Abstract;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Util.Query;

namespace Business.Concrete;

public class ExcelManager(
    IAgveSistemService agveSistemService,
    IUygulamaService uygulamaService,
    ITasinabilirCihazveOrtamService tasinabilirCihazveOrtamService,
    IIoTCihazService ioTCihazService,
    IFizikselMekanService fizikselMekanService,
    IPersonelService personelService,
    IKriptografiEnvanteriService kriptografiEnvanteriService,
    IBasiliBilgiService basiliBilgiService,
    IElektronikBilgiService elektronikBilgiService,
    IVeritabaniService veritabaniService,
    ISurecService surecService,
    IRaporlamaService raporlamaService,
    IEpostaTalepService epostaTalepService) : IExcelService
{
    public async Task<MemoryStream> GenerateExcelAgveSistem()
    {
        var agVeSistemList = await agveSistemService.GetAllExcelAsync();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Ağ ve Sistemler");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Yedekleme Tipi";
        worksheet.Cells[1, 26].Value = "Yedekleme Türü";
        worksheet.Cells[1, 27].Value = "Yedekleme Sıklığı";
        worksheet.Cells[1, 28].Value = "Yedeklerin Saklama Süresi";
        worksheet.Cells[1, 29].Value = "Yedekleme Alanı";
        worksheet.Cells[1, 30].Value = "Yedekten Dönüş Planı";
        worksheet.Cells[1, 31].Value = "Yedekleme Sorumlusu";
        worksheet.Cells[1, 32].Value = "Kriptoloji";
        worksheet.Cells[1, 33].Value = "Kriptoloji Türü";
        worksheet.Cells[1, 34].Value = "Kullanılan Kriptoloji";
        worksheet.Cells[1, 35].Value = "Anahtar Sorumlusu";
        worksheet.Cells[1, 36].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 37].Value = "Anlık Mesajlaşma Kullanımı";
        worksheet.Cells[1, 38].Value = "Bulut Bilişim";
        worksheet.Cells[1, 39].Value = "Yeni Gelişmeler ve Tedarik";
        worksheet.Cells[1, 40].Value = "Kritik Altyapı Sistemi";
        worksheet.Cells[1, 41].Value = "Ip Adresi";
        worksheet.Cells[1, 42].Value = "İşletim Sistemi";
        worksheet.Cells[1, 43].Value = "Lisans Takip Sorumlusu";
        worksheet.Cells[1, 44].Value = "Marka/Model";
        worksheet.Cells[1, 45].Value = "Seri Numarası";
        worksheet.Cells[1, 46].Value = "Zimmet Sahibi";
        worksheet.Cells[1, 47].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 48].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 49].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 49])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < agVeSistemList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("AS", agVeSistemList[i].Id);
            worksheet.Cells[i + 2, 2].Value = agVeSistemList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = agVeSistemList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = agVeSistemList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = agVeSistemList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = agVeSistemList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = agVeSistemList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = agVeSistemList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = agVeSistemList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = agVeSistemList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = agVeSistemList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = agVeSistemList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                (agVeSistemList[i].Gizlilik?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].Gizlilik?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].Gizlilik?.StartsWith('c') ?? false) ? "3 puan" :
                (agVeSistemList[i].Gizlilik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                (agVeSistemList[i].Butunluk?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].Butunluk?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].Butunluk?.StartsWith('c') ?? false) ? "3 puan" :
                (agVeSistemList[i].Butunluk?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                (agVeSistemList[i].Erisilebilirlik?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].Erisilebilirlik?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].Erisilebilirlik?.StartsWith('c') ?? false) ? "3 puan" :
                (agVeSistemList[i].Erisilebilirlik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                (agVeSistemList[i].EtkilenenKisiSayisi?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].EtkilenenKisiSayisi?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].EtkilenenKisiSayisi?.StartsWith('c') ?? false) ? "3 puan" :
                (agVeSistemList[i].EtkilenenKisiSayisi?.StartsWith('d') ?? false) ? "4 puan" :
                (agVeSistemList[i].EtkilenenKisiSayisi?.StartsWith('e') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                (agVeSistemList[i].ToplumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].ToplumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].ToplumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                (agVeSistemList[i].ToplumsalSonuc?.StartsWith('d') ?? false) ? "5 puan" :
                (agVeSistemList[i].ToplumsalSonuc?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                (agVeSistemList[i].KurumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].KurumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].KurumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                (agVeSistemList[i].SektorelEtki?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].SektorelEtki?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].SektorelEtki?.StartsWith('c') ?? false) ? "3 puan" :
                (agVeSistemList[i].SektorelEtki?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                (agVeSistemList[i].BagimliVarlik?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].BagimliVarlik?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].BagimliVarlik?.StartsWith('c') ?? false) ? "3 puan" :
                (agVeSistemList[i].BagimliVarlik?.StartsWith('d') ?? false) ? "5 puan" :
                (agVeSistemList[i].BagimliVarlik?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = agVeSistemList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = agVeSistemList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = agVeSistemList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = agVeSistemList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = agVeSistemList[i].YedeklemeTipi;
            worksheet.Cells[i + 2, 26].Value = agVeSistemList[i].YedeklemeTuru;
            worksheet.Cells[i + 2, 27].Value = agVeSistemList[i].YedeklemeSikligi;
            worksheet.Cells[i + 2, 28].Value = agVeSistemList[i].YedeklerinSaklamaSuresi;
            worksheet.Cells[i + 2, 29].Value = agVeSistemList[i].YedeklemeAlani;
            worksheet.Cells[i + 2, 30].Value = agVeSistemList[i].YedektenDonusPlani;
            worksheet.Cells[i + 2, 31].Value = agVeSistemList[i].YedeklemeSorumlusu;
            worksheet.Cells[i + 2, 32].Value = agVeSistemList[i].Kriptoloji;
            worksheet.Cells[i + 2, 33].Value = agVeSistemList[i].KriptolojiTuru;
            worksheet.Cells[i + 2, 34].Value = agVeSistemList[i].KullanilanKriptoloji;
            worksheet.Cells[i + 2, 35].Value = agVeSistemList[i].AnahtarSorumlusu;
            worksheet.Cells[i + 2, 36].Value = agVeSistemList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 37].Value = agVeSistemList[i].AnlikMesajlasmaKullanimi;
            worksheet.Cells[i + 2, 38].Value = agVeSistemList[i].BulutBilisim;
            worksheet.Cells[i + 2, 39].Value = agVeSistemList[i].YeniGelismelerveTedarik;
            worksheet.Cells[i + 2, 40].Value = agVeSistemList[i].KritikAltyapiSistemi;
            worksheet.Cells[i + 2, 41].Value = agVeSistemList[i].IpAdresi;
            worksheet.Cells[i + 2, 42].Value = agVeSistemList[i].IsletimSistemi;
            worksheet.Cells[i + 2, 43].Value = agVeSistemList[i].LisansTakipSorumlusu;
            worksheet.Cells[i + 2, 44].Value = agVeSistemList[i].MarkaModel;
            worksheet.Cells[i + 2, 45].Value = agVeSistemList[i].SeriNumarasi;
            worksheet.Cells[i + 2, 46].Value = agVeSistemList[i].ZimmetSahibi;
            worksheet.Cells[i + 2, 47].Value = agVeSistemList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 48].Value = agVeSistemList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 49].Value = agVeSistemList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelAgveSistem(
        string search, FilterBag filterBag)
    {
        var agVeSistemList = await agveSistemService.GetAllExcelAsync(search, filterBag);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Ağ ve Sistemler");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Yedekleme Tipi";
        worksheet.Cells[1, 26].Value = "Yedekleme Türü";
        worksheet.Cells[1, 27].Value = "Yedekleme Sıklığı";
        worksheet.Cells[1, 28].Value = "Yedeklerin Saklama Süresi";
        worksheet.Cells[1, 29].Value = "Yedekleme Alanı";
        worksheet.Cells[1, 30].Value = "Yedekten Dönüş Planı";
        worksheet.Cells[1, 31].Value = "Yedekleme Sorumlusu";
        worksheet.Cells[1, 32].Value = "Kriptoloji";
        worksheet.Cells[1, 33].Value = "Kriptoloji Türü";
        worksheet.Cells[1, 34].Value = "Kullanılan Kriptoloji";
        worksheet.Cells[1, 35].Value = "Anahtar Sorumlusu";
        worksheet.Cells[1, 36].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 37].Value = "Anlık Mesajlaşma Kullanımı";
        worksheet.Cells[1, 38].Value = "Bulut Bilişim";
        worksheet.Cells[1, 39].Value = "Yeni Gelişmeler ve Tedarik";
        worksheet.Cells[1, 40].Value = "Kritik Altyapı Sistemi";
        worksheet.Cells[1, 41].Value = "Ip Adresi";
        worksheet.Cells[1, 42].Value = "İşletim Sistemi";
        worksheet.Cells[1, 43].Value = "Lisans Takip Sorumlusu";
        worksheet.Cells[1, 44].Value = "Marka/Model";
        worksheet.Cells[1, 45].Value = "Seri Numarası";
        worksheet.Cells[1, 46].Value = "Zimmet Sahibi";
        worksheet.Cells[1, 47].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 48].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 49].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 49])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < agVeSistemList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("AS", agVeSistemList[i].Id);
            worksheet.Cells[i + 2, 2].Value = agVeSistemList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = agVeSistemList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = agVeSistemList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = agVeSistemList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = agVeSistemList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = agVeSistemList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = agVeSistemList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = agVeSistemList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = agVeSistemList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = agVeSistemList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = agVeSistemList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                (agVeSistemList[i].Gizlilik?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].Gizlilik?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].Gizlilik?.StartsWith('c') ?? false) ? "3 puan" :
                (agVeSistemList[i].Gizlilik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                (agVeSistemList[i].Butunluk?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].Butunluk?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].Butunluk?.StartsWith('c') ?? false) ? "3 puan" :
                (agVeSistemList[i].Butunluk?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                (agVeSistemList[i].Erisilebilirlik?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].Erisilebilirlik?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].Erisilebilirlik?.StartsWith('c') ?? false) ? "3 puan" :
                (agVeSistemList[i].Erisilebilirlik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                (agVeSistemList[i].EtkilenenKisiSayisi?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].EtkilenenKisiSayisi?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].EtkilenenKisiSayisi?.StartsWith('c') ?? false) ? "3 puan" :
                (agVeSistemList[i].EtkilenenKisiSayisi?.StartsWith('d') ?? false) ? "4 puan" :
                (agVeSistemList[i].EtkilenenKisiSayisi?.StartsWith('e') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                (agVeSistemList[i].ToplumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].ToplumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].ToplumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                (agVeSistemList[i].ToplumsalSonuc?.StartsWith('d') ?? false) ? "5 puan" :
                (agVeSistemList[i].ToplumsalSonuc?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                (agVeSistemList[i].KurumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].KurumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].KurumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                (agVeSistemList[i].SektorelEtki?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].SektorelEtki?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].SektorelEtki?.StartsWith('c') ?? false) ? "3 puan" :
                (agVeSistemList[i].SektorelEtki?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                (agVeSistemList[i].BagimliVarlik?.StartsWith('a') ?? false) ? "1 puan" :
                (agVeSistemList[i].BagimliVarlik?.StartsWith('b') ?? false) ? "2 puan" :
                (agVeSistemList[i].BagimliVarlik?.StartsWith('c') ?? false) ? "3 puan" :
                (agVeSistemList[i].BagimliVarlik?.StartsWith('d') ?? false) ? "5 puan" :
                (agVeSistemList[i].BagimliVarlik?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = agVeSistemList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = agVeSistemList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = agVeSistemList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = agVeSistemList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = agVeSistemList[i].YedeklemeTipi;
            worksheet.Cells[i + 2, 26].Value = agVeSistemList[i].YedeklemeTuru;
            worksheet.Cells[i + 2, 27].Value = agVeSistemList[i].YedeklemeSikligi;
            worksheet.Cells[i + 2, 28].Value = agVeSistemList[i].YedeklerinSaklamaSuresi;
            worksheet.Cells[i + 2, 29].Value = agVeSistemList[i].YedeklemeAlani;
            worksheet.Cells[i + 2, 30].Value = agVeSistemList[i].YedektenDonusPlani;
            worksheet.Cells[i + 2, 31].Value = agVeSistemList[i].YedeklemeSorumlusu;
            worksheet.Cells[i + 2, 32].Value = agVeSistemList[i].Kriptoloji;
            worksheet.Cells[i + 2, 33].Value = agVeSistemList[i].KriptolojiTuru;
            worksheet.Cells[i + 2, 34].Value = agVeSistemList[i].KullanilanKriptoloji;
            worksheet.Cells[i + 2, 35].Value = agVeSistemList[i].AnahtarSorumlusu;
            worksheet.Cells[i + 2, 36].Value = agVeSistemList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 37].Value = agVeSistemList[i].AnlikMesajlasmaKullanimi;
            worksheet.Cells[i + 2, 38].Value = agVeSistemList[i].BulutBilisim;
            worksheet.Cells[i + 2, 39].Value = agVeSistemList[i].YeniGelismelerveTedarik;
            worksheet.Cells[i + 2, 40].Value = agVeSistemList[i].KritikAltyapiSistemi;
            worksheet.Cells[i + 2, 41].Value = agVeSistemList[i].IpAdresi;
            worksheet.Cells[i + 2, 42].Value = agVeSistemList[i].IsletimSistemi;
            worksheet.Cells[i + 2, 43].Value = agVeSistemList[i].LisansTakipSorumlusu;
            worksheet.Cells[i + 2, 44].Value = agVeSistemList[i].MarkaModel;
            worksheet.Cells[i + 2, 45].Value = agVeSistemList[i].SeriNumarasi;
            worksheet.Cells[i + 2, 46].Value = agVeSistemList[i].ZimmetSahibi;
            worksheet.Cells[i + 2, 47].Value = agVeSistemList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 48].Value = agVeSistemList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 49].Value = agVeSistemList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }
    public async Task<MemoryStream> GenerateExcelUygulama()
    {
        var uygulamaList = await uygulamaService.GetAllExcelAsync();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Uygulamalar");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Yedekleme Tipi";
        worksheet.Cells[1, 26].Value = "Yedekleme Türü";
        worksheet.Cells[1, 27].Value = "Yedekleme Sıklığı";
        worksheet.Cells[1, 28].Value = "Yedeklerin Saklama Süresi";
        worksheet.Cells[1, 29].Value = "Yedekleme Alanı";
        worksheet.Cells[1, 30].Value = "Yedekten Dönüş Planı";
        worksheet.Cells[1, 31].Value = "Yedekleme Sorumlusu";
        worksheet.Cells[1, 32].Value = "Kriptoloji";
        worksheet.Cells[1, 33].Value = "Kriptoloji Türü";
        worksheet.Cells[1, 34].Value = "Kullanılan Kriptoloji";
        worksheet.Cells[1, 35].Value = "Anahtar Sorumlusu";
        worksheet.Cells[1, 36].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 37].Value = "Anlık Mesajlaşma Kullanımı";
        worksheet.Cells[1, 38].Value = "Bulut Bilişim";
        worksheet.Cells[1, 39].Value = "Yeni Gelişmeler ve Tedarik";
        worksheet.Cells[1, 40].Value = "Kritik Altyapı Sistemi";
        worksheet.Cells[1, 41].Value = "Ip Adresi";
        worksheet.Cells[1, 42].Value = "URL Adresi";
        worksheet.Cells[1, 43].Value = "Yazılım Sürümü";
        worksheet.Cells[1, 44].Value = "Yazılım Yayıncısı";
        worksheet.Cells[1, 45].Value = "Edinim Tarihi";
        worksheet.Cells[1, 46].Value = "Lisans Adedi";
        worksheet.Cells[1, 47].Value = "Lisans Takip Sorumlusu";
        worksheet.Cells[1, 48].Value = "Destek Durumu";
        worksheet.Cells[1, 49].Value = "Destek Alınan Tedarikçi";
        worksheet.Cells[1, 50].Value = "Bakım Süreci";
        worksheet.Cells[1, 51].Value = "Bakım Kapsamı";
        worksheet.Cells[1, 52].Value = "Yazılımın Yüklendiği Donanımlar";
        worksheet.Cells[1, 53].Value = "Veritabanı ve Sürümü";
        worksheet.Cells[1, 54].Value = "Veritabanı Versiyonu";
        worksheet.Cells[1, 55].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 56].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 57].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 57])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < uygulamaList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("U", uygulamaList[i].Id);
            worksheet.Cells[i + 2, 2].Value = uygulamaList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = uygulamaList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = uygulamaList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = uygulamaList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = uygulamaList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = uygulamaList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = uygulamaList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = uygulamaList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = uygulamaList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = uygulamaList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = uygulamaList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                (uygulamaList[i].Gizlilik?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].Gizlilik?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].Gizlilik?.StartsWith('c') ?? false) ? "3 puan" :
                (uygulamaList[i].Gizlilik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                (uygulamaList[i].Butunluk?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].Butunluk?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].Butunluk?.StartsWith('c') ?? false) ? "3 puan" :
                (uygulamaList[i].Butunluk?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                (uygulamaList[i].Erisilebilirlik?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].Erisilebilirlik?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].Erisilebilirlik?.StartsWith('c') ?? false) ? "3 puan" :
                (uygulamaList[i].Erisilebilirlik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                (uygulamaList[i].EtkilenenKisiSayisi?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].EtkilenenKisiSayisi?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].EtkilenenKisiSayisi?.StartsWith('c') ?? false) ? "3 puan" :
                (uygulamaList[i].EtkilenenKisiSayisi?.StartsWith('d') ?? false) ? "4 puan" :
                (uygulamaList[i].EtkilenenKisiSayisi?.StartsWith('e') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                (uygulamaList[i].ToplumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].ToplumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].ToplumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                (uygulamaList[i].ToplumsalSonuc?.StartsWith('d') ?? false) ? "5 puan" :
                (uygulamaList[i].ToplumsalSonuc?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                (uygulamaList[i].KurumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].KurumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].KurumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                (uygulamaList[i].SektorelEtki?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].SektorelEtki?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].SektorelEtki?.StartsWith('c') ?? false) ? "3 puan" :
                (uygulamaList[i].SektorelEtki?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                (uygulamaList[i].BagimliVarlik?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].BagimliVarlik?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].BagimliVarlik?.StartsWith('c') ?? false) ? "3 puan" :
                (uygulamaList[i].BagimliVarlik?.StartsWith('d') ?? false) ? "5 puan" :
                (uygulamaList[i].BagimliVarlik?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = uygulamaList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = uygulamaList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = uygulamaList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = uygulamaList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = uygulamaList[i].YedeklemeTipi;
            worksheet.Cells[i + 2, 26].Value = uygulamaList[i].YedeklemeTuru;
            worksheet.Cells[i + 2, 27].Value = uygulamaList[i].YedeklemeSikligi;
            worksheet.Cells[i + 2, 28].Value = uygulamaList[i].YedeklerinSaklamaSuresi;
            worksheet.Cells[i + 2, 29].Value = uygulamaList[i].YedeklemeAlani;
            worksheet.Cells[i + 2, 30].Value = uygulamaList[i].YedektenDonusPlani;
            worksheet.Cells[i + 2, 31].Value = uygulamaList[i].YedeklemeSorumlusu;
            worksheet.Cells[i + 2, 32].Value = uygulamaList[i].Kriptoloji;
            worksheet.Cells[i + 2, 33].Value = uygulamaList[i].KriptolojiTuru;
            worksheet.Cells[i + 2, 34].Value = uygulamaList[i].KullanilanKriptoloji;
            worksheet.Cells[i + 2, 35].Value = uygulamaList[i].AnahtarSorumlusu;
            worksheet.Cells[i + 2, 36].Value = uygulamaList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 37].Value = uygulamaList[i].AnlikMesajlasmaKullanimi;
            worksheet.Cells[i + 2, 38].Value = uygulamaList[i].BulutBilisim;
            worksheet.Cells[i + 2, 39].Value = uygulamaList[i].YeniGelismelerveTedarik;
            worksheet.Cells[i + 2, 40].Value = uygulamaList[i].KritikAltyapiSistemi;
            worksheet.Cells[i + 2, 41].Value = uygulamaList[i].IpAdresi;
            worksheet.Cells[i + 2, 42].Value = uygulamaList[i].UrlAdresi;
            worksheet.Cells[i + 2, 43].Value = uygulamaList[i].YazilimSurumu;
            worksheet.Cells[i + 2, 44].Value = uygulamaList[i].YazilimYayincisi;
            worksheet.Cells[i + 2, 45].Value = uygulamaList[i].EdinimTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 46].Value = uygulamaList[i].LisansAdedi;
            worksheet.Cells[i + 2, 47].Value = uygulamaList[i].LisansTakipSorumlusu;
            worksheet.Cells[i + 2, 48].Value = uygulamaList[i].DestekDurumu;
            worksheet.Cells[i + 2, 49].Value = uygulamaList[i].DestekAlinanTedarikci;
            worksheet.Cells[i + 2, 50].Value = uygulamaList[i].BakimSuresi;
            worksheet.Cells[i + 2, 51].Value = uygulamaList[i].BakimKapsami;
            worksheet.Cells[i + 2, 52].Value = uygulamaList[i].YaziliminYuklendigiDonanimlar;
            worksheet.Cells[i + 2, 53].Value = uygulamaList[i].VeritabaniveSurumu;
            worksheet.Cells[i + 2, 54].Value = uygulamaList[i].VeritabaniVersiyonu;
            worksheet.Cells[i + 2, 55].Value = uygulamaList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 56].Value = uygulamaList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 57].Value = uygulamaList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelUygulama(string search, FilterBag filterBag)
    {
        var uygulamaList = await uygulamaService.GetAllExcelAsync(search, filterBag);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Uygulamalar");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Yedekleme Tipi";
        worksheet.Cells[1, 26].Value = "Yedekleme Türü";
        worksheet.Cells[1, 27].Value = "Yedekleme Sıklığı";
        worksheet.Cells[1, 28].Value = "Yedeklerin Saklama Süresi";
        worksheet.Cells[1, 29].Value = "Yedekleme Alanı";
        worksheet.Cells[1, 30].Value = "Yedekten Dönüş Planı";
        worksheet.Cells[1, 31].Value = "Yedekleme Sorumlusu";
        worksheet.Cells[1, 32].Value = "Kriptoloji";
        worksheet.Cells[1, 33].Value = "Kriptoloji Türü";
        worksheet.Cells[1, 34].Value = "Kullanılan Kriptoloji";
        worksheet.Cells[1, 35].Value = "Anahtar Sorumlusu";
        worksheet.Cells[1, 36].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 37].Value = "Anlık Mesajlaşma Kullanımı";
        worksheet.Cells[1, 38].Value = "Bulut Bilişim";
        worksheet.Cells[1, 39].Value = "Yeni Gelişmeler ve Tedarik";
        worksheet.Cells[1, 40].Value = "Kritik Altyapı Sistemi";
        worksheet.Cells[1, 41].Value = "Ip Adresi";
        worksheet.Cells[1, 42].Value = "URL Adresi";
        worksheet.Cells[1, 43].Value = "Yazılım Sürümü";
        worksheet.Cells[1, 44].Value = "Yazılım Yayıncısı";
        worksheet.Cells[1, 45].Value = "Edinim Tarihi";
        worksheet.Cells[1, 46].Value = "Lisans Adedi";
        worksheet.Cells[1, 47].Value = "Lisans Takip Sorumlusu";
        worksheet.Cells[1, 48].Value = "Destek Durumu";
        worksheet.Cells[1, 49].Value = "Destek Alınan Tedarikçi";
        worksheet.Cells[1, 50].Value = "Bakım Süreci";
        worksheet.Cells[1, 51].Value = "Bakım Kapsamı";
        worksheet.Cells[1, 52].Value = "Yazılımın Yüklendiği Donanımlar";
        worksheet.Cells[1, 53].Value = "Veritabanı ve Sürümü";
        worksheet.Cells[1, 54].Value = "Veritabanı Versiyonu";
        worksheet.Cells[1, 55].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 56].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 57].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 57])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < uygulamaList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("U", uygulamaList[i].Id);
            worksheet.Cells[i + 2, 2].Value = uygulamaList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = uygulamaList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = uygulamaList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = uygulamaList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = uygulamaList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = uygulamaList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = uygulamaList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = uygulamaList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = uygulamaList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = uygulamaList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = uygulamaList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                (uygulamaList[i].Gizlilik?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].Gizlilik?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].Gizlilik?.StartsWith('c') ?? false) ? "3 puan" :
                (uygulamaList[i].Gizlilik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                (uygulamaList[i].Butunluk?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].Butunluk?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].Butunluk?.StartsWith('c') ?? false) ? "3 puan" :
                (uygulamaList[i].Butunluk?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                (uygulamaList[i].Erisilebilirlik?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].Erisilebilirlik?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].Erisilebilirlik?.StartsWith('c') ?? false) ? "3 puan" :
                (uygulamaList[i].Erisilebilirlik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                (uygulamaList[i].EtkilenenKisiSayisi?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].EtkilenenKisiSayisi?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].EtkilenenKisiSayisi?.StartsWith('c') ?? false) ? "3 puan" :
                (uygulamaList[i].EtkilenenKisiSayisi?.StartsWith('d') ?? false) ? "4 puan" :
                (uygulamaList[i].EtkilenenKisiSayisi?.StartsWith('e') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                (uygulamaList[i].ToplumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].ToplumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].ToplumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                (uygulamaList[i].ToplumsalSonuc?.StartsWith('d') ?? false) ? "5 puan" :
                (uygulamaList[i].ToplumsalSonuc?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                (uygulamaList[i].KurumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].KurumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].KurumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                (uygulamaList[i].SektorelEtki?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].SektorelEtki?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].SektorelEtki?.StartsWith('c') ?? false) ? "3 puan" :
                (uygulamaList[i].SektorelEtki?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                (uygulamaList[i].BagimliVarlik?.StartsWith('a') ?? false) ? "1 puan" :
                (uygulamaList[i].BagimliVarlik?.StartsWith('b') ?? false) ? "2 puan" :
                (uygulamaList[i].BagimliVarlik?.StartsWith('c') ?? false) ? "3 puan" :
                (uygulamaList[i].BagimliVarlik?.StartsWith('d') ?? false) ? "5 puan" :
                (uygulamaList[i].BagimliVarlik?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = uygulamaList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = uygulamaList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = uygulamaList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = uygulamaList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = uygulamaList[i].YedeklemeTipi;
            worksheet.Cells[i + 2, 26].Value = uygulamaList[i].YedeklemeTuru;
            worksheet.Cells[i + 2, 27].Value = uygulamaList[i].YedeklemeSikligi;
            worksheet.Cells[i + 2, 28].Value = uygulamaList[i].YedeklerinSaklamaSuresi;
            worksheet.Cells[i + 2, 29].Value = uygulamaList[i].YedeklemeAlani;
            worksheet.Cells[i + 2, 30].Value = uygulamaList[i].YedektenDonusPlani;
            worksheet.Cells[i + 2, 31].Value = uygulamaList[i].YedeklemeSorumlusu;
            worksheet.Cells[i + 2, 32].Value = uygulamaList[i].Kriptoloji;
            worksheet.Cells[i + 2, 33].Value = uygulamaList[i].KriptolojiTuru;
            worksheet.Cells[i + 2, 34].Value = uygulamaList[i].KullanilanKriptoloji;
            worksheet.Cells[i + 2, 35].Value = uygulamaList[i].AnahtarSorumlusu;
            worksheet.Cells[i + 2, 36].Value = uygulamaList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 37].Value = uygulamaList[i].AnlikMesajlasmaKullanimi;
            worksheet.Cells[i + 2, 38].Value = uygulamaList[i].BulutBilisim;
            worksheet.Cells[i + 2, 39].Value = uygulamaList[i].YeniGelismelerveTedarik;
            worksheet.Cells[i + 2, 40].Value = uygulamaList[i].KritikAltyapiSistemi;
            worksheet.Cells[i + 2, 41].Value = uygulamaList[i].IpAdresi;
            worksheet.Cells[i + 2, 42].Value = uygulamaList[i].UrlAdresi;
            worksheet.Cells[i + 2, 43].Value = uygulamaList[i].YazilimSurumu;
            worksheet.Cells[i + 2, 44].Value = uygulamaList[i].YazilimYayincisi;
            worksheet.Cells[i + 2, 45].Value = uygulamaList[i].EdinimTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 46].Value = uygulamaList[i].LisansAdedi;
            worksheet.Cells[i + 2, 47].Value = uygulamaList[i].LisansTakipSorumlusu;
            worksheet.Cells[i + 2, 48].Value = uygulamaList[i].DestekDurumu;
            worksheet.Cells[i + 2, 49].Value = uygulamaList[i].DestekAlinanTedarikci;
            worksheet.Cells[i + 2, 50].Value = uygulamaList[i].BakimSuresi;
            worksheet.Cells[i + 2, 51].Value = uygulamaList[i].BakimKapsami;
            worksheet.Cells[i + 2, 52].Value = uygulamaList[i].YaziliminYuklendigiDonanimlar;
            worksheet.Cells[i + 2, 53].Value = uygulamaList[i].VeritabaniveSurumu;
            worksheet.Cells[i + 2, 54].Value = uygulamaList[i].VeritabaniVersiyonu;
            worksheet.Cells[i + 2, 55].Value = uygulamaList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 56].Value = uygulamaList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 57].Value = uygulamaList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelTasinabilirCihaz()
    {
        var tasinabilirCihazveOrtamList = await tasinabilirCihazveOrtamService.GetAllExcelAsync();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Taşınabilir Cihaz ve Ortam");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Yedekleme Tipi";
        worksheet.Cells[1, 26].Value = "Yedekleme Türü";
        worksheet.Cells[1, 27].Value = "Yedekleme Sıklığı";
        worksheet.Cells[1, 28].Value = "Yedeklerin Saklama Süresi";
        worksheet.Cells[1, 29].Value = "Yedekleme Alanı";
        worksheet.Cells[1, 30].Value = "Yedekten Dönüş Planı";
        worksheet.Cells[1, 31].Value = "Yedekleme Sorumlusu";
        worksheet.Cells[1, 32].Value = "Kriptoloji";
        worksheet.Cells[1, 33].Value = "Kriptoloji Türü";
        worksheet.Cells[1, 34].Value = "Kullanılan Kriptoloji";
        worksheet.Cells[1, 35].Value = "Anahtar Sorumlusu";
        worksheet.Cells[1, 36].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 37].Value = "Anlık Mesajlaşma Kullanımı";
        worksheet.Cells[1, 38].Value = "Bulut Bilişim";
        worksheet.Cells[1, 39].Value = "Yeni Gelişmeler ve Tedarik";
        worksheet.Cells[1, 40].Value = "Kritik Altyapı Sistemi";
        worksheet.Cells[1, 41].Value = "Ip Adresi";
        worksheet.Cells[1, 42].Value = "İşletim Sistemi";
        worksheet.Cells[1, 43].Value = "Lisans Takip Sorumlusu";
        worksheet.Cells[1, 44].Value = "Marka/Model";
        worksheet.Cells[1, 45].Value = "Seri Numarası";
        worksheet.Cells[1, 46].Value = "Zimmet Sahibi";
        worksheet.Cells[1, 47].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 48].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 49].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 49])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < tasinabilirCihazveOrtamList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("TCO", tasinabilirCihazveOrtamList[i].Id);
            worksheet.Cells[i + 2, 2].Value = tasinabilirCihazveOrtamList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = tasinabilirCihazveOrtamList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = tasinabilirCihazveOrtamList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = tasinabilirCihazveOrtamList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = tasinabilirCihazveOrtamList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = tasinabilirCihazveOrtamList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = tasinabilirCihazveOrtamList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = tasinabilirCihazveOrtamList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = tasinabilirCihazveOrtamList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = tasinabilirCihazveOrtamList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = tasinabilirCihazveOrtamList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                (tasinabilirCihazveOrtamList[i].Gizlilik?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].Gizlilik?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].Gizlilik?.StartsWith('c') ?? false) ? "3 puan" :
                (tasinabilirCihazveOrtamList[i].Gizlilik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                (tasinabilirCihazveOrtamList[i].Butunluk?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].Butunluk?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].Butunluk?.StartsWith('c') ?? false) ? "3 puan" :
                (tasinabilirCihazveOrtamList[i].Butunluk?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                (tasinabilirCihazveOrtamList[i].Erisilebilirlik?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].Erisilebilirlik?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].Erisilebilirlik?.StartsWith('c') ?? false) ? "3 puan" :
                (tasinabilirCihazveOrtamList[i].Erisilebilirlik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                (tasinabilirCihazveOrtamList[i].EtkilenenKisiSayisi?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].EtkilenenKisiSayisi?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].EtkilenenKisiSayisi?.StartsWith('c') ?? false) ? "3 puan" :
                (tasinabilirCihazveOrtamList[i].EtkilenenKisiSayisi?.StartsWith('d') ?? false) ? "4 puan" :
                (tasinabilirCihazveOrtamList[i].EtkilenenKisiSayisi?.StartsWith('e') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                (tasinabilirCihazveOrtamList[i].ToplumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].ToplumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].ToplumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                (tasinabilirCihazveOrtamList[i].ToplumsalSonuc?.StartsWith('d') ?? false) ? "5 puan" :
                (tasinabilirCihazveOrtamList[i].ToplumsalSonuc?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                (tasinabilirCihazveOrtamList[i].KurumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].KurumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].KurumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                (tasinabilirCihazveOrtamList[i].SektorelEtki?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].SektorelEtki?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].SektorelEtki?.StartsWith('c') ?? false) ? "3 puan" :
                (tasinabilirCihazveOrtamList[i].SektorelEtki?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                (tasinabilirCihazveOrtamList[i].BagimliVarlik?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].BagimliVarlik?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].BagimliVarlik?.StartsWith('c') ?? false) ? "3 puan" :
                (tasinabilirCihazveOrtamList[i].BagimliVarlik?.StartsWith('d') ?? false) ? "5 puan" :
                (tasinabilirCihazveOrtamList[i].BagimliVarlik?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = tasinabilirCihazveOrtamList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = tasinabilirCihazveOrtamList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = tasinabilirCihazveOrtamList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = tasinabilirCihazveOrtamList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = tasinabilirCihazveOrtamList[i].YedeklemeTipi;
            worksheet.Cells[i + 2, 26].Value = tasinabilirCihazveOrtamList[i].YedeklemeTuru;
            worksheet.Cells[i + 2, 27].Value = tasinabilirCihazveOrtamList[i].YedeklemeSikligi;
            worksheet.Cells[i + 2, 28].Value = tasinabilirCihazveOrtamList[i].YedeklerinSaklamaSuresi;
            worksheet.Cells[i + 2, 29].Value = tasinabilirCihazveOrtamList[i].YedeklemeAlani;
            worksheet.Cells[i + 2, 30].Value = tasinabilirCihazveOrtamList[i].YedektenDonusPlani;
            worksheet.Cells[i + 2, 31].Value = tasinabilirCihazveOrtamList[i].YedeklemeSorumlusu;
            worksheet.Cells[i + 2, 32].Value = tasinabilirCihazveOrtamList[i].Kriptoloji;
            worksheet.Cells[i + 2, 33].Value = tasinabilirCihazveOrtamList[i].KriptolojiTuru;
            worksheet.Cells[i + 2, 34].Value = tasinabilirCihazveOrtamList[i].KullanilanKriptoloji;
            worksheet.Cells[i + 2, 35].Value = tasinabilirCihazveOrtamList[i].AnahtarSorumlusu;
            worksheet.Cells[i + 2, 36].Value = tasinabilirCihazveOrtamList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 37].Value =
                tasinabilirCihazveOrtamList[i].AnlikMesajlasmaKullanimi;
            worksheet.Cells[i + 2, 38].Value = tasinabilirCihazveOrtamList[i].BulutBilisim;
            worksheet.Cells[i + 2, 39].Value =
                tasinabilirCihazveOrtamList[i].YeniGelismelerveTedarik;
            worksheet.Cells[i + 2, 40].Value = tasinabilirCihazveOrtamList[i].KritikAltyapiSistemi;
            worksheet.Cells[i + 2, 41].Value = tasinabilirCihazveOrtamList[i].IpAdresi;
            worksheet.Cells[i + 2, 42].Value = tasinabilirCihazveOrtamList[i].IsletimSistemi;
            worksheet.Cells[i + 2, 43].Value = tasinabilirCihazveOrtamList[i].LisansTakipSorumlusu;
            worksheet.Cells[i + 2, 44].Value = tasinabilirCihazveOrtamList[i].MarkaModel;
            worksheet.Cells[i + 2, 45].Value = tasinabilirCihazveOrtamList[i].SeriNumarasi;
            worksheet.Cells[i + 2, 46].Value = tasinabilirCihazveOrtamList[i].ZimmetSahibi;
            worksheet.Cells[i + 2, 47].Value =
                tasinabilirCihazveOrtamList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 48].Value =
                tasinabilirCihazveOrtamList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 49].Value =
                tasinabilirCihazveOrtamList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelTasinabilirCihaz(
        string search, FilterBag filterBag)
    {
        var tasinabilirCihazveOrtamList =
            await tasinabilirCihazveOrtamService.GetAllExcelAsync(search, filterBag);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Taşınabilir Cihaz ve Ortam");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Yedekleme Tipi";
        worksheet.Cells[1, 26].Value = "Yedekleme Türü";
        worksheet.Cells[1, 27].Value = "Yedekleme Sıklığı";
        worksheet.Cells[1, 28].Value = "Yedeklerin Saklama Süresi";
        worksheet.Cells[1, 29].Value = "Yedekleme Alanı";
        worksheet.Cells[1, 30].Value = "Yedekten Dönüş Planı";
        worksheet.Cells[1, 31].Value = "Yedekleme Sorumlusu";
        worksheet.Cells[1, 32].Value = "Kriptoloji";
        worksheet.Cells[1, 33].Value = "Kriptoloji Türü";
        worksheet.Cells[1, 34].Value = "Kullanılan Kriptoloji";
        worksheet.Cells[1, 35].Value = "Anahtar Sorumlusu";
        worksheet.Cells[1, 36].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 37].Value = "Anlık Mesajlaşma Kullanımı";
        worksheet.Cells[1, 38].Value = "Bulut Bilişim";
        worksheet.Cells[1, 39].Value = "Yeni Gelişmeler ve Tedarik";
        worksheet.Cells[1, 40].Value = "Kritik Altyapı Sistemi";
        worksheet.Cells[1, 41].Value = "Ip Adresi";
        worksheet.Cells[1, 42].Value = "İşletim Sistemi";
        worksheet.Cells[1, 43].Value = "Lisans Takip Sorumlusu";
        worksheet.Cells[1, 44].Value = "Marka/Model";
        worksheet.Cells[1, 45].Value = "Seri Numarası";
        worksheet.Cells[1, 46].Value = "Zimmet Sahibi";
        worksheet.Cells[1, 47].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 48].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 49].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 49])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < tasinabilirCihazveOrtamList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("TCO", tasinabilirCihazveOrtamList[i].Id);
            worksheet.Cells[i + 2, 2].Value = tasinabilirCihazveOrtamList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = tasinabilirCihazveOrtamList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = tasinabilirCihazveOrtamList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = tasinabilirCihazveOrtamList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = tasinabilirCihazveOrtamList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = tasinabilirCihazveOrtamList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = tasinabilirCihazveOrtamList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = tasinabilirCihazveOrtamList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = tasinabilirCihazveOrtamList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = tasinabilirCihazveOrtamList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = tasinabilirCihazveOrtamList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                (tasinabilirCihazveOrtamList[i].Gizlilik?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].Gizlilik?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].Gizlilik?.StartsWith('c') ?? false) ? "3 puan" :
                (tasinabilirCihazveOrtamList[i].Gizlilik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                (tasinabilirCihazveOrtamList[i].Butunluk?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].Butunluk?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].Butunluk?.StartsWith('c') ?? false) ? "3 puan" :
                (tasinabilirCihazveOrtamList[i].Butunluk?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                (tasinabilirCihazveOrtamList[i].Erisilebilirlik?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].Erisilebilirlik?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].Erisilebilirlik?.StartsWith('c') ?? false) ? "3 puan" :
                (tasinabilirCihazveOrtamList[i].Erisilebilirlik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                (tasinabilirCihazveOrtamList[i].EtkilenenKisiSayisi?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].EtkilenenKisiSayisi?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].EtkilenenKisiSayisi?.StartsWith('c') ?? false) ? "3 puan" :
                (tasinabilirCihazveOrtamList[i].EtkilenenKisiSayisi?.StartsWith('d') ?? false) ? "4 puan" :
                (tasinabilirCihazveOrtamList[i].EtkilenenKisiSayisi?.StartsWith('e') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                (tasinabilirCihazveOrtamList[i].ToplumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].ToplumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].ToplumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                (tasinabilirCihazveOrtamList[i].ToplumsalSonuc?.StartsWith('d') ?? false) ? "5 puan" :
                (tasinabilirCihazveOrtamList[i].ToplumsalSonuc?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                (tasinabilirCihazveOrtamList[i].KurumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].KurumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].KurumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                (tasinabilirCihazveOrtamList[i].SektorelEtki?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].SektorelEtki?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].SektorelEtki?.StartsWith('c') ?? false) ? "3 puan" :
                (tasinabilirCihazveOrtamList[i].SektorelEtki?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                (tasinabilirCihazveOrtamList[i].BagimliVarlik?.StartsWith('a') ?? false) ? "1 puan" :
                (tasinabilirCihazveOrtamList[i].BagimliVarlik?.StartsWith('b') ?? false) ? "2 puan" :
                (tasinabilirCihazveOrtamList[i].BagimliVarlik?.StartsWith('c') ?? false) ? "3 puan" :
                (tasinabilirCihazveOrtamList[i].BagimliVarlik?.StartsWith('d') ?? false) ? "5 puan" :
                (tasinabilirCihazveOrtamList[i].BagimliVarlik?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = tasinabilirCihazveOrtamList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = tasinabilirCihazveOrtamList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = tasinabilirCihazveOrtamList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = tasinabilirCihazveOrtamList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = tasinabilirCihazveOrtamList[i].YedeklemeTipi;
            worksheet.Cells[i + 2, 26].Value = tasinabilirCihazveOrtamList[i].YedeklemeTuru;
            worksheet.Cells[i + 2, 27].Value = tasinabilirCihazveOrtamList[i].YedeklemeSikligi;
            worksheet.Cells[i + 2, 28].Value = tasinabilirCihazveOrtamList[i].YedeklerinSaklamaSuresi;
            worksheet.Cells[i + 2, 29].Value = tasinabilirCihazveOrtamList[i].YedeklemeAlani;
            worksheet.Cells[i + 2, 30].Value = tasinabilirCihazveOrtamList[i].YedektenDonusPlani;
            worksheet.Cells[i + 2, 31].Value = tasinabilirCihazveOrtamList[i].YedeklemeSorumlusu;
            worksheet.Cells[i + 2, 32].Value = tasinabilirCihazveOrtamList[i].Kriptoloji;
            worksheet.Cells[i + 2, 33].Value = tasinabilirCihazveOrtamList[i].KriptolojiTuru;
            worksheet.Cells[i + 2, 34].Value = tasinabilirCihazveOrtamList[i].KullanilanKriptoloji;
            worksheet.Cells[i + 2, 35].Value = tasinabilirCihazveOrtamList[i].AnahtarSorumlusu;
            worksheet.Cells[i + 2, 36].Value = tasinabilirCihazveOrtamList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 37].Value =
                tasinabilirCihazveOrtamList[i].AnlikMesajlasmaKullanimi;
            worksheet.Cells[i + 2, 38].Value = tasinabilirCihazveOrtamList[i].BulutBilisim;
            worksheet.Cells[i + 2, 39].Value =
                tasinabilirCihazveOrtamList[i].YeniGelismelerveTedarik;
            worksheet.Cells[i + 2, 40].Value = tasinabilirCihazveOrtamList[i].KritikAltyapiSistemi;
            worksheet.Cells[i + 2, 41].Value = tasinabilirCihazveOrtamList[i].IpAdresi;
            worksheet.Cells[i + 2, 42].Value = tasinabilirCihazveOrtamList[i].IsletimSistemi;
            worksheet.Cells[i + 2, 43].Value = tasinabilirCihazveOrtamList[i].LisansTakipSorumlusu;
            worksheet.Cells[i + 2, 44].Value = tasinabilirCihazveOrtamList[i].MarkaModel;
            worksheet.Cells[i + 2, 45].Value = tasinabilirCihazveOrtamList[i].SeriNumarasi;
            worksheet.Cells[i + 2, 46].Value = tasinabilirCihazveOrtamList[i].ZimmetSahibi;
            worksheet.Cells[i + 2, 47].Value =
                tasinabilirCihazveOrtamList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 48].Value =
                tasinabilirCihazveOrtamList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 49].Value =
                tasinabilirCihazveOrtamList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelIoT()
    {
        var ioTCihazList = await ioTCihazService.GetAllExcelAsync();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("IoT Cihazları");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Yedekleme Tipi";
        worksheet.Cells[1, 26].Value = "Yedekleme Türü";
        worksheet.Cells[1, 27].Value = "Yedekleme Sıklığı";
        worksheet.Cells[1, 28].Value = "Yedeklerin Saklama Süresi";
        worksheet.Cells[1, 29].Value = "Yedekleme Alanı";
        worksheet.Cells[1, 30].Value = "Yedekten Dönüş Planı";
        worksheet.Cells[1, 31].Value = "Yedekleme Sorumlusu";
        worksheet.Cells[1, 32].Value = "Kriptoloji";
        worksheet.Cells[1, 33].Value = "Kriptoloji Türü";
        worksheet.Cells[1, 34].Value = "Kullanılan Kriptoloji";
        worksheet.Cells[1, 35].Value = "Anahtar Sorumlusu";
        worksheet.Cells[1, 36].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 37].Value = "Anlık Mesajlaşma Kullanımı";
        worksheet.Cells[1, 38].Value = "Bulut Bilişim";
        worksheet.Cells[1, 39].Value = "Yeni Gelişmeler ve Tedarik";
        worksheet.Cells[1, 40].Value = "Kritik Altyapı Sistemi";
        worksheet.Cells[1, 41].Value = "Ip Adresi";
        worksheet.Cells[1, 42].Value = "İşletim Sistemi";
        worksheet.Cells[1, 43].Value = "Lisans Takip Sorumlusu";
        worksheet.Cells[1, 44].Value = "Marka/Model";
        worksheet.Cells[1, 45].Value = "Seri Numarası";
        worksheet.Cells[1, 46].Value = "Zimmet Sahibi";
        worksheet.Cells[1, 47].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 48].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 49].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 49])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < ioTCihazList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("IoT", ioTCihazList[i].Id);
            worksheet.Cells[i + 2, 2].Value = ioTCihazList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = ioTCihazList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = ioTCihazList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = ioTCihazList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = ioTCihazList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = ioTCihazList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = ioTCihazList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = ioTCihazList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = ioTCihazList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = ioTCihazList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = ioTCihazList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                (ioTCihazList[i].Gizlilik?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].Gizlilik?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].Gizlilik?.StartsWith('c') ?? false) ? "3 puan" :
                (ioTCihazList[i].Gizlilik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                (ioTCihazList[i].Butunluk?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].Butunluk?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].Butunluk?.StartsWith('c') ?? false) ? "3 puan" :
                (ioTCihazList[i].Butunluk?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                (ioTCihazList[i].Erisilebilirlik?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].Erisilebilirlik?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].Erisilebilirlik?.StartsWith('c') ?? false) ? "3 puan" :
                (ioTCihazList[i].Erisilebilirlik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                (ioTCihazList[i].EtkilenenKisiSayisi?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].EtkilenenKisiSayisi?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].EtkilenenKisiSayisi?.StartsWith('c') ?? false) ? "3 puan" :
                (ioTCihazList[i].EtkilenenKisiSayisi?.StartsWith('d') ?? false) ? "4 puan" :
                (ioTCihazList[i].EtkilenenKisiSayisi?.StartsWith('e') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                (ioTCihazList[i].ToplumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].ToplumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].ToplumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                (ioTCihazList[i].ToplumsalSonuc?.StartsWith('d') ?? false) ? "5 puan" :
                (ioTCihazList[i].ToplumsalSonuc?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                (ioTCihazList[i].KurumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].KurumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].KurumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                (ioTCihazList[i].SektorelEtki?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].SektorelEtki?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].SektorelEtki?.StartsWith('c') ?? false) ? "3 puan" :
                (ioTCihazList[i].SektorelEtki?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                (ioTCihazList[i].BagimliVarlik?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].BagimliVarlik?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].BagimliVarlik?.StartsWith('c') ?? false) ? "3 puan" :
                (ioTCihazList[i].BagimliVarlik?.StartsWith('d') ?? false) ? "5 puan" :
                (ioTCihazList[i].BagimliVarlik?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = ioTCihazList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = ioTCihazList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = ioTCihazList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = ioTCihazList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = ioTCihazList[i].YedeklemeTipi;
            worksheet.Cells[i + 2, 26].Value = ioTCihazList[i].YedeklemeTuru;
            worksheet.Cells[i + 2, 27].Value = ioTCihazList[i].YedeklemeSikligi;
            worksheet.Cells[i + 2, 28].Value = ioTCihazList[i].YedeklerinSaklamaSuresi;
            worksheet.Cells[i + 2, 29].Value = ioTCihazList[i].YedeklemeAlani;
            worksheet.Cells[i + 2, 30].Value = ioTCihazList[i].YedektenDonusPlani;
            worksheet.Cells[i + 2, 31].Value = ioTCihazList[i].YedeklemeSorumlusu;
            worksheet.Cells[i + 2, 32].Value = ioTCihazList[i].Kriptoloji;
            worksheet.Cells[i + 2, 33].Value = ioTCihazList[i].KriptolojiTuru;
            worksheet.Cells[i + 2, 34].Value = ioTCihazList[i].KullanilanKriptoloji;
            worksheet.Cells[i + 2, 35].Value = ioTCihazList[i].AnahtarSorumlusu;
            worksheet.Cells[i + 2, 36].Value = ioTCihazList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 37].Value = ioTCihazList[i].AnlikMesajlasmaKullanimi;
            worksheet.Cells[i + 2, 38].Value = ioTCihazList[i].BulutBilisim;
            worksheet.Cells[i + 2, 39].Value = ioTCihazList[i].YeniGelismelerveTedarik;
            worksheet.Cells[i + 2, 40].Value = ioTCihazList[i].KritikAltyapiSistemi;
            worksheet.Cells[i + 2, 41].Value = ioTCihazList[i].IpAdresi;
            worksheet.Cells[i + 2, 42].Value = ioTCihazList[i].IsletimSistemi;
            worksheet.Cells[i + 2, 43].Value = ioTCihazList[i].LisansTakipSorumlusu;
            worksheet.Cells[i + 2, 44].Value = ioTCihazList[i].MarkaModel;
            worksheet.Cells[i + 2, 45].Value = ioTCihazList[i].SeriNumarasi;
            worksheet.Cells[i + 2, 46].Value = ioTCihazList[i].ZimmetSahibi;
            worksheet.Cells[i + 2, 47].Value = ioTCihazList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 48].Value = ioTCihazList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 49].Value = ioTCihazList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelIoT(string search, FilterBag filterBag)
    {
        var ioTCihazList = await ioTCihazService.GetAllExcelAsync(search, filterBag);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("IoT Cihazları");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Yedekleme Tipi";
        worksheet.Cells[1, 26].Value = "Yedekleme Türü";
        worksheet.Cells[1, 27].Value = "Yedekleme Sıklığı";
        worksheet.Cells[1, 28].Value = "Yedeklerin Saklama Süresi";
        worksheet.Cells[1, 29].Value = "Yedekleme Alanı";
        worksheet.Cells[1, 30].Value = "Yedekten Dönüş Planı";
        worksheet.Cells[1, 31].Value = "Yedekleme Sorumlusu";
        worksheet.Cells[1, 32].Value = "Kriptoloji";
        worksheet.Cells[1, 33].Value = "Kriptoloji Türü";
        worksheet.Cells[1, 34].Value = "Kullanılan Kriptoloji";
        worksheet.Cells[1, 35].Value = "Anahtar Sorumlusu";
        worksheet.Cells[1, 36].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 37].Value = "Anlık Mesajlaşma Kullanımı";
        worksheet.Cells[1, 38].Value = "Bulut Bilişim";
        worksheet.Cells[1, 39].Value = "Yeni Gelişmeler ve Tedarik";
        worksheet.Cells[1, 40].Value = "Kritik Altyapı Sistemi";
        worksheet.Cells[1, 41].Value = "Ip Adresi";
        worksheet.Cells[1, 42].Value = "İşletim Sistemi";
        worksheet.Cells[1, 43].Value = "Lisans Takip Sorumlusu";
        worksheet.Cells[1, 44].Value = "Marka/Model";
        worksheet.Cells[1, 45].Value = "Seri Numarası";
        worksheet.Cells[1, 46].Value = "Zimmet Sahibi";
        worksheet.Cells[1, 47].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 48].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 49].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 49])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < ioTCihazList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("IoT", ioTCihazList[i].Id);
            worksheet.Cells[i + 2, 2].Value = ioTCihazList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = ioTCihazList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = ioTCihazList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = ioTCihazList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = ioTCihazList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = ioTCihazList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = ioTCihazList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = ioTCihazList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = ioTCihazList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = ioTCihazList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = ioTCihazList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                (ioTCihazList[i].Gizlilik?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].Gizlilik?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].Gizlilik?.StartsWith('c') ?? false) ? "3 puan" :
                (ioTCihazList[i].Gizlilik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                (ioTCihazList[i].Butunluk?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].Butunluk?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].Butunluk?.StartsWith('c') ?? false) ? "3 puan" :
                (ioTCihazList[i].Butunluk?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                (ioTCihazList[i].Erisilebilirlik?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].Erisilebilirlik?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].Erisilebilirlik?.StartsWith('c') ?? false) ? "3 puan" :
                (ioTCihazList[i].Erisilebilirlik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                (ioTCihazList[i].EtkilenenKisiSayisi?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].EtkilenenKisiSayisi?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].EtkilenenKisiSayisi?.StartsWith('c') ?? false) ? "3 puan" :
                (ioTCihazList[i].EtkilenenKisiSayisi?.StartsWith('d') ?? false) ? "4 puan" :
                (ioTCihazList[i].EtkilenenKisiSayisi?.StartsWith('e') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                (ioTCihazList[i].ToplumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].ToplumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].ToplumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                (ioTCihazList[i].ToplumsalSonuc?.StartsWith('d') ?? false) ? "5 puan" :
                (ioTCihazList[i].ToplumsalSonuc?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                (ioTCihazList[i].KurumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].KurumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].KurumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                (ioTCihazList[i].SektorelEtki?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].SektorelEtki?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].SektorelEtki?.StartsWith('c') ?? false) ? "3 puan" :
                (ioTCihazList[i].SektorelEtki?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                (ioTCihazList[i].BagimliVarlik?.StartsWith('a') ?? false) ? "1 puan" :
                (ioTCihazList[i].BagimliVarlik?.StartsWith('b') ?? false) ? "2 puan" :
                (ioTCihazList[i].BagimliVarlik?.StartsWith('c') ?? false) ? "3 puan" :
                (ioTCihazList[i].BagimliVarlik?.StartsWith('d') ?? false) ? "5 puan" :
                (ioTCihazList[i].BagimliVarlik?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = ioTCihazList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = ioTCihazList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = ioTCihazList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = ioTCihazList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = ioTCihazList[i].YedeklemeTipi;
            worksheet.Cells[i + 2, 26].Value = ioTCihazList[i].YedeklemeTuru;
            worksheet.Cells[i + 2, 27].Value = ioTCihazList[i].YedeklemeSikligi;
            worksheet.Cells[i + 2, 28].Value = ioTCihazList[i].YedeklerinSaklamaSuresi;
            worksheet.Cells[i + 2, 29].Value = ioTCihazList[i].YedeklemeAlani;
            worksheet.Cells[i + 2, 30].Value = ioTCihazList[i].YedektenDonusPlani;
            worksheet.Cells[i + 2, 31].Value = ioTCihazList[i].YedeklemeSorumlusu;
            worksheet.Cells[i + 2, 32].Value = ioTCihazList[i].Kriptoloji;
            worksheet.Cells[i + 2, 33].Value = ioTCihazList[i].KriptolojiTuru;
            worksheet.Cells[i + 2, 34].Value = ioTCihazList[i].KullanilanKriptoloji;
            worksheet.Cells[i + 2, 35].Value = ioTCihazList[i].AnahtarSorumlusu;
            worksheet.Cells[i + 2, 36].Value = ioTCihazList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 37].Value = ioTCihazList[i].AnlikMesajlasmaKullanimi;
            worksheet.Cells[i + 2, 38].Value = ioTCihazList[i].BulutBilisim;
            worksheet.Cells[i + 2, 39].Value = ioTCihazList[i].YeniGelismelerveTedarik;
            worksheet.Cells[i + 2, 40].Value = ioTCihazList[i].KritikAltyapiSistemi;
            worksheet.Cells[i + 2, 41].Value = ioTCihazList[i].IpAdresi;
            worksheet.Cells[i + 2, 42].Value = ioTCihazList[i].IsletimSistemi;
            worksheet.Cells[i + 2, 43].Value = ioTCihazList[i].LisansTakipSorumlusu;
            worksheet.Cells[i + 2, 44].Value = ioTCihazList[i].MarkaModel;
            worksheet.Cells[i + 2, 45].Value = ioTCihazList[i].SeriNumarasi;
            worksheet.Cells[i + 2, 46].Value = ioTCihazList[i].ZimmetSahibi;
            worksheet.Cells[i + 2, 47].Value = ioTCihazList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 48].Value = ioTCihazList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 49].Value = ioTCihazList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelFizikselMekan()
    {
        var fizikselMekanList = await fizikselMekanService.GetAllExcelAsync();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Fiziksel Mekanlar");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 26].Value = "Basılı Bilgi";
        worksheet.Cells[1, 27].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 28].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 29].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 29])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < fizikselMekanList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("FM", fizikselMekanList[i].Id);
            worksheet.Cells[i + 2, 2].Value = fizikselMekanList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = fizikselMekanList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = fizikselMekanList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = fizikselMekanList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = fizikselMekanList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = fizikselMekanList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = fizikselMekanList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = fizikselMekanList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = fizikselMekanList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = fizikselMekanList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = fizikselMekanList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                (fizikselMekanList[i].Gizlilik?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].Gizlilik?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].Gizlilik?.StartsWith('c') ?? false) ? "3 puan" :
                (fizikselMekanList[i].Gizlilik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                (fizikselMekanList[i].Butunluk?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].Butunluk?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].Butunluk?.StartsWith('c') ?? false) ? "3 puan" :
                (fizikselMekanList[i].Butunluk?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                (fizikselMekanList[i].Erisilebilirlik?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].Erisilebilirlik?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].Erisilebilirlik?.StartsWith('c') ?? false) ? "3 puan" :
                (fizikselMekanList[i].Erisilebilirlik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                (fizikselMekanList[i].EtkilenenKisiSayisi?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].EtkilenenKisiSayisi?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].EtkilenenKisiSayisi?.StartsWith('c') ?? false) ? "3 puan" :
                (fizikselMekanList[i].EtkilenenKisiSayisi?.StartsWith('d') ?? false) ? "4 puan" :
                (fizikselMekanList[i].EtkilenenKisiSayisi?.StartsWith('e') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                (fizikselMekanList[i].ToplumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].ToplumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].ToplumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                (fizikselMekanList[i].ToplumsalSonuc?.StartsWith('d') ?? false) ? "5 puan" :
                (fizikselMekanList[i].ToplumsalSonuc?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                (fizikselMekanList[i].KurumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].KurumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].KurumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                (fizikselMekanList[i].SektorelEtki?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].SektorelEtki?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].SektorelEtki?.StartsWith('c') ?? false) ? "3 puan" :
                (fizikselMekanList[i].SektorelEtki?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                (fizikselMekanList[i].BagimliVarlik?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].BagimliVarlik?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].BagimliVarlik?.StartsWith('c') ?? false) ? "3 puan" :
                (fizikselMekanList[i].BagimliVarlik?.StartsWith('d') ?? false) ? "5 puan" :
                (fizikselMekanList[i].BagimliVarlik?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = fizikselMekanList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = fizikselMekanList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = fizikselMekanList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = fizikselMekanList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = fizikselMekanList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 26].Value = fizikselMekanList[i].BasiliBilgi;
            worksheet.Cells[i + 2, 27].Value = fizikselMekanList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 28].Value = fizikselMekanList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 29].Value = fizikselMekanList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelFizikselMekan(string search, FilterBag filterBag)
    {
        var fizikselMekanList =
            await fizikselMekanService.GetAllExcelAsync(search, filterBag);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Fiziksel Mekanlar");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 26].Value = "Basılı Bilgi";
        worksheet.Cells[1, 27].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 28].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 29].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 29])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < fizikselMekanList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("FM", fizikselMekanList[i].Id);
            worksheet.Cells[i + 2, 2].Value = fizikselMekanList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = fizikselMekanList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = fizikselMekanList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = fizikselMekanList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = fizikselMekanList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = fizikselMekanList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = fizikselMekanList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = fizikselMekanList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = fizikselMekanList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = fizikselMekanList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = fizikselMekanList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                (fizikselMekanList[i].Gizlilik?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].Gizlilik?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].Gizlilik?.StartsWith('c') ?? false) ? "3 puan" :
                (fizikselMekanList[i].Gizlilik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                (fizikselMekanList[i].Butunluk?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].Butunluk?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].Butunluk?.StartsWith('c') ?? false) ? "3 puan" :
                (fizikselMekanList[i].Butunluk?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                (fizikselMekanList[i].Erisilebilirlik?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].Erisilebilirlik?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].Erisilebilirlik?.StartsWith('c') ?? false) ? "3 puan" :
                (fizikselMekanList[i].Erisilebilirlik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                (fizikselMekanList[i].EtkilenenKisiSayisi?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].EtkilenenKisiSayisi?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].EtkilenenKisiSayisi?.StartsWith('c') ?? false) ? "3 puan" :
                (fizikselMekanList[i].EtkilenenKisiSayisi?.StartsWith('d') ?? false) ? "4 puan" :
                (fizikselMekanList[i].EtkilenenKisiSayisi?.StartsWith('e') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                (fizikselMekanList[i].ToplumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].ToplumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].ToplumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                (fizikselMekanList[i].ToplumsalSonuc?.StartsWith('d') ?? false) ? "5 puan" :
                (fizikselMekanList[i].ToplumsalSonuc?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                (fizikselMekanList[i].KurumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].KurumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].KurumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                (fizikselMekanList[i].SektorelEtki?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].SektorelEtki?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].SektorelEtki?.StartsWith('c') ?? false) ? "3 puan" :
                (fizikselMekanList[i].SektorelEtki?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                (fizikselMekanList[i].BagimliVarlik?.StartsWith('a') ?? false) ? "1 puan" :
                (fizikselMekanList[i].BagimliVarlik?.StartsWith('b') ?? false) ? "2 puan" :
                (fizikselMekanList[i].BagimliVarlik?.StartsWith('c') ?? false) ? "3 puan" :
                (fizikselMekanList[i].BagimliVarlik?.StartsWith('d') ?? false) ? "5 puan" :
                (fizikselMekanList[i].BagimliVarlik?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = fizikselMekanList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = fizikselMekanList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = fizikselMekanList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = fizikselMekanList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = fizikselMekanList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 26].Value = fizikselMekanList[i].BasiliBilgi;
            worksheet.Cells[i + 2, 27].Value = fizikselMekanList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 28].Value = fizikselMekanList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 29].Value = fizikselMekanList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelPersonel()
    {
        var personelList = await personelService.GetAllExcelAsync();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Personel");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "MTPD";
        worksheet.Cells[1, 23].Value = "Kurtarma Planları";
        worksheet.Cells[1, 24].Value = "Vekalet Edilme Durumu";
        worksheet.Cells[1, 25].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 26].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 27].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 27])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < personelList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("P", personelList[i].Id);
            worksheet.Cells[i + 2, 2].Value = personelList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = personelList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = personelList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = personelList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = personelList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = personelList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = personelList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = personelList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = personelList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = personelList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = personelList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                (personelList[i].Gizlilik?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].Gizlilik?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].Gizlilik?.StartsWith('c') ?? false) ? "3 puan" :
                (personelList[i].Gizlilik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                (personelList[i].Butunluk?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].Butunluk?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].Butunluk?.StartsWith('c') ?? false) ? "3 puan" :
                (personelList[i].Butunluk?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                (personelList[i].Erisilebilirlik?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].Erisilebilirlik?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].Erisilebilirlik?.StartsWith('c') ?? false) ? "3 puan" :
                (personelList[i].Erisilebilirlik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                (personelList[i].EtkilenenKisiSayisi?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].EtkilenenKisiSayisi?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].EtkilenenKisiSayisi?.StartsWith('c') ?? false) ? "3 puan" :
                (personelList[i].EtkilenenKisiSayisi?.StartsWith('d') ?? false) ? "4 puan" :
                (personelList[i].EtkilenenKisiSayisi?.StartsWith('e') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                (personelList[i].ToplumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].ToplumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].ToplumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                (personelList[i].ToplumsalSonuc?.StartsWith('d') ?? false) ? "5 puan" :
                (personelList[i].ToplumsalSonuc?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                (personelList[i].KurumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].KurumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].KurumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                (personelList[i].SektorelEtki?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].SektorelEtki?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].SektorelEtki?.StartsWith('c') ?? false) ? "3 puan" :
                (personelList[i].SektorelEtki?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                (personelList[i].BagimliVarlik?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].BagimliVarlik?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].BagimliVarlik?.StartsWith('c') ?? false) ? "3 puan" :
                (personelList[i].BagimliVarlik?.StartsWith('d') ?? false) ? "5 puan" :
                (personelList[i].BagimliVarlik?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = personelList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = personelList[i].Mtpd;
            worksheet.Cells[i + 2, 23].Value = personelList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 24].Value = personelList[i].VekaletEdilmeDurumu;
            worksheet.Cells[i + 2, 25].Value = personelList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 26].Value = personelList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 27].Value = personelList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelPersonel(string search, FilterBag filterBag)
    {
        var personelList = await personelService.GetAllExcelAsync(search, filterBag);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Personel");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "MTPD";
        worksheet.Cells[1, 23].Value = "Kurtarma Planları";
        worksheet.Cells[1, 24].Value = "Vekalet Edilme Durumu";
        worksheet.Cells[1, 25].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 26].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 27].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 27])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < personelList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("P", personelList[i].Id);
            worksheet.Cells[i + 2, 2].Value = personelList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = personelList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = personelList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = personelList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = personelList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = personelList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = personelList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = personelList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = personelList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = personelList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = personelList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                (personelList[i].Gizlilik?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].Gizlilik?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].Gizlilik?.StartsWith('c') ?? false) ? "3 puan" :
                (personelList[i].Gizlilik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                (personelList[i].Butunluk?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].Butunluk?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].Butunluk?.StartsWith('c') ?? false) ? "3 puan" :
                (personelList[i].Butunluk?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                (personelList[i].Erisilebilirlik?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].Erisilebilirlik?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].Erisilebilirlik?.StartsWith('c') ?? false) ? "3 puan" :
                (personelList[i].Erisilebilirlik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                (personelList[i].EtkilenenKisiSayisi?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].EtkilenenKisiSayisi?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].EtkilenenKisiSayisi?.StartsWith('c') ?? false) ? "3 puan" :
                (personelList[i].EtkilenenKisiSayisi?.StartsWith('d') ?? false) ? "4 puan" :
                (personelList[i].EtkilenenKisiSayisi?.StartsWith('e') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                (personelList[i].ToplumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].ToplumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].ToplumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                (personelList[i].ToplumsalSonuc?.StartsWith('d') ?? false) ? "5 puan" :
                (personelList[i].ToplumsalSonuc?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                (personelList[i].KurumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].KurumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].KurumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                (personelList[i].SektorelEtki?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].SektorelEtki?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].SektorelEtki?.StartsWith('c') ?? false) ? "3 puan" :
                (personelList[i].SektorelEtki?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                (personelList[i].BagimliVarlik?.StartsWith('a') ?? false) ? "1 puan" :
                (personelList[i].BagimliVarlik?.StartsWith('b') ?? false) ? "2 puan" :
                (personelList[i].BagimliVarlik?.StartsWith('c') ?? false) ? "3 puan" :
                (personelList[i].BagimliVarlik?.StartsWith('d') ?? false) ? "5 puan" :
                (personelList[i].BagimliVarlik?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = personelList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = personelList[i].Mtpd;
            worksheet.Cells[i + 2, 23].Value = personelList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 24].Value = personelList[i].VekaletEdilmeDurumu;
            worksheet.Cells[i + 2, 25].Value = personelList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 26].Value = personelList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 27].Value = personelList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelKriptografiEnvanteri()
    {
        var kriptografiEnvanteriList = await kriptografiEnvanteriService.GetAllExcelAsync();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Kriptografi Envanteri");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Adı";
        worksheet.Cells[1, 3].Value = "Üretim Yeri";
        worksheet.Cells[1, 4].Value = "Kullanım Amacı";
        worksheet.Cells[1, 5].Value = "Oluşturma Tarihi";
        worksheet.Cells[1, 6].Value = "Kullanım Süresi";
        worksheet.Cells[1, 7].Value = "Anahtar Sorumlusu";
        worksheet.Cells[1, 8].Value = "Anahtar Saklama Alanı";
        worksheet.Cells[1, 9].Value = "Destek Alınan Tedarikçi";
        worksheet.Cells[1, 10].Value = "Donanım / Yazılım";
        worksheet.Cells[1, 11].Value = "Algoritma";
        worksheet.Cells[1, 12].Value = "Ortak Kriterler";
        worksheet.Cells[1, 13].Value = "Kullanım Seviyesi";
        worksheet.Cells[1, 14].Value = "Kullanım Kabiliyetleri";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 14])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < kriptografiEnvanteriList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("KE", kriptografiEnvanteriList[i].Id);
            worksheet.Cells[i + 2, 2].Value = kriptografiEnvanteriList[i].VarlikAdi;
            worksheet.Cells[i + 2, 3].Value = kriptografiEnvanteriList[i].UretimYeri;
            worksheet.Cells[i + 2, 4].Value = kriptografiEnvanteriList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 5].Value = kriptografiEnvanteriList[i].OlusturmaTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 6].Value = kriptografiEnvanteriList[i].KullanimSuresi;
            worksheet.Cells[i + 2, 7].Value = kriptografiEnvanteriList[i].AnahtarSorumlusu;
            worksheet.Cells[i + 2, 8].Value = kriptografiEnvanteriList[i].AnahtarSaklamaAlani;
            worksheet.Cells[i + 2, 9].Value = kriptografiEnvanteriList[i].DestekAlinanTedarikci;
            worksheet.Cells[i + 2, 10].Value = kriptografiEnvanteriList[i].DonanimYazilim;
            worksheet.Cells[i + 2, 11].Value = kriptografiEnvanteriList[i].Algoritma;
            worksheet.Cells[i + 2, 12].Value = kriptografiEnvanteriList[i].OrtakKriterler;
            worksheet.Cells[i + 2, 13].Value = kriptografiEnvanteriList[i].KullanimSeviyesi;
            worksheet.Cells[i + 2, 14].Value = kriptografiEnvanteriList[i].KullanimKabiliyetleri;
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelKriptografiEnvanteri(string search)
    {
        var kriptografiEnvanteriList =
            await kriptografiEnvanteriService.GetAllExcelAsync(search);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Kriptografi Envanteri");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Adı";
        worksheet.Cells[1, 3].Value = "Varlık Sahibi";
        worksheet.Cells[1, 4].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 5].Value = "Üretim Yeri";
        worksheet.Cells[1, 6].Value = "Kullanım Amacı";
        worksheet.Cells[1, 7].Value = "Oluşturma Tarihi";
        worksheet.Cells[1, 8].Value = "Kullanım Süresi";
        worksheet.Cells[1, 9].Value = "Anahtar Sorumlusu";
        worksheet.Cells[1, 10].Value = "Anahtar Saklama Alanı";
        worksheet.Cells[1, 11].Value = "Destek Alınan Tedarikçi";
        worksheet.Cells[1, 12].Value = "Donanım / Yazılım";
        worksheet.Cells[1, 13].Value = "Algoritma";
        worksheet.Cells[1, 14].Value = "Ortak Kriterler";
        worksheet.Cells[1, 15].Value = "Kullanım Seviyesi";
        worksheet.Cells[1, 16].Value = "Kullanım Kabiliyetleri";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 16])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < kriptografiEnvanteriList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("KE", kriptografiEnvanteriList[i].Id);
            worksheet.Cells[i + 2, 2].Value = kriptografiEnvanteriList[i].VarlikAdi;
            worksheet.Cells[i + 2, 3].Value = kriptografiEnvanteriList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 4].Value = kriptografiEnvanteriList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 5].Value = kriptografiEnvanteriList[i].UretimYeri;
            worksheet.Cells[i + 2, 6].Value = kriptografiEnvanteriList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 7].Value = kriptografiEnvanteriList[i].OlusturmaTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 8].Value = kriptografiEnvanteriList[i].KullanimSuresi;
            worksheet.Cells[i + 2, 9].Value = kriptografiEnvanteriList[i].AnahtarSorumlusu;
            worksheet.Cells[i + 2, 10].Value = kriptografiEnvanteriList[i].AnahtarSaklamaAlani;
            worksheet.Cells[i + 2, 11].Value = kriptografiEnvanteriList[i].DestekAlinanTedarikci;
            worksheet.Cells[i + 2, 12].Value = kriptografiEnvanteriList[i].DonanimYazilim;
            worksheet.Cells[i + 2, 13].Value = kriptografiEnvanteriList[i].Algoritma;
            worksheet.Cells[i + 2, 14].Value = kriptografiEnvanteriList[i].OrtakKriterler;
            worksheet.Cells[i + 2, 15].Value = kriptografiEnvanteriList[i].KullanimSeviyesi;
            worksheet.Cells[i + 2, 16].Value = kriptografiEnvanteriList[i].KullanimKabiliyetleri;
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelBasiliBilgi()
    {
        var basiliBilgiList = await basiliBilgiService.GetAllExcelAsync();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Basılı Bilgi");

        // Add headers
        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 26].Value = "Saklama Süresi";
        worksheet.Cells[1, 27].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 28].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 29].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 29])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < basiliBilgiList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("BB", basiliBilgiList[i].Id);
            worksheet.Cells[i + 2, 2].Value = basiliBilgiList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = basiliBilgiList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = basiliBilgiList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = basiliBilgiList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = basiliBilgiList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = basiliBilgiList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = basiliBilgiList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = basiliBilgiList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = basiliBilgiList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = basiliBilgiList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = basiliBilgiList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                (basiliBilgiList[i].Gizlilik?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].Gizlilik?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].Gizlilik?.StartsWith('c') ?? false) ? "3 puan" :
                (basiliBilgiList[i].Gizlilik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                (basiliBilgiList[i].Butunluk?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].Butunluk?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].Butunluk?.StartsWith('c') ?? false) ? "3 puan" :
                (basiliBilgiList[i].Butunluk?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                (basiliBilgiList[i].Erisilebilirlik?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].Erisilebilirlik?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].Erisilebilirlik?.StartsWith('c') ?? false) ? "3 puan" :
                (basiliBilgiList[i].Erisilebilirlik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                (basiliBilgiList[i].EtkilenenKisiSayisi?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].EtkilenenKisiSayisi?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].EtkilenenKisiSayisi?.StartsWith('c') ?? false) ? "3 puan" :
                (basiliBilgiList[i].EtkilenenKisiSayisi?.StartsWith('d') ?? false) ? "4 puan" :
                (basiliBilgiList[i].EtkilenenKisiSayisi?.StartsWith('e') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                (basiliBilgiList[i].ToplumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].ToplumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].ToplumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                (basiliBilgiList[i].ToplumsalSonuc?.StartsWith('d') ?? false) ? "5 puan" :
                (basiliBilgiList[i].ToplumsalSonuc?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                (basiliBilgiList[i].KurumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].KurumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].KurumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                (basiliBilgiList[i].SektorelEtki?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].SektorelEtki?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].SektorelEtki?.StartsWith('c') ?? false) ? "3 puan" :
                (basiliBilgiList[i].SektorelEtki?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                (basiliBilgiList[i].BagimliVarlik?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].BagimliVarlik?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].BagimliVarlik?.StartsWith('c') ?? false) ? "3 puan" :
                (basiliBilgiList[i].BagimliVarlik?.StartsWith('d') ?? false) ? "5 puan" :
                (basiliBilgiList[i].BagimliVarlik?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = basiliBilgiList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = basiliBilgiList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = basiliBilgiList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = basiliBilgiList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = basiliBilgiList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 26].Value = basiliBilgiList[i].SaklamaSuresi;
            worksheet.Cells[i + 2, 27].Value = basiliBilgiList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 28].Value = basiliBilgiList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 29].Value = basiliBilgiList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelBasiliBilgi(string search, FilterBag filterBag)
    {
        var basiliBilgiList = await basiliBilgiService.GetAllExcelAsync(search, filterBag);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Basılı Bilgi");

        // Add headers
        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 26].Value = "Saklama Süresi";
        worksheet.Cells[1, 27].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 28].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 29].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 29])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < basiliBilgiList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("BB", basiliBilgiList[i].Id);
            worksheet.Cells[i + 2, 2].Value = basiliBilgiList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = basiliBilgiList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = basiliBilgiList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = basiliBilgiList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = basiliBilgiList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = basiliBilgiList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = basiliBilgiList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = basiliBilgiList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = basiliBilgiList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = basiliBilgiList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = basiliBilgiList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                (basiliBilgiList[i].Gizlilik?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].Gizlilik?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].Gizlilik?.StartsWith('c') ?? false) ? "3 puan" :
                (basiliBilgiList[i].Gizlilik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                (basiliBilgiList[i].Butunluk?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].Butunluk?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].Butunluk?.StartsWith('c') ?? false) ? "3 puan" :
                (basiliBilgiList[i].Butunluk?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                (basiliBilgiList[i].Erisilebilirlik?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].Erisilebilirlik?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].Erisilebilirlik?.StartsWith('c') ?? false) ? "3 puan" :
                (basiliBilgiList[i].Erisilebilirlik?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                (basiliBilgiList[i].EtkilenenKisiSayisi?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].EtkilenenKisiSayisi?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].EtkilenenKisiSayisi?.StartsWith('c') ?? false) ? "3 puan" :
                (basiliBilgiList[i].EtkilenenKisiSayisi?.StartsWith('d') ?? false) ? "4 puan" :
                (basiliBilgiList[i].EtkilenenKisiSayisi?.StartsWith('e') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                (basiliBilgiList[i].ToplumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].ToplumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].ToplumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                (basiliBilgiList[i].ToplumsalSonuc?.StartsWith('d') ?? false) ? "5 puan" :
                (basiliBilgiList[i].ToplumsalSonuc?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                (basiliBilgiList[i].KurumsalSonuc?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].KurumsalSonuc?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].KurumsalSonuc?.StartsWith('c') ?? false) ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                (basiliBilgiList[i].SektorelEtki?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].SektorelEtki?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].SektorelEtki?.StartsWith('c') ?? false) ? "3 puan" :
                (basiliBilgiList[i].SektorelEtki?.StartsWith('d') ?? false) ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                (basiliBilgiList[i].BagimliVarlik?.StartsWith('a') ?? false) ? "1 puan" :
                (basiliBilgiList[i].BagimliVarlik?.StartsWith('b') ?? false) ? "2 puan" :
                (basiliBilgiList[i].BagimliVarlik?.StartsWith('c') ?? false) ? "3 puan" :
                (basiliBilgiList[i].BagimliVarlik?.StartsWith('d') ?? false) ? "5 puan" :
                (basiliBilgiList[i].BagimliVarlik?.StartsWith('e') ?? false) ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = basiliBilgiList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = basiliBilgiList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = basiliBilgiList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = basiliBilgiList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = basiliBilgiList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 26].Value = basiliBilgiList[i].SaklamaSuresi;
            worksheet.Cells[i + 2, 27].Value = basiliBilgiList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 28].Value = basiliBilgiList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 29].Value = basiliBilgiList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelElektronikBilgi()
    {
        var elektronikBilgiList = await elektronikBilgiService.GetAllExcelAsync();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Elektronik Bilgi");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Yedekleme Tipi";
        worksheet.Cells[1, 26].Value = "Yedekleme Türü";
        worksheet.Cells[1, 27].Value = "Yedekleme Sıklığı";
        worksheet.Cells[1, 28].Value = "Yedeklerin Saklama Süresi";
        worksheet.Cells[1, 29].Value = "Yedekleme Alanı";
        worksheet.Cells[1, 30].Value = "Yedekten Dönüş Planı";
        worksheet.Cells[1, 31].Value = "Yedekleme Sorumlusu";
        worksheet.Cells[1, 32].Value = "Kriptoloji";
        worksheet.Cells[1, 33].Value = "Kriptoloji Türü";
        worksheet.Cells[1, 34].Value = "Kullanılan Kriptoloji";
        worksheet.Cells[1, 35].Value = "Anahtar Sorumlusu";
        worksheet.Cells[1, 36].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 37].Value = "Saklama Süresi";
        worksheet.Cells[1, 38].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 39].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 40].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 40])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < elektronikBilgiList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("EB", elektronikBilgiList[i].Id);
            worksheet.Cells[i + 2, 2].Value = elektronikBilgiList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = elektronikBilgiList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = elektronikBilgiList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = elektronikBilgiList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = elektronikBilgiList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = elektronikBilgiList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = elektronikBilgiList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = elektronikBilgiList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = elektronikBilgiList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = elektronikBilgiList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = elektronikBilgiList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                elektronikBilgiList[i].Gizlilik?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].Gizlilik?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].Gizlilik?.StartsWith('c') == true ? "3 puan" :
                elektronikBilgiList[i].Gizlilik?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                elektronikBilgiList[i].Butunluk?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].Butunluk?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].Butunluk?.StartsWith('c') == true ? "3 puan" :
                elektronikBilgiList[i].Butunluk?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                elektronikBilgiList[i].Erisilebilirlik?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].Erisilebilirlik?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].Erisilebilirlik?.StartsWith('c') == true ? "3 puan" :
                elektronikBilgiList[i].Erisilebilirlik?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                elektronikBilgiList[i].EtkilenenKisiSayisi?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].EtkilenenKisiSayisi?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].EtkilenenKisiSayisi?.StartsWith('c') == true ? "3 puan" :
                elektronikBilgiList[i].EtkilenenKisiSayisi?.StartsWith('d') == true ? "4 puan" :
                elektronikBilgiList[i].EtkilenenKisiSayisi?.StartsWith('e') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                elektronikBilgiList[i].ToplumsalSonuc?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].ToplumsalSonuc?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].ToplumsalSonuc?.StartsWith('c') == true ? "3 puan" :
                elektronikBilgiList[i].ToplumsalSonuc?.StartsWith('d') == true ? "5 puan" :
                elektronikBilgiList[i].ToplumsalSonuc?.StartsWith('e') == true ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                elektronikBilgiList[i].KurumsalSonuc?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].KurumsalSonuc?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].KurumsalSonuc?.StartsWith('c') == true ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                elektronikBilgiList[i].SektorelEtki?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].SektorelEtki?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].SektorelEtki?.StartsWith('c') == true ? "3 puan" :
                elektronikBilgiList[i].SektorelEtki?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                elektronikBilgiList[i].BagimliVarlik?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].BagimliVarlik?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].BagimliVarlik?.StartsWith('c') == true ? "3 puan" :
                elektronikBilgiList[i].BagimliVarlik?.StartsWith('d') == true ? "5 puan" :
                elektronikBilgiList[i].BagimliVarlik?.StartsWith('e') == true ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = elektronikBilgiList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = elektronikBilgiList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = elektronikBilgiList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = elektronikBilgiList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = elektronikBilgiList[i].YedeklemeTipi;
            worksheet.Cells[i + 2, 26].Value = elektronikBilgiList[i].YedeklemeTuru;
            worksheet.Cells[i + 2, 27].Value = elektronikBilgiList[i].YedeklemeSikligi;
            worksheet.Cells[i + 2, 28].Value = elektronikBilgiList[i].YedeklerinSaklamaSuresi;
            worksheet.Cells[i + 2, 29].Value = elektronikBilgiList[i].YedeklemeAlani;
            worksheet.Cells[i + 2, 30].Value = elektronikBilgiList[i].YedektenDonusPlani;
            worksheet.Cells[i + 2, 31].Value = elektronikBilgiList[i].YedeklemeSorumlusu;
            worksheet.Cells[i + 2, 32].Value = elektronikBilgiList[i].Kriptoloji;
            worksheet.Cells[i + 2, 33].Value = elektronikBilgiList[i].KriptolojiTuru;
            worksheet.Cells[i + 2, 34].Value = elektronikBilgiList[i].KullanilanKriptoloji;
            worksheet.Cells[i + 2, 35].Value = elektronikBilgiList[i].AnahtarSorumlusu;
            worksheet.Cells[i + 2, 36].Value = elektronikBilgiList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 37].Value = elektronikBilgiList[i].SaklamaSuresi;
            worksheet.Cells[i + 2, 38].Value = elektronikBilgiList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 39].Value = elektronikBilgiList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 40].Value = elektronikBilgiList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelElektronikBilgi(string search, FilterBag filterBag)
    {
        var elektronikBilgiList =
            await elektronikBilgiService.GetAllExcelAsync(search, filterBag);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Elektronik Bilgi");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Yedekleme Tipi";
        worksheet.Cells[1, 26].Value = "Yedekleme Türü";
        worksheet.Cells[1, 27].Value = "Yedekleme Sıklığı";
        worksheet.Cells[1, 28].Value = "Yedeklerin Saklama Süresi";
        worksheet.Cells[1, 29].Value = "Yedekleme Alanı";
        worksheet.Cells[1, 30].Value = "Yedekten Dönüş Planı";
        worksheet.Cells[1, 31].Value = "Yedekleme Sorumlusu";
        worksheet.Cells[1, 32].Value = "Kriptoloji";
        worksheet.Cells[1, 33].Value = "Kriptoloji Türü";
        worksheet.Cells[1, 34].Value = "Kullanılan Kriptoloji";
        worksheet.Cells[1, 35].Value = "Anahtar Sorumlusu";
        worksheet.Cells[1, 36].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 37].Value = "Saklama Süresi";
        worksheet.Cells[1, 38].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 39].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 40].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 40])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < elektronikBilgiList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("EB", elektronikBilgiList[i].Id);
            worksheet.Cells[i + 2, 2].Value = elektronikBilgiList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = elektronikBilgiList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = elektronikBilgiList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = elektronikBilgiList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = elektronikBilgiList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = elektronikBilgiList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = elektronikBilgiList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = elektronikBilgiList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = elektronikBilgiList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = elektronikBilgiList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = elektronikBilgiList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                elektronikBilgiList[i].Gizlilik?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].Gizlilik?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].Gizlilik?.StartsWith('c') == true ? "3 puan" :
                elektronikBilgiList[i].Gizlilik?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                elektronikBilgiList[i].Butunluk?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].Butunluk?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].Butunluk?.StartsWith('c') == true ? "3 puan" :
                elektronikBilgiList[i].Butunluk?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                elektronikBilgiList[i].Erisilebilirlik?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].Erisilebilirlik?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].Erisilebilirlik?.StartsWith('c') == true ? "3 puan" :
                elektronikBilgiList[i].Erisilebilirlik?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                elektronikBilgiList[i].EtkilenenKisiSayisi?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].EtkilenenKisiSayisi?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].EtkilenenKisiSayisi?.StartsWith('c') == true ? "3 puan" :
                elektronikBilgiList[i].EtkilenenKisiSayisi?.StartsWith('d') == true ? "4 puan" :
                elektronikBilgiList[i].EtkilenenKisiSayisi?.StartsWith('e') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                elektronikBilgiList[i].ToplumsalSonuc?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].ToplumsalSonuc?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].ToplumsalSonuc?.StartsWith('c') == true ? "3 puan" :
                elektronikBilgiList[i].ToplumsalSonuc?.StartsWith('d') == true ? "5 puan" :
                elektronikBilgiList[i].ToplumsalSonuc?.StartsWith('e') == true ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                elektronikBilgiList[i].KurumsalSonuc?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].KurumsalSonuc?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].KurumsalSonuc?.StartsWith('c') == true ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                elektronikBilgiList[i].SektorelEtki?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].SektorelEtki?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].SektorelEtki?.StartsWith('c') == true ? "3 puan" :
                elektronikBilgiList[i].SektorelEtki?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                elektronikBilgiList[i].BagimliVarlik?.StartsWith('a') == true ? "1 puan" :
                elektronikBilgiList[i].BagimliVarlik?.StartsWith('b') == true ? "2 puan" :
                elektronikBilgiList[i].BagimliVarlik?.StartsWith('c') == true ? "3 puan" :
                elektronikBilgiList[i].BagimliVarlik?.StartsWith('d') == true ? "5 puan" :
                elektronikBilgiList[i].BagimliVarlik?.StartsWith('e') == true ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = elektronikBilgiList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = elektronikBilgiList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = elektronikBilgiList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = elektronikBilgiList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = elektronikBilgiList[i].YedeklemeTipi;
            worksheet.Cells[i + 2, 26].Value = elektronikBilgiList[i].YedeklemeTuru;
            worksheet.Cells[i + 2, 27].Value = elektronikBilgiList[i].YedeklemeSikligi;
            worksheet.Cells[i + 2, 28].Value = elektronikBilgiList[i].YedeklerinSaklamaSuresi;
            worksheet.Cells[i + 2, 29].Value = elektronikBilgiList[i].YedeklemeAlani;
            worksheet.Cells[i + 2, 30].Value = elektronikBilgiList[i].YedektenDonusPlani;
            worksheet.Cells[i + 2, 31].Value = elektronikBilgiList[i].YedeklemeSorumlusu;
            worksheet.Cells[i + 2, 32].Value = elektronikBilgiList[i].Kriptoloji;
            worksheet.Cells[i + 2, 33].Value = elektronikBilgiList[i].KriptolojiTuru;
            worksheet.Cells[i + 2, 34].Value = elektronikBilgiList[i].KullanilanKriptoloji;
            worksheet.Cells[i + 2, 35].Value = elektronikBilgiList[i].AnahtarSorumlusu;
            worksheet.Cells[i + 2, 36].Value = elektronikBilgiList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 37].Value = elektronikBilgiList[i].SaklamaSuresi;
            worksheet.Cells[i + 2, 38].Value = elektronikBilgiList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 39].Value = elektronikBilgiList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 40].Value = elektronikBilgiList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelVeritabani()
    {
        var veritabaniList = await veritabaniService.GetAllExcelAsync();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Veritabanı");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Yedekleme Tipi";
        worksheet.Cells[1, 26].Value = "Yedekleme Türü";
        worksheet.Cells[1, 27].Value = "Yedekleme Sıklığı";
        worksheet.Cells[1, 28].Value = "Yedeklerin Saklama Süresi";
        worksheet.Cells[1, 29].Value = "Yedekleme Alanı";
        worksheet.Cells[1, 30].Value = "Yedekten Dönüş Planı";
        worksheet.Cells[1, 31].Value = "Yedekleme Sorumlusu";
        worksheet.Cells[1, 32].Value = "Kriptoloji";
        worksheet.Cells[1, 33].Value = "Kriptoloji Türü";
        worksheet.Cells[1, 34].Value = "Kullanılan Kriptoloji";
        worksheet.Cells[1, 35].Value = "Anahtar Sorumlusu";
        worksheet.Cells[1, 36].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 37].Value = "Bulut Bilişim";
        worksheet.Cells[1, 38].Value = "Yeni Gelişmeler ve Tedarik";
        worksheet.Cells[1, 39].Value = "Kritik Altyapı Sistemi";
        worksheet.Cells[1, 40].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 41].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 42].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 42])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < veritabaniList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("V", veritabaniList[i].Id);
            worksheet.Cells[i + 2, 2].Value = veritabaniList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = veritabaniList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = veritabaniList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = veritabaniList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = veritabaniList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = veritabaniList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = veritabaniList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = veritabaniList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = veritabaniList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = veritabaniList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = veritabaniList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                veritabaniList[i].Gizlilik?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].Gizlilik?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].Gizlilik?.StartsWith('c') == true ? "3 puan" :
                veritabaniList[i].Gizlilik?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                veritabaniList[i].Butunluk?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].Butunluk?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].Butunluk?.StartsWith('c') == true ? "3 puan" :
                veritabaniList[i].Butunluk?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                veritabaniList[i].Erisilebilirlik?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].Erisilebilirlik?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].Erisilebilirlik?.StartsWith('c') == true ? "3 puan" :
                veritabaniList[i].Erisilebilirlik?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                veritabaniList[i].EtkilenenKisiSayisi?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].EtkilenenKisiSayisi?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].EtkilenenKisiSayisi?.StartsWith('c') == true ? "3 puan" :
                veritabaniList[i].EtkilenenKisiSayisi?.StartsWith('d') == true ? "4 puan" :
                veritabaniList[i].EtkilenenKisiSayisi?.StartsWith('e') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                veritabaniList[i].ToplumsalSonuc?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].ToplumsalSonuc?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].ToplumsalSonuc?.StartsWith('c') == true ? "3 puan" :
                veritabaniList[i].ToplumsalSonuc?.StartsWith('d') == true ? "5 puan" :
                veritabaniList[i].ToplumsalSonuc?.StartsWith('e') == true ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                veritabaniList[i].KurumsalSonuc?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].KurumsalSonuc?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].KurumsalSonuc?.StartsWith('c') == true ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                veritabaniList[i].SektorelEtki?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].SektorelEtki?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].SektorelEtki?.StartsWith('c') == true ? "3 puan" :
                veritabaniList[i].SektorelEtki?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                veritabaniList[i].BagimliVarlik?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].BagimliVarlik?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].BagimliVarlik?.StartsWith('c') == true ? "3 puan" :
                veritabaniList[i].BagimliVarlik?.StartsWith('d') == true ? "5 puan" :
                veritabaniList[i].BagimliVarlik?.StartsWith('e') == true ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = veritabaniList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = veritabaniList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = veritabaniList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = veritabaniList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = veritabaniList[i].YedeklemeTipi;
            worksheet.Cells[i + 2, 26].Value = veritabaniList[i].YedeklemeTuru;
            worksheet.Cells[i + 2, 27].Value = veritabaniList[i].YedeklemeSikligi;
            worksheet.Cells[i + 2, 28].Value = veritabaniList[i].YedeklerinSaklamaSuresi;
            worksheet.Cells[i + 2, 29].Value = veritabaniList[i].YedeklemeAlani;
            worksheet.Cells[i + 2, 30].Value = veritabaniList[i].YedektenDonusPlani;
            worksheet.Cells[i + 2, 31].Value = veritabaniList[i].YedeklemeSorumlusu;
            worksheet.Cells[i + 2, 32].Value = veritabaniList[i].Kriptoloji;
            worksheet.Cells[i + 2, 33].Value = veritabaniList[i].KriptolojiTuru;
            worksheet.Cells[i + 2, 34].Value = veritabaniList[i].KullanilanKriptoloji;
            worksheet.Cells[i + 2, 35].Value = veritabaniList[i].AnahtarSorumlusu;
            worksheet.Cells[i + 2, 36].Value = veritabaniList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 37].Value = veritabaniList[i].BulutBilisim;
            worksheet.Cells[i + 2, 38].Value = veritabaniList[i].YeniGelismelerveTedarik;
            worksheet.Cells[i + 2, 39].Value = veritabaniList[i].KritikAltyapiSistemi;
            worksheet.Cells[i + 2, 40].Value = veritabaniList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 41].Value = veritabaniList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 42].Value = veritabaniList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelVeritabani(string search, FilterBag filterBag)
    {
        var veritabaniList = await veritabaniService.GetAllExcelAsync(search, filterBag);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Veritabanı");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Yedekleme Tipi";
        worksheet.Cells[1, 26].Value = "Yedekleme Türü";
        worksheet.Cells[1, 27].Value = "Yedekleme Sıklığı";
        worksheet.Cells[1, 28].Value = "Yedeklerin Saklama Süresi";
        worksheet.Cells[1, 29].Value = "Yedekleme Alanı";
        worksheet.Cells[1, 30].Value = "Yedekten Dönüş Planı";
        worksheet.Cells[1, 31].Value = "Yedekleme Sorumlusu";
        worksheet.Cells[1, 32].Value = "Kriptoloji";
        worksheet.Cells[1, 33].Value = "Kriptoloji Türü";
        worksheet.Cells[1, 34].Value = "Kullanılan Kriptoloji";
        worksheet.Cells[1, 35].Value = "Anahtar Sorumlusu";
        worksheet.Cells[1, 36].Value = "Kişisel Veri Barındırma";
        worksheet.Cells[1, 37].Value = "Bulut Bilişim";
        worksheet.Cells[1, 38].Value = "Yeni Gelişmeler ve Tedarik";
        worksheet.Cells[1, 39].Value = "Kritik Altyapı Sistemi";
        worksheet.Cells[1, 40].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 41].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 42].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 42])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < veritabaniList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("V", veritabaniList[i].Id);
            worksheet.Cells[i + 2, 2].Value = veritabaniList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = veritabaniList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = veritabaniList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = veritabaniList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = veritabaniList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = veritabaniList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = veritabaniList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = veritabaniList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = veritabaniList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = veritabaniList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = veritabaniList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                veritabaniList[i].Gizlilik?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].Gizlilik?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].Gizlilik?.StartsWith('c') == true ? "3 puan" :
                veritabaniList[i].Gizlilik?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                veritabaniList[i].Butunluk?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].Butunluk?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].Butunluk?.StartsWith('c') == true ? "3 puan" :
                veritabaniList[i].Butunluk?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                veritabaniList[i].Erisilebilirlik?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].Erisilebilirlik?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].Erisilebilirlik?.StartsWith('c') == true ? "3 puan" :
                veritabaniList[i].Erisilebilirlik?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                veritabaniList[i].EtkilenenKisiSayisi?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].EtkilenenKisiSayisi?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].EtkilenenKisiSayisi?.StartsWith('c') == true ? "3 puan" :
                veritabaniList[i].EtkilenenKisiSayisi?.StartsWith('d') == true ? "4 puan" :
                veritabaniList[i].EtkilenenKisiSayisi?.StartsWith('e') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                veritabaniList[i].ToplumsalSonuc?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].ToplumsalSonuc?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].ToplumsalSonuc?.StartsWith('c') == true ? "3 puan" :
                veritabaniList[i].ToplumsalSonuc?.StartsWith('d') == true ? "5 puan" :
                veritabaniList[i].ToplumsalSonuc?.StartsWith('e') == true ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                veritabaniList[i].KurumsalSonuc?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].KurumsalSonuc?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].KurumsalSonuc?.StartsWith('c') == true ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                veritabaniList[i].SektorelEtki?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].SektorelEtki?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].SektorelEtki?.StartsWith('c') == true ? "3 puan" :
                veritabaniList[i].SektorelEtki?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                veritabaniList[i].BagimliVarlik?.StartsWith('a') == true ? "1 puan" :
                veritabaniList[i].BagimliVarlik?.StartsWith('b') == true ? "2 puan" :
                veritabaniList[i].BagimliVarlik?.StartsWith('c') == true ? "3 puan" :
                veritabaniList[i].BagimliVarlik?.StartsWith('d') == true ? "5 puan" :
                veritabaniList[i].BagimliVarlik?.StartsWith('e') == true ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = veritabaniList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = veritabaniList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = veritabaniList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = veritabaniList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = veritabaniList[i].YedeklemeTipi;
            worksheet.Cells[i + 2, 26].Value = veritabaniList[i].YedeklemeTuru;
            worksheet.Cells[i + 2, 27].Value = veritabaniList[i].YedeklemeSikligi;
            worksheet.Cells[i + 2, 28].Value = veritabaniList[i].YedeklerinSaklamaSuresi;
            worksheet.Cells[i + 2, 29].Value = veritabaniList[i].YedeklemeAlani;
            worksheet.Cells[i + 2, 30].Value = veritabaniList[i].YedektenDonusPlani;
            worksheet.Cells[i + 2, 31].Value = veritabaniList[i].YedeklemeSorumlusu;
            worksheet.Cells[i + 2, 32].Value = veritabaniList[i].Kriptoloji;
            worksheet.Cells[i + 2, 33].Value = veritabaniList[i].KriptolojiTuru;
            worksheet.Cells[i + 2, 34].Value = veritabaniList[i].KullanilanKriptoloji;
            worksheet.Cells[i + 2, 35].Value = veritabaniList[i].AnahtarSorumlusu;
            worksheet.Cells[i + 2, 36].Value = veritabaniList[i].KisiselVeriBarindirma;
            worksheet.Cells[i + 2, 37].Value = veritabaniList[i].BulutBilisim;
            worksheet.Cells[i + 2, 38].Value = veritabaniList[i].YeniGelismelerveTedarik;
            worksheet.Cells[i + 2, 39].Value = veritabaniList[i].KritikAltyapiSistemi;
            worksheet.Cells[i + 2, 40].Value = veritabaniList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 41].Value = veritabaniList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 42].Value = veritabaniList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelSurec()
    {
        var surecList = await surecService.GetAllExcelAsync();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Süreç");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 26].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 27].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 27])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < surecList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("S", surecList[i].Id);
            worksheet.Cells[i + 2, 2].Value = surecList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = surecList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = surecList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = surecList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = surecList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = surecList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = surecList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = surecList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = surecList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = surecList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = surecList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                surecList[i].Gizlilik?.StartsWith('a') == true ? "1 puan" :
                surecList[i].Gizlilik?.StartsWith('b') == true ? "2 puan" :
                surecList[i].Gizlilik?.StartsWith('c') == true ? "3 puan" :
                surecList[i].Gizlilik?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                surecList[i].Butunluk?.StartsWith('a') == true ? "1 puan" :
                surecList[i].Butunluk?.StartsWith('b') == true ? "2 puan" :
                surecList[i].Butunluk?.StartsWith('c') == true ? "3 puan" :
                surecList[i].Butunluk?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                surecList[i].Erisilebilirlik?.StartsWith('a') == true ? "1 puan" :
                surecList[i].Erisilebilirlik?.StartsWith('b') == true ? "2 puan" :
                surecList[i].Erisilebilirlik?.StartsWith('c') == true ? "3 puan" :
                surecList[i].Erisilebilirlik?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                surecList[i].EtkilenenKisiSayisi?.StartsWith('a') == true ? "1 puan" :
                surecList[i].EtkilenenKisiSayisi?.StartsWith('b') == true ? "2 puan" :
                surecList[i].EtkilenenKisiSayisi?.StartsWith('c') == true ? "3 puan" :
                surecList[i].EtkilenenKisiSayisi?.StartsWith('d') == true ? "4 puan" :
                surecList[i].EtkilenenKisiSayisi?.StartsWith('e') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                surecList[i].ToplumsalSonuc?.StartsWith('a') == true ? "1 puan" :
                surecList[i].ToplumsalSonuc?.StartsWith('b') == true ? "2 puan" :
                surecList[i].ToplumsalSonuc?.StartsWith('c') == true ? "3 puan" :
                surecList[i].ToplumsalSonuc?.StartsWith('d') == true ? "5 puan" :
                surecList[i].ToplumsalSonuc?.StartsWith('e') == true ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                surecList[i].KurumsalSonuc?.StartsWith('a') == true ? "1 puan" :
                surecList[i].KurumsalSonuc?.StartsWith('b') == true ? "2 puan" :
                surecList[i].KurumsalSonuc?.StartsWith('c') == true ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                surecList[i].SektorelEtki?.StartsWith('a') == true ? "1 puan" :
                surecList[i].SektorelEtki?.StartsWith('b') == true ? "2 puan" :
                surecList[i].SektorelEtki?.StartsWith('c') == true ? "3 puan" :
                surecList[i].SektorelEtki?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                surecList[i].BagimliVarlik?.StartsWith('a') == true ? "1 puan" :
                surecList[i].BagimliVarlik?.StartsWith('b') == true ? "2 puan" :
                surecList[i].BagimliVarlik?.StartsWith('c') == true ? "3 puan" :
                surecList[i].BagimliVarlik?.StartsWith('d') == true ? "5 puan" :
                surecList[i].BagimliVarlik?.StartsWith('e') == true ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = surecList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = surecList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = surecList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = surecList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = surecList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 26].Value = surecList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 27].Value = surecList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelSurec(string search, FilterBag filterBag)
    {
        var surecList = await surecService.GetAllExcelAsync(search, filterBag);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Süreç");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Bilgi Sınıfı";
        worksheet.Cells[1, 13].Value = "Gizlilik";
        worksheet.Cells[1, 14].Value = "Bütünlük";
        worksheet.Cells[1, 15].Value = "Erişebilirlik";
        worksheet.Cells[1, 16].Value = "Etkilenen Kişi Sayısı";
        worksheet.Cells[1, 17].Value = "Toplumsal Sonuçlar";
        worksheet.Cells[1, 18].Value = "Kurumsal Sonuçlar";
        worksheet.Cells[1, 19].Value = "Sektörel Etki";
        worksheet.Cells[1, 20].Value = "Bağımlı Varlıklar";
        worksheet.Cells[1, 21].Value = "RPO";
        worksheet.Cells[1, 22].Value = "RTO";
        worksheet.Cells[1, 23].Value = "MTPD";
        worksheet.Cells[1, 24].Value = "Kurtarma Planları";
        worksheet.Cells[1, 25].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 26].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 27].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 27])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < surecList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("S", surecList[i].Id);
            worksheet.Cells[i + 2, 2].Value = surecList[i].Kategori;
            worksheet.Cells[i + 2, 3].Value = surecList[i].AltKategori;
            worksheet.Cells[i + 2, 4].Value = surecList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = surecList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = surecList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = surecList[i].Durum;
            worksheet.Cells[i + 2, 8].Value = surecList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = surecList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = surecList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = surecList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = surecList[i].BilgiSinifi;
            worksheet.Cells[i + 2, 13].Value =
                surecList[i].Gizlilik?.StartsWith('a') == true ? "1 puan" :
                surecList[i].Gizlilik?.StartsWith('b') == true ? "2 puan" :
                surecList[i].Gizlilik?.StartsWith('c') == true ? "3 puan" :
                surecList[i].Gizlilik?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 14].Value =
                surecList[i].Butunluk?.StartsWith('a') == true ? "1 puan" :
                surecList[i].Butunluk?.StartsWith('b') == true ? "2 puan" :
                surecList[i].Butunluk?.StartsWith('c') == true ? "3 puan" :
                surecList[i].Butunluk?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 15].Value =
                surecList[i].Erisilebilirlik?.StartsWith('a') == true ? "1 puan" :
                surecList[i].Erisilebilirlik?.StartsWith('b') == true ? "2 puan" :
                surecList[i].Erisilebilirlik?.StartsWith('c') == true ? "3 puan" :
                surecList[i].Erisilebilirlik?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 16].Value =
                surecList[i].EtkilenenKisiSayisi?.StartsWith('a') == true ? "1 puan" :
                surecList[i].EtkilenenKisiSayisi?.StartsWith('b') == true ? "2 puan" :
                surecList[i].EtkilenenKisiSayisi?.StartsWith('c') == true ? "3 puan" :
                surecList[i].EtkilenenKisiSayisi?.StartsWith('d') == true ? "4 puan" :
                surecList[i].EtkilenenKisiSayisi?.StartsWith('e') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 17].Value =
                surecList[i].ToplumsalSonuc?.StartsWith('a') == true ? "1 puan" :
                surecList[i].ToplumsalSonuc?.StartsWith('b') == true ? "2 puan" :
                surecList[i].ToplumsalSonuc?.StartsWith('c') == true ? "3 puan" :
                surecList[i].ToplumsalSonuc?.StartsWith('d') == true ? "5 puan" :
                surecList[i].ToplumsalSonuc?.StartsWith('e') == true ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 18].Value =
                surecList[i].KurumsalSonuc?.StartsWith('a') == true ? "1 puan" :
                surecList[i].KurumsalSonuc?.StartsWith('b') == true ? "2 puan" :
                surecList[i].KurumsalSonuc?.StartsWith('c') == true ? "3 puan" :
                "";
            worksheet.Cells[i + 2, 19].Value =
                surecList[i].SektorelEtki?.StartsWith('a') == true ? "1 puan" :
                surecList[i].SektorelEtki?.StartsWith('b') == true ? "2 puan" :
                surecList[i].SektorelEtki?.StartsWith('c') == true ? "3 puan" :
                surecList[i].SektorelEtki?.StartsWith('d') == true ? "5 puan" :
                "";
            worksheet.Cells[i + 2, 20].Value =
                surecList[i].BagimliVarlik?.StartsWith('a') == true ? "1 puan" :
                surecList[i].BagimliVarlik?.StartsWith('b') == true ? "2 puan" :
                surecList[i].BagimliVarlik?.StartsWith('c') == true ? "3 puan" :
                surecList[i].BagimliVarlik?.StartsWith('d') == true ? "5 puan" :
                surecList[i].BagimliVarlik?.StartsWith('e') == true ? "6 puan" :
                "";
            worksheet.Cells[i + 2, 21].Value = surecList[i].Rpo;
            worksheet.Cells[i + 2, 22].Value = surecList[i].Rto;
            worksheet.Cells[i + 2, 23].Value = surecList[i].Mtpd;
            worksheet.Cells[i + 2, 24].Value = surecList[i].KurtarmaPlanlari;
            worksheet.Cells[i + 2, 25].Value = surecList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 26].Value = surecList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 27].Value = surecList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelRaporlama(
        string? search, FilterBag? filterBag = null)
    {
        var raporlamaList = await raporlamaService.GetAllExcelAsync(
            search, filterBag);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Raporlama");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Varlık Kategorileri";
        worksheet.Cells[1, 3].Value = "Varlık Grubu Adı";
        worksheet.Cells[1, 4].Value = "Varlık Adı";
        worksheet.Cells[1, 5].Value = "Kullanım Amacı";
        worksheet.Cells[1, 6].Value = "Miktar";
        worksheet.Cells[1, 7].Value = "Durum";
        worksheet.Cells[1, 8].Value = "Konum";
        worksheet.Cells[1, 9].Value = "Varlık Sahibi";
        worksheet.Cells[1, 10].Value = "Varlık Sahibi Alt Departman";
        worksheet.Cells[1, 11].Value = "Operasyonel Sahibi";
        worksheet.Cells[1, 12].Value = "Envantere Giriş Tarihi";
        worksheet.Cells[1, 13].Value = "Envanter Güncelleme Tarihi";
        worksheet.Cells[1, 14].Value = "Envanterden Çıkış Tarihi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 27])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < raporlamaList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("S", raporlamaList[i].Id);
            worksheet.Cells[i + 2, 2].Value = raporlamaList[i].KategoriAd;
            worksheet.Cells[i + 2, 3].Value = raporlamaList[i].AltKategoriAd;
            worksheet.Cells[i + 2, 4].Value = raporlamaList[i].VarlikAdi;
            worksheet.Cells[i + 2, 5].Value = raporlamaList[i].KullanimAmaci;
            worksheet.Cells[i + 2, 6].Value = raporlamaList[i].Miktar;
            worksheet.Cells[i + 2, 7].Value = raporlamaList[i].DurumAd;
            worksheet.Cells[i + 2, 8].Value = raporlamaList[i].Konum;
            worksheet.Cells[i + 2, 9].Value = raporlamaList[i].VarlikSahibi;
            worksheet.Cells[i + 2, 10].Value = raporlamaList[i].VarlikSahibiAltDepartman;
            worksheet.Cells[i + 2, 11].Value = raporlamaList[i].OperasyonelSahibi;
            worksheet.Cells[i + 2, 12].Value = raporlamaList[i].EnvantereGirisTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 13].Value = raporlamaList[i].EnvanterGuncellemeTarihi?.ToString("dd.MM.yyyy");
            worksheet.Cells[i + 2, 14].Value = raporlamaList[i].EnvanterdenCikisTarihi?.ToString("dd.MM.yyyy");
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<MemoryStream> GenerateExcelEpostaTalepleri(
        string? search = null)
    {
        var epostaTalepList = await epostaTalepService.GetAllExcelAsync(search ?? "");

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Eposta Talepleri");

        // Add headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Kurum";
        worksheet.Cells[1, 3].Value = "Üçüncü Taraf";
        worksheet.Cells[1, 4].Value = "Talep Edilen";
        worksheet.Cells[1, 5].Value = "Talep Eden";
        worksheet.Cells[1, 6].Value = "Talep Nedeni";
        worksheet.Cells[1, 7].Value = "Talep Süresi";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 7])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Add data
        for (var i = 0; i < epostaTalepList.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = FormatId("ET", epostaTalepList[i].Id);
            worksheet.Cells[i + 2, 2].Value = epostaTalepList[i].KurumAd;
            worksheet.Cells[i + 2, 3].Value = epostaTalepList[i].UcuncuTaraf;
            worksheet.Cells[i + 2, 4].Value = epostaTalepList[i].TalepEdilen;
            worksheet.Cells[i + 2, 5].Value = epostaTalepList[i].TalepEden;
            worksheet.Cells[i + 2, 6].Value = epostaTalepList[i].TalepNedeni;
            worksheet.Cells[i + 2, 7].Value = epostaTalepList[i].TalepSuresi;
        }

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();

        // Return the Excel file
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public static string FormatId(string prefix, int id)
    {
        return $"{prefix}{id:D5}";
    }
}