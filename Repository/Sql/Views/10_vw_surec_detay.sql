DROP VIEW IF EXISTS vw_surec_detay CASCADE;

CREATE VIEW vw_surec_detay AS
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
    t.rpo::text                                         AS rpo,
    t.rpo_tip,
    t.rto::text                                         AS rto,
    t.rto_tip,
    t.mtpd::text                                        AS mtpd,
    t.mtpd_tip,
    t.kurtarma_planlari,
    t.notlar,
    t.envantere_giris_tarihi,
    t.envanter_guncelleme_tarihi,
    t.envanterden_cikis_tarihi
FROM surecler t
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
WHERE COALESCE(t.silinsin_mi, false) = false;
