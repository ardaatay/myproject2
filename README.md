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
