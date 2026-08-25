-- Görünümlerde tekrar eden iki dönüşüm. Fonksiyona alınmasalardı aynı CASE
-- ifadesi 100'den fazla sütunda elle tekrarlanacaktı.

-- Boolean alanlar arayüzde metin olarak gösterilir. NULL, "bilinmiyor"
-- anlamını koruduğu için metne çevrilmez.
CREATE OR REPLACE FUNCTION ev_hayir(deger boolean) RETURNS text AS $$
    SELECT CASE deger
               WHEN true  THEN 'Evet'
               WHEN false THEN 'Hayır'
               ELSE NULL
           END;
$$ LANGUAGE sql IMMUTABLE;

-- RPO/RTO/MTPD gibi süreler veritabanında sayı + birim çifti olarak durur
-- (rpo=4, rpo_tip='Saat'), arayüzde ise tek metin beklenir.
CREATE OR REPLACE FUNCTION sure_metni(deger integer, tip text) RETURNS text AS $$
    SELECT CASE
               WHEN deger IS NULL THEN NULL
               ELSE deger::text || COALESCE(' ' || NULLIF(btrim(tip), ''), '')
           END;
$$ LANGUAGE sql IMMUTABLE;
