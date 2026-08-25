DROP VIEW IF EXISTS vw_kriptografi_envanteri_detay CASCADE;

-- Kriptografi envanteri diğer varlık türlerinden farklı bir şemaya sahiptir:
-- kategori, durum ve konum alanları yoktur, bunun yerine kendi denetim
-- alanlarını (created_date / updated_date / deleted_date) taşır.
CREATE VIEW vw_kriptografi_envanteri_detay AS
SELECT
    t.id,
    t.organizasyon_id,
    t.varlik_adi,
    t.varlik_sahibi,
    t.varlik_sahibi_id,
    t.varlik_sahibi_alt_departman,
    t.varlik_sahibi_alt_departman_id,
    t.uretim_yeri,
    t.kullanim_amaci,
    t.olusturma_tarihi,
    sure_metni(t.kullanim_suresi, t.kullanim_suresi_tip) AS kullanim_suresi,
    ans.ad                                              AS anahtar_sorumlusu,
    t.anahtar_saklama_alani,
    t.destek_alinan_tedarikci,
    t.donanim_yazilim,
    t.algoritma,
    t.ortak_kriterler,
    ksv.ad                                              AS kullanim_seviyesi,
    t.kullanim_kabiliyetleri,
    t.notlar,
    t.aktif,
    t.created_date,
    t.updated_date,
    t.deleted_date
FROM kriptografi_envanterleri t
LEFT JOIN anahtar_sorumlulari   ans ON ans.id = t.anahtar_sorumlusu_id
LEFT JOIN kullanim_seviyeleri   ksv ON ksv.id = t.kullanim_seviyesi_id
WHERE COALESCE(t.silinsin_mi, false) = false;
