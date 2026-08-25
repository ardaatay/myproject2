DROP VIEW IF EXISTS vw_raporlama_genel CASCADE;

-- Türden bağımsız genel rapor: on varlık tablosunun ortak alanlarını birleştirir.
-- Kriptografi envanteri farklı şemada olduğu (kategori/durum/konum taşımadığı)
-- için bu birleşimin dışındadır.
--
-- id değerleri kaynak tablo içinde benzersizdir, birleşim genelinde değil.
-- Görünüm HasNoKey ile eşlendiği ve yalnızca okuma amaçlı kullanıldığı için
-- bu bir sorun oluşturmaz.
CREATE VIEW vw_raporlama_genel AS
WITH varliklar AS (
    SELECT id, organizasyon_id, kategori_id, alt_kategori_id, varlik_adi, kullanim_amaci, miktar, durum_id,
           konum, konum_id, varlik_sahibi, varlik_sahibi_id,
           varlik_sahibi_alt_departman, varlik_sahibi_alt_departman_id, operasyonel_sahibi,
           envantere_giris_tarihi, envanter_guncelleme_tarihi, envanterden_cikis_tarihi
    FROM agve_sistemler              WHERE COALESCE(silinsin_mi, false) = false
    UNION ALL
    SELECT id, organizasyon_id, kategori_id, alt_kategori_id, varlik_adi, kullanim_amaci, miktar, durum_id,
           konum, konum_id, varlik_sahibi, varlik_sahibi_id,
           varlik_sahibi_alt_departman, varlik_sahibi_alt_departman_id, operasyonel_sahibi,
           envantere_giris_tarihi, envanter_guncelleme_tarihi, envanterden_cikis_tarihi
    FROM basili_bilgiler             WHERE COALESCE(silinsin_mi, false) = false
    UNION ALL
    SELECT id, organizasyon_id, kategori_id, alt_kategori_id, varlik_adi, kullanim_amaci, miktar, durum_id,
           konum, konum_id, varlik_sahibi, varlik_sahibi_id,
           varlik_sahibi_alt_departman, varlik_sahibi_alt_departman_id, operasyonel_sahibi,
           envantere_giris_tarihi, envanter_guncelleme_tarihi, envanterden_cikis_tarihi
    FROM elektronik_bilgiler         WHERE COALESCE(silinsin_mi, false) = false
    UNION ALL
    SELECT id, organizasyon_id, kategori_id, alt_kategori_id, varlik_adi, kullanim_amaci, miktar, durum_id,
           konum, konum_id, varlik_sahibi, varlik_sahibi_id,
           varlik_sahibi_alt_departman, varlik_sahibi_alt_departman_id, operasyonel_sahibi,
           envantere_giris_tarihi, envanter_guncelleme_tarihi, envanterden_cikis_tarihi
    FROM fiziksel_mekanlar           WHERE COALESCE(silinsin_mi, false) = false
    UNION ALL
    SELECT id, organizasyon_id, kategori_id, alt_kategori_id, varlik_adi, kullanim_amaci, miktar, durum_id,
           konum, konum_id, varlik_sahibi, varlik_sahibi_id,
           varlik_sahibi_alt_departman, varlik_sahibi_alt_departman_id, operasyonel_sahibi,
           envantere_giris_tarihi, envanter_guncelleme_tarihi, envanterden_cikis_tarihi
    FROM iot_cihazlari               WHERE COALESCE(silinsin_mi, false) = false
    UNION ALL
    SELECT id, organizasyon_id, kategori_id, alt_kategori_id, varlik_adi, kullanim_amaci, miktar, durum_id,
           konum, konum_id, varlik_sahibi, varlik_sahibi_id,
           varlik_sahibi_alt_departman, varlik_sahibi_alt_departman_id, operasyonel_sahibi,
           envantere_giris_tarihi, envanter_guncelleme_tarihi, envanterden_cikis_tarihi
    FROM personeller                 WHERE COALESCE(silinsin_mi, false) = false
    UNION ALL
    SELECT id, organizasyon_id, kategori_id, alt_kategori_id, varlik_adi, kullanim_amaci, miktar, durum_id,
           konum, konum_id, varlik_sahibi, varlik_sahibi_id,
           varlik_sahibi_alt_departman, varlik_sahibi_alt_departman_id, operasyonel_sahibi,
           envantere_giris_tarihi, envanter_guncelleme_tarihi, envanterden_cikis_tarihi
    FROM surecler                    WHERE COALESCE(silinsin_mi, false) = false
    UNION ALL
    SELECT id, organizasyon_id, kategori_id, alt_kategori_id, varlik_adi, kullanim_amaci, miktar, durum_id,
           konum, konum_id, varlik_sahibi, varlik_sahibi_id,
           varlik_sahibi_alt_departman, varlik_sahibi_alt_departman_id, operasyonel_sahibi,
           envantere_giris_tarihi, envanter_guncelleme_tarihi, envanterden_cikis_tarihi
    FROM tasinabilir_cihaz_ve_ortamlar WHERE COALESCE(silinsin_mi, false) = false
    UNION ALL
    SELECT id, organizasyon_id, kategori_id, alt_kategori_id, varlik_adi, kullanim_amaci, miktar, durum_id,
           konum, konum_id, varlik_sahibi, varlik_sahibi_id,
           varlik_sahibi_alt_departman, varlik_sahibi_alt_departman_id, operasyonel_sahibi,
           envantere_giris_tarihi, envanter_guncelleme_tarihi, envanterden_cikis_tarihi
    FROM uygulamalar                 WHERE COALESCE(silinsin_mi, false) = false
    UNION ALL
    SELECT id, organizasyon_id, kategori_id, alt_kategori_id, varlik_adi, kullanim_amaci, miktar, durum_id,
           konum, konum_id, varlik_sahibi, varlik_sahibi_id,
           varlik_sahibi_alt_departman, varlik_sahibi_alt_departman_id, operasyonel_sahibi,
           envantere_giris_tarihi, envanter_guncelleme_tarihi, envanterden_cikis_tarihi
    FROM veritabanlari               WHERE COALESCE(silinsin_mi, false) = false
)
SELECT
    v.id,
    v.organizasyon_id,
    v.kategori_id,
    kat.ad                              AS kategori_ad,
    v.alt_kategori_id,
    alt.ad                              AS alt_kategori_ad,
    v.varlik_adi,
    v.kullanim_amaci,
    v.miktar,
    v.durum_id,
    dur.ad                              AS durum_ad,
    v.konum,
    v.konum_id,
    v.varlik_sahibi,
    v.varlik_sahibi_id,
    v.varlik_sahibi_alt_departman,
    v.varlik_sahibi_alt_departman_id,
    v.operasyonel_sahibi,
    v.envantere_giris_tarihi,
    v.envanter_guncelleme_tarihi,
    v.envanterden_cikis_tarihi
FROM varliklar v
LEFT JOIN kategoriler kat ON kat.id = v.kategori_id
LEFT JOIN kategoriler alt ON alt.id = v.alt_kategori_id
LEFT JOIN durumlar    dur ON dur.id = v.durum_id;
