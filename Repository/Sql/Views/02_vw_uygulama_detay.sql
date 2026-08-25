DROP VIEW IF EXISTS vw_uygulama_detay CASCADE;

CREATE VIEW vw_uygulama_detay AS
SELECT
    t.id,
    t.organizasyon_id,
    kat.ad                                              AS kategori,
    alt.ad                                              AS alt_kategori,
    t.varlik_adi,
    t.kullanim_amaci,
    t.miktar,
    t.durum_id,
    dur.ad                                              AS durum,
    t.konum,
    t.varlik_sahibi,
    t.varlik_sahibi_id,
    t.varlik_sahibi_alt_departman,
    t.varlik_sahibi_alt_departman_id,
    t.operasyonel_sahibi,
    t.operasyonel_sahibi_id,
    bsn.ad                                              AS bilgi_sinifi,
    giz.ad                                              AS gizlilik,
    btn.ad                                              AS butunluk,
    ers.ad                                              AS erisilebilirlik,
    eks.ad                                              AS etkilenen_kisi_sayisi,
    tsn.ad                                              AS toplumsal_sonuc,
    ksn.ad                                              AS kurumsal_sonuc,
    skt.ad                                              AS sektorel_etki,
    bgv.ad                                              AS bagimli_varlik,
    sure_metni(t.rpo,  t.rpo_tip)                       AS rpo,
    sure_metni(t.rto,  t.rto_tip)                       AS rto,
    sure_metni(t.mtpd, t.mtpd_tip)                      AS mtpd,
    t.kurtarma_planlari,
    ytp.ad                                              AS yedekleme_tipi,
    t.yedekleme_turu,
    t.yedekleme_sikligi,
    t.yedeklerin_saklama_suresi,
    t.yedekleme_alani,
    t.yedekten_donus_plani,
    ysr.ad                                              AS yedekleme_sorumlusu,
    ev_hayir(t.kriptoloji)                              AS kriptoloji,
    ktr.ad                                              AS kriptoloji_turu,
    t.kullanilan_kriptoloji,
    ans.ad                                              AS anahtar_sorumlusu,
    ev_hayir(t.kisisel_veri_barindirma)                 AS kisisel_veri_barindirma,
    ev_hayir(t.anlik_mesajlasma_kullanimi)              AS anlik_mesajlasma_kullanimi,
    ev_hayir(t.bulut_bilisim)                           AS bulut_bilisim,
    ev_hayir(t.yeni_gelistirmelerve_tedarik)            AS yeni_gelismelerve_tedarik,
    ev_hayir(t.kritik_altyapi_sistemi)                  AS kritik_altyapi_sistemi,
    t.ip_adresi,
    t.url_adresi,
    t.yazilim_surumu,
    t.yazilim_yayimcisi                                 AS yazilim_yayincisi,
    t.edinim_tarihi,
    t.lisans_adedi,
    lts.ad                                              AS lisans_takip_sorumlusu,
    ddr.ad                                              AS destek_durumu,
    t.destek_alinan_tedarikci,
    t.bakim_suresi,
    t.bakim_kapsami,
    t.yazilimin_yuklendigi_donanimlar,
    t.veritabanive_surumu,
    t.veritabani_versiyonu,
    t.notlar,
    t.envantere_giris_tarihi,
    t.envanter_guncelleme_tarihi,
    t.envanterden_cikis_tarihi
FROM uygulamalar t
LEFT JOIN kategoriler               kat ON kat.id = t.kategori_id
LEFT JOIN kategoriler               alt ON alt.id = t.alt_kategori_id
LEFT JOIN durumlar                  dur ON dur.id = t.durum_id
LEFT JOIN bilgi_siniflari           bsn ON bsn.id = t.bilgi_sinifi_id
LEFT JOIN gizlilikler               giz ON giz.id = t.gizlilik_id
LEFT JOIN butunlukler               btn ON btn.id = t.butunluk_id
LEFT JOIN erisilebilirlikler        ers ON ers.id = t.erisilebilirlik_id
LEFT JOIN etkilenen_kisi_sayilari   eks ON eks.id = t.etkilenen_kisi_sayisi_id
LEFT JOIN toplumsal_sonuclar        tsn ON tsn.id = t.toplumsal_sonuc_id
LEFT JOIN kurumsal_sonuclar         ksn ON ksn.id = t.kurumsal_sonuc_id
LEFT JOIN sektorel_etkiler          skt ON skt.id = t.sektorel_etki_id
LEFT JOIN bagimli_varliklar         bgv ON bgv.id = t.bagimli_varlik_id
LEFT JOIN yedekleme_tipleri         ytp ON ytp.id = t.yedekleme_tipi_id
LEFT JOIN yedekleme_sorumlulari     ysr ON ysr.id = t.yedekleme_sorumlusu_id
LEFT JOIN kriptoloji_turleri        ktr ON ktr.id = t.kriptoloji_turu_id
LEFT JOIN anahtar_sorumlulari       ans ON ans.id = t.anahtar_sorumlusu_id
LEFT JOIN lisans_takip_sorumlulari  lts ON lts.id = t.lisans_takip_sorumlusu_id
LEFT JOIN destek_durumlari          ddr ON ddr.id = t.destek_durumu_id
WHERE COALESCE(t.silinsin_mi, false) = false;
