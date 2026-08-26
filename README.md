# VarlıkEnvanteri

Bilgi güvenliği varlık envanteri yönetim uygulaması. ASP.NET Core MVC (.NET 10),
Entity Framework Core 10, PostgreSQL.

## Docker ile çalıştırma

```bash
cp .env.example .env      # değerleri doldurun, POSTGRES_PASSWORD zorunlu
docker compose up -d --build
```

Uygulama <http://localhost:8080> adresinde açılır. İlk açılışta veritabanı şeması
migration'larla kurulur, görünümler oluşturulur ve başlangıç verisi tohumlanır.

### İlk giriş

`.env` içinde `YONETICI_SIFRESI` boş bırakılırsa rastgele bir geçici şifre üretilip
uygulama günlüğüne yazılır:

```bash
docker compose logs web | grep "Yönetici hesabı oluşturuldu"
```

Bu şifre yalnızca bir kez gösterilir ve ilk girişte değiştirilmesi zorunludur.
Yönetici hesabı `Merkez` adlı kök birime bağlanır; birim ağacını giriş yaptıktan
sonra düzenleyebilirsiniz.

Makinede zaten bir PostgreSQL çalışıyorsa `.env` içindeki `POSTGRES_PORT` değerini
değiştirin; uygulama veritabanına konteyner ağı üzerinden bağlandığı için bundan
etkilenmez.

```bash
docker compose logs -f web    # günlükler
docker compose down           # durdur
docker compose down -v        # durdur ve veritabanını sil
```

Sağlık durumu `/health` adresinden okunabilir; compose bunu konteyner sağlık
kontrolü olarak kullanır.

### Şifre sıfırlama

Şifresini unutan kullanıcı için yönetici, **Kullanıcılar** ekranındaki anahtar
düğmesiyle tek kullanımlık bir şifre üretir. Şifre yalnızca bir kez gösterilir,
kullanıcının açık oturumları kapanır ve ilk girişte değiştirmesi istenir.
E-posta altyapısı gerekmez.

Active Directory üzerinden giriş yapan hesaplarda şifre uygulamada tutulmadığı için
bu düğme kapalıdır; sıfırlama dizin tarafında yapılır.

### Kimlik doğrulama: yerel ve Active Directory

Giriş yöntemi **kullanıcı bazında** seçilir. Aynı kurumda yerel hesaplarla dizin
hesapları bir arada bulunabilir.

| Yöntem | Şifre nerede | Uygulamada sıfırlanabilir mi |
|---|---|---|
| Yerel | Uygulama veritabanında PBKDF2 karması olarak | Evet |
| Active Directory | Yalnızca dizinde | Hayır |

**Ayarlar.** Bağlantı bilgileri **Kullanıcılar → Active Directory Ayarları**
ekranından girilir ve veritabanında kiracı başına tek kayıtta tutulur; `appsettings`
içinde dizin bilgisi bulunmaz. Servis hesabının şifresi DataProtection ile
şifrelenerek saklanır — bu yüzden `UygulamaAyarlari:VeriDizini` kalıcı bir dizini
göstermelidir, aksi halde anahtarlar yenilendiğinde şifrenin yeniden girilmesi
gerekir. Ekran bu durumu uyarı olarak bildirir.

Ayarlar kaydedilmeden önce **Bağlantıyı sına** düğmesiyle denenebilir. Test
kullanıcı adı ve şifresi girilirse gerçek giriş akışının aynısı (arama filtresi ve
zorunlu grup denetimi dahil) çalıştırılır; girilen test şifresi hiçbir yere yazılmaz.

**Kullanıcı tanımlama.** Kullanıcı ekleme ve düzenleme ekranlarında *Giriş yöntemi*
seçilir. Active Directory seçilirse, dizindeki hesap adı uygulamadakinden farklıysa
ayrıca girilebilir; boş bırakılırsa kullanıcı adının kendisi aranır. Roller, birim
ve yetkiler her iki yöntemde de uygulama tarafında yönetilir — dizin yalnızca
kimliği doğrular.

Yöntem değiştirildiğinde kullanıcının yerel şifresi silinir ve açık oturumları
kapatılır. Dizinden yerele dönen bir hesabın giriş yapabilmesi için yönetici
şifre sıfırlaması yapmalıdır.

**Güvenlik.** Şifre bağlantı üzerinden gönderildiği için LDAPS (636) ya da StartTLS
kullanılması önerilir. *Zorunlu grup* tanımlıysa iç içe gruplar da sayılır ve üyelik
doğrulanamadığında giriş reddedilir. Dizin girişleri de yerel giriş gibi hatalı
deneme sayacına ve hesap kilitlemeye tabidir.

**Konteynerde.** LDAP istemci kütüphanesi (`libldap.so.2`) çalışma zamanında
yüklenir; `Dockerfile` bunu kurar. Kendi imajınızı hazırlıyorsanız kütüphanenin
bulunduğundan emin olun, yoksa dizin girişi çalışmaz.

## Yerel geliştirme

PostgreSQL'i tek başına ayağa kaldırıp uygulamayı makinede çalıştırabilirsiniz:

```bash
cp Web/appsettings.Development.json.example Web/appsettings.Development.json
docker compose up -d db
dotnet run --project Web
```

Bağlantı dizesi `Web/appsettings.Development.json` içinden okunur. Bu dosya
depoya işlenmez; şablonu `.example` uzantılı sürümüdür.

## Railway ile yayınlama

Depoda `railway.json` bulunur: Railway kök dizindeki `Dockerfile` ile derler,
sağlık kontrolü olarak `/health` ucunu kullanır ve tek örnek çalıştırır.

Web servisine tanımlanacak değişkenler:

```
DATABASE_URL=${{Postgres.DATABASE_URL}}
HttpsYonlendirmesiAcik=false
Veritabani__BaslangictaMigrateEt=true
Veritabani__BaslangictaGorunumleriUygula=true
Veritabani__BaslangicVerisiniKur=true
Veritabani__YoneticiKullaniciAdi=admin
Veritabani__YoneticiSifresi=<güçlü bir şifre>
```

`Postgres` yerine veritabanı servisinin paneldeki tam adını yazın. Uygulama URL
biçimini Npgsql biçimine kendisi çevirdiği için ayrıca bağlantı dizesi kurmanız
gerekmez. Dinlenecek portu Railway `PORT` değişkeniyle bildirir ve imaj bunu
dikkate alır; hedef port ayarıyla uğraşmanız gerekmez.

`HttpsYonlendirmesiAcik=false` zorunludur. TLS bağlantısını Railway sonlandırır,
konteynere istek HTTP olarak ulaşır; ayar açık bırakılırsa yönlendirme döngüsü
oluşur.

**Kalıcı disk gerekir.** DataProtection anahtarları ve kullanıcı istatistikleri
`/var/lib/varlik` altına yazılır. Railway'in dosya sistemi geçici olduğundan bu
yola bir Volume bağlanmazsa her yeni dağıtımda anahtarlar değişir ve açık olan
tüm oturumlar düşer. Aynı nedenle örnek sayısı 1'de kalmalıdır: dosya tabanlı
anahtarlık birden fazla örnek arasında paylaşılmaz.

İlk açılıştan sonra migration ve tohumlama satırlarını `false` yapabilirsiniz;
tohumlama zaten kullanıcı varken hiçbir şey yapmaz.

Özel ağ üzerinden bağlanırken (`*.railway.internal`) SSL gerekmez. Genel TCP
vekilini kullanmanız gerekirse adresin sonuna `?sslmode=require` ekleyin;
Npgsql 8'den beri `Require` şifreler ama sertifika zincirini doğrulamaz, bu
yüzden ayrıca bir "sertifikaya güven" ayarı gerekmez.

## Konfigürasyon

Tüm ayarlar ortam değişkeniyle geçersiz kılınabilir (`__` iç içe anahtarları ayırır).

| Anahtar | Varsayılan | Açıklama |
|---|---|---|
| `ConnectionStrings__DefaultConnection` | — | PostgreSQL bağlantı dizesi (Npgsql anahtar=değer biçimi) |
| `DATABASE_URL` | — | Alternatif: `postgresql://kullanici:sifre@sunucu:port/veritabani`. Üstteki boşsa kullanılır, Npgsql biçimine çevrilir |
| `PORT` | — | Dinlenecek port. Tanımlıysa imajdaki 8080 varsayılanını geçersiz kılar |
| `Veritabani__BaslangictaMigrateEt` | `false` | Açılışta migration'ları uygula |
| `Veritabani__BaslangictaGorunumleriUygula` | `false` | Açılışta SQL görünümlerini oluştur |
| `Veritabani__BaslangicVerisiniKur` | `false` | Roller, kök birim ve yönetici hesabını tohumla |
| `Veritabani__YoneticiKullaniciAdi` | `admin` | Tohumlanacak yönetici hesabının adı |
| `Veritabani__YoneticiSifresi` | — | Boşsa rastgele üretilip günlüğe yazılır |
| `UygulamaAyarlari__UygulamaAdi` | `Varlık Envanteri` | Tarayıcı başlığı ve üst çubuktaki ad |
| `UygulamaAyarlari__LogoYolu` | `/img/logo.svg` | Logo ve favicon yolu (`wwwroot` köküne göre) |
| `UygulamaAyarlari__Kultur` | `tr-TR` | Sayı, tarih ve harf büyütme kuralları |
| `UygulamaAyarlari__TarihFormati` | `dd.MM.yyyy` | Kısa tarih biçimi |
| `UygulamaAyarlari__OturumSuresiDk` | `60` | Oturum ve kimlik çerezi süresi |
| `UygulamaAyarlari__VeriDizini` | — | Yazılabilir veri dizini (istatistik, DataProtection anahtarları) |
| `UygulamaAyarlari__SifrePolitikasi__*` | — | Uzunluk, karakter sınıfları, kilit eşiği ve süresi |
| `HttpsYonlendirmesiAcik` | `true` | TLS'i ters vekil sonlandırıyorsa `false` yapın |

Kurumsal görünümü değiştirmek için `Web/wwwroot/img/logo.svg` dosyasını
değiştirin veya `UygulamaAyarlari__LogoYolu` ile başka bir dosyayı gösterin.

### Çok kiracılılık

Veriler **organizasyon** bazında izole edilir. Kiracıya ait her entity
`IKiraciEntity` uygular ve `organizasyon_id` taşır; DbContext bunlara global
sorgu filtresi kurar ve yeni kayıtlara aktif organizasyonu otomatik atar.

| | |
|---|---|
| İzole | Varlık tabloları, birimler, kullanıcılar, roller ataması, e-posta talepleri, loglar |
| Ortak | Referans/liste tabloları (gizlilik, kategori, durum, konum, sorumlular …) ve rol tanımları |

Aktif organizasyon, oturum açan kullanıcının `OrganizasyonId` claim'inden gelir.
`SUPERADMIN` rolüne sahip kullanıcılar filtreden muaftır ve tüm kiracıları görür.

> `Organizasyon` ile `Kurum` farklı şeylerdir. `Organizasyon` kiracıdır —
> uygulamayı kullanan ve verinin sahibi olan kurumdur. `Kurum` ise e-posta
> taleplerinde "bu talep hangi kurumla ilgili" sorusunu yanıtlayan, üçüncü taraf
> kurumları da içerebilen ortak bir referans listesidir.

Temiz kurulumda tek bir organizasyon tohumlanır; tek kiracılı dağıtımlarda
başka bir şey yapmanız gerekmez.

### Yetkilendirme

Erişim kontrolü rol adlarına değil **politikalara** dayanır. Kod içinde
`[Authorize(Policy = Yetkiler.TeknikVarlikDuzenle)]` gibi izinler kullanılır;
hangi rolün hangi izne sahip olduğu `appsettings.json` içindeki `Yetkilendirme`
bölümünden gelir. Böylece kurumlar kendi rol adlarını kullanabilir.

| Politika grubu | Kapsam |
|---|---|
| `TeknikVarlik.*` | Ağ/sistem, uygulama, taşınabilir cihaz, IoT, fiziksel mekan, personel |
| `BilgiVarlik.*` | Basılı ve elektronik bilgiler, süreçler, veritabanları |
| `Kripto.*` | Kriptografi envanteri |
| `EpostaTalep.*` | E-posta talepleri |
| `Sistem.Yonet` | Tanımlamalar, raporlar, kullanıcı ve birim yönetimi, kalıcı silme |

Her grupta `Listele` → `Goruntule` → `Duzenle` → `Olustur` sırasıyla daralan
yetkiler bulunur. Tek bir izni ortam değişkeniyle de değiştirebilirsiniz:

```bash
Yetkilendirme__Politikalar__TeknikVarlik.Listele__0=KENDI_ROLUNUZ
```

Politika `appsettings.json` içinde tanımlanmazsa `Core/Security/YetkiAyarlari.cs`
içindeki varsayılan kullanılır.

Tohumlama yalnızca eksik kayıtları ekler; var olanlara dokunmaz. Bu yüzden
açılışta çalıştırılması güvenlidir.

## Loglama ve hata takibi

İki ayrı kayıt tutulur ve her birinin yönetim ekranı vardır. İkisi de
`Sistem.Yonet` yetkisi ister ve salt okunurdur; log düzenlenemez veya silinemez.

| Ekran | Ne tutar | Nereden yazılır |
|---|---|---|
| **İşlem Logları** | `[LogAspect]` ile işaretlenmiş iş katmanı çağrıları: kim, ne zaman, hangi parametreyle, ne kadar sürede | `LogInterceptor` |
| **Hata Logları** | Kullanıcıya hata bildirimi gösterilmesine yol açan her istisna: mesaj, yığın izi, istek, kullanıcı | `ExceptionMiddleware` |

Log kayıtları, işin kendi veritabanı işleminden **ayrı bir bağlantıyla** yazılır.
Başarısız olup geri alınan bir işlemin logu da silinseydi geriye hiçbir iz
kalmazdı — oysa asıl kaydedilmek istenen tam olarak o durumdur. Aynı nedenle
loglama hataları yutulur ve yalnızca uygulama günlüğüne düşer; log yazılamaması
kullanıcının işlemini bozmaz.

### Hata kodu

Bir hata oluştuğunda kullanıcıya teknik ayrıntı değil, `HTA-K7F4-9QXZ`
biçiminde kısa bir referans gösterilir. Kod; bildirim kutusunda, hata sayfasında
ve giriş ekranında kopyalanabilir olarak çıkar, AJAX yanıtlarında `hataKodu`
alanıyla döner.

Yönetici **Hata Logları** ekranının en üstündeki kutuya bu kodu yapıştırıp
kaydın tamamına ulaşır. Arama büyük/küçük harf, tire ve boşluk farklarına
duyarsızdır; `HTA` ön eki yazılmasa da bulur.

Aynı istekte oluşan işlem logu ile hata logu aynı kodu taşır, bu yüzden iki
ekran arasında tek tıkla geçilebilir: hatanın hangi çağrıdan ve hangi
parametrelerle doğduğu doğrudan görülür.

Kod, karıştırılan karakterleri (I, L, O, U, 0, 1) içermeyen 30 harflik bir
alfabeden üretilir; telefonda okunacak kadar kısa, tahmin edilemeyecek kadar
geniştir. Oturum açılmadan oluşan hatalar hiçbir kiracıya bağlı değildir ve
listede görünmez — koduyla aranarak bulunur.

Hata kaydı ele alındığında ayrıntı sayfasından **çözüldü** olarak işaretlenir;
liste bu duruma göre süzülebilir ve özet kartlarında açık kayıt sayısı görünür.

### Neler loglanır

Varlık, kullanıcı, rol ve rol atama işlemlerinin ekleme/güncelleme/silme
adımları `[LogAspect]` işaretiyle otomatik kaydedilir.

Oturum olayları bu sarmalın dışındadır ve elle yazılır: giriş ve şifre
akışlarının parametreleri düz metin şifre taşır, serileştirilmemelidir. Aynı
gerekçeyle Active Directory ayar değişikliği de elle kaydedilir — servis hesabı
şifresinin kendisi değil, yalnızca değiştirilip değiştirilmediği yazılır.

Listede **Oturum** modülü altında görünen olaylar:

| Olay | Ne zaman | Kayda giren |
|---|---|---|
| `Giris` | Her giriş denemesi | Kullanıcı adı, sonuç (şifre hatalı / hesap kilitli / pasif / rolü yok / dizin reddetti), IP |
| `Cikis` | Oturum kapatma | Kullanıcı adı, IP |
| `SifreDegistir` | Kullanıcının kendi şifresini değiştirmesi | Hesap ve sonuç |
| `SifreSifirla` | Yönetici sıfırlaması | Sıfırlanan hesap |

Hiçbir şifre — ne mevcut, ne yeni, ne de denenen — kayda geçmez. Başarısız
girişte yazılan tek kullanıcı girdisi denenen kullanıcı adıdır.

Girişte kullanıcı adı hiçbir hesapla eşleşmiyorsa olay bir kiracıya bağlanamaz;
kurulumda tek organizasyon varsa ona yazılır, birden fazlaysa sahipsiz kalır ve
yalnızca kurumlar arası yetkili görür.

### İşletim notları

- Tablolar sürekli büyür. Uzun süre çalışan kurulumlarda `logs` ve
  `hata_loglari` için bir saklama süresi belirleyip düzenli temizlik
  planlanmalıdır; uygulama kendiliğinden silme yapmaz.
- Bu sürümden önce yazılmış log kayıtlarında kiracı bilgisi boştur
  (`organizasyon_id = 0`); bu kayıtlar kiracı yöneticisinin listesinde
  görünmez. Tek kiracılı bir kurulumda tümünü mevcut organizasyona taşımak
  için: `UPDATE logs SET organizasyon_id = <id> WHERE organizasyon_id = 0;`

## Veritabanı şeması

Şema EF Core migration'larıyla yönetilir.

```bash
dotnet ef migrations add <Ad> --project Repository --startup-project Web --output-dir Migrations
dotnet ef database update --project Repository --startup-project Web
```

### Görünümler

EF Core görünümleri (view) migration'larla yönetmediği için tanımları
`Repository/Sql/Views/*.sql` altında ayrı tutulur, gömülü kaynak olarak derlenir
ve `Veritabani__BaslangictaGorunumleriUygula` açıkken uygulama açılışında
çalıştırılır. Her dosya `DROP VIEW ... CASCADE` ile başlar, dolayısıyla işlem
yinelenebilirdir.

Görünüm tanımını değiştirdikten sonra uygulamayı yeniden başlatmak yeterlidir;
migration üretmek gerekmez.

Tablo, sütun, indeks ve kısıt adları PostgreSQL standardı olan snake_case'e
çevrilir (`EFCore.NamingConventions`). C# tarafındaki `Birim.UstId` özelliği
veritabanında `birimler.ust_id` sütununa karşılık gelir; sorgularda tırnaklamaya
gerek yoktur:

```sql
SELECT ad, ust_id FROM birimler WHERE durum = true;
```

## Proje yapısı

| Proje | Sorumluluk |
|---|---|
| `Core` | Generic repository, unit of work, aspect'ler, ortak yardımcılar |
| `Entity` | Veritabanı entity'leri |
| `Dto` | Katmanlar arası veri transfer nesneleri |
| `Repository` | DbContext, repository implementasyonları, migration'lar |
| `Business` | İş kuralları (manager'lar), AutoMapper profili, interceptor'lar |
| `Util` | Sorgu yardımcıları |
| `Web` | MVC controller'lar, view'lar, middleware |

## Bilinen eksikler

Proje kurumdan bağımsız hale getirilme sürecinde. Aşağıdakiler henüz tamamlanmadı:

- **Kullanıcının kendi şifresini sıfırlaması mümkün değil.** Yönetici, Kullanıcılar
  ekranından tek kullanımlık şifre üretebilir; e-posta ile self-servis sıfırlama
  SMTP yapılandırması bekliyor.
- **Referans listeleri boş gelir.** Yalnızca `durumlar` tohumlanır (kimlikleri
  koda gömülü olduğu için). Gizlilik, kategori, konum ve benzeri listeler kuruma
  göre değiştiğinden ilk kurulumda elle doldurulmalıdır.
- **Organizasyon yönetim arayüzü yok.** Kiracı izolasyonu çalışıyor, ancak yeni
  organizasyon eklemek ve kullanıcıları organizasyonlara atamak şimdilik
  veritabanından yapılıyor.
- **Birim bazlı kapsamlama hâlâ rol adına bağlı.** Manager'larda "ADMIN tümünü
  görür, diğerleri kendi birimini" mantığı `IsInRole("ADMIN")` ile yazılı;
  yetkilendirmenin geri kalanı politikalara taşındı, bu kısım taşınmadı.
