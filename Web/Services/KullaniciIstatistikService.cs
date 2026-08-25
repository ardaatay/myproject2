using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Web.Services
{
    public class KullaniciIstatistikService
    {
        private static readonly ConcurrentDictionary<string, DateTime> _aktifKullanicilar = new ConcurrentDictionary<string, DateTime>();
        private static int _toplamGirisSayisi = 0;
        private static readonly object _lockObject = new object();
        private readonly ILogger<KullaniciIstatistikService> _logger;
        private readonly string _istatistikDosyaYolu;

        /// <summary>
        /// Dosyaya yazılamıyorsa false olur ve istatistikler yalnızca bellekte tutulur.
        /// Bu bir yan özelliktir; kalıcılığın çalışmaması girişi engellememelidir.
        /// </summary>
        private readonly bool _kaliciKayitAcik;

        public KullaniciIstatistikService(
            ILogger<KullaniciIstatistikService> logger,
            IConfiguration configuration)
        {
            _logger = logger;

            // Konteynerde /app root'a aittir ve uygulama root olmayan kullanıcıyla
            // çalışır; bu yüzden dizin dışarıdan verilebilir olmalıdır.
            var veriDizini = configuration.GetValue<string>("UygulamaAyarlari:VeriDizini");
            if (string.IsNullOrWhiteSpace(veriDizini))
                veriDizini = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");

            _istatistikDosyaYolu = Path.Combine(veriDizini, "kullanici_istatistik.json");

            try
            {
                Directory.CreateDirectory(veriDizini);
                IstatistikleriYukle();
                _kaliciKayitAcik = true;
            }
            catch (Exception ex)
            {
                _kaliciKayitAcik = false;
                _logger.LogWarning(ex,
                    "Kullanıcı istatistikleri {Dizin} dizinine yazılamıyor. " +
                    "İstatistikler yalnızca bellekte tutulacak.", veriDizini);
            }
        }

        // Kullanıcı giriş yaptığında çağrılır
        public void KullaniciGirisYapti(string kullaniciId)
        {
            if (string.IsNullOrEmpty(kullaniciId))
            {
                _logger.LogWarning("KullaniciGirisYapti metodu boş veya null kullanıcı ID ile çağrıldı");
                return;
            }

            try
            {
                _logger.LogInformation("Kullanıcı giriş yapıyor: {KullaniciId}", kullaniciId);
                _aktifKullanicilar.AddOrUpdate(kullaniciId, DateTime.Now, (key, oldValue) => DateTime.Now);
                
                lock (_lockObject)
                {
                    _toplamGirisSayisi++;
                    _logger.LogInformation("Toplam giriş sayısı artırıldı: {ToplamGirisSayisi}", _toplamGirisSayisi);
                    IstatistikleriKaydet();
                }
                
                _logger.LogInformation("Kullanıcı giriş yaptı: {KullaniciId}", kullaniciId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı giriş istatistiği güncellenirken hata oluştu: {KullaniciId}", kullaniciId);
            }
        }

        // Kullanıcı çıkış yaptığında çağrılır
        public void KullaniciCikisYapti(string kullaniciId)
        {
            if (string.IsNullOrEmpty(kullaniciId))
            {
                _logger.LogWarning("KullaniciCikisYapti metodu boş veya null kullanıcı ID ile çağrıldı");
                return;
            }

            try
            {
                if (_aktifKullanicilar.TryRemove(kullaniciId, out _))
                {
                    _logger.LogInformation("Kullanıcı çıkış yaptı: {KullaniciId}", kullaniciId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı çıkış istatistiği güncellenirken hata oluştu: {KullaniciId}", kullaniciId);
            }
        }

        // Aktif kullanıcıları temizle (30 dakika hareketsiz olanları)
        public void EskiOturumlariTemizle()
        {
            try
            {
                var eskiOturumZamani = DateTime.Now.AddMinutes(-30);
                var eskiOturumlar = _aktifKullanicilar.Where(x => x.Value < eskiOturumZamani).Select(x => x.Key).ToList();
                
                if (eskiOturumlar.Any())
                {
                    _logger.LogInformation("Eski oturumlar temizleniyor: {Count} kullanıcı", eskiOturumlar.Count);
                    
                    foreach (var kullaniciId in eskiOturumlar)
                    {
                        _aktifKullanicilar.TryRemove(kullaniciId, out _);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eski oturumlar temizlenirken hata oluştu");
            }
        }

        // Aktif kullanıcı sayısını al
        public int GetAktifKullaniciSayisi()
        {
            try
            {
                EskiOturumlariTemizle();
                return _aktifKullanicilar.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Aktif kullanıcı sayısı alınırken hata oluştu");
                return 0;
            }
        }

        // Toplam giriş sayısını al
        public int GetToplamGirisSayisi()
        {
            return _toplamGirisSayisi;
        }

        // Kullanıcı aktivitesini güncelle
        public void KullaniciAktivitesiGuncelle(string kullaniciId)
        {
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return;
            }

            try
            {
                _aktifKullanicilar.AddOrUpdate(kullaniciId, DateTime.Now, (key, oldValue) => DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı aktivitesi güncellenirken hata oluştu: {KullaniciId}", kullaniciId);
            }
        }
        
        // İstatistikleri JSON dosyasına kaydet
        private void IstatistikleriKaydet()
        {
            if (!_kaliciKayitAcik)
                return;

            try
            {
                var istatistikler = new KullaniciIstatistikleri
                {
                    ToplamGirisSayisi = _toplamGirisSayisi,
                    SonGuncelleme = DateTime.Now
                };
                
                var jsonString = JsonSerializer.Serialize(istatistikler, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_istatistikDosyaYolu, jsonString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İstatistikler dosyaya kaydedilirken hata oluştu");
            }
        }
        
        // İstatistikleri JSON dosyasından yükle
        private void IstatistikleriYukle()
        {
            try
            {
                if (File.Exists(_istatistikDosyaYolu))
                {
                    var jsonString = File.ReadAllText(_istatistikDosyaYolu);
                    var istatistikler = JsonSerializer.Deserialize<KullaniciIstatistikleri>(jsonString);
                    
                    if (istatistikler != null)
                    {
                        _toplamGirisSayisi = istatistikler.ToplamGirisSayisi;
                        _logger.LogInformation("İstatistikler dosyadan yüklendi. Toplam giriş sayısı: {ToplamGirisSayisi}", _toplamGirisSayisi);
                    }
                }
                else
                {
                    _logger.LogInformation("İstatistik dosyası bulunamadı. Yeni bir dosya oluşturulacak.");
                    IstatistikleriKaydet();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İstatistikler dosyadan yüklenirken hata oluştu");
            }
        }
    }
    
    // JSON dosyasında saklanacak istatistik sınıfı
    public class KullaniciIstatistikleri
    {
        public int ToplamGirisSayisi { get; set; }
        public DateTime SonGuncelleme { get; set; }
    }
}