using System.Linq.Expressions;
using Core.Repository;
using Core.Util;
using Dto.DTOs;
using Dto.Rapor;
using Dto.Uygulama;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Repository.Abstract;
using Repository.Context;
using Util.Query;

namespace Repository.Concrete;

public class UygulamaRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<Uygulama, int>(context), IUygulamaRepository
{
    // ---------------------------------------------------------------
    // View üzerinden sorgular
    // ---------------------------------------------------------------

    public async Task<List<ListUygulamaDto>> GetListWithDetailsAsync(
        Expression<Func<ListUygulamaDto, bool>>? filter = null)
    {
        var query = context.UygulamaDetay.AsQueryable();

        if (filter != null)
            query = query.Where(filter);

        return await query.ToListAsync();
    }

    public async Task<List<ListUygulamaDto>> GetListWithDetailsAsync(
        string search, FilterBag filterBag,
        Expression<Func<ListUygulamaDto, bool>>? filter = null)
    {
        var query = context.UygulamaDetay.AsQueryable();

        query = filterBag.ApplyFilters(query);

        if (filter != null)
            query = query.Where(filter);


        if (!string.IsNullOrEmpty(search))
        {
            var s = search.ToLower();
            query = query.Where(p =>
                (p.Kategori            != null && p.Kategori.ToLower().Contains(s))            ||
                (p.AltKategori         != null && p.AltKategori.ToLower().Contains(s))         ||
                (p.VarlikAdi           != null && p.VarlikAdi.ToLower().Contains(s))           ||
                (p.KullanimAmaci       != null && p.KullanimAmaci.ToLower().Contains(s))       ||
                (p.Durum               != null && p.Durum.ToLower().Contains(s))               ||
                (p.Konum               != null && p.Konum.ToLower().Contains(s))               ||
                (p.VarlikSahibi        != null && p.VarlikSahibi.ToLower().Contains(s))        ||
                (p.VarlikSahibiAltDepartman != null && p.VarlikSahibiAltDepartman.ToLower().Contains(s)) ||
                (p.OperasyonelSahibi   != null && p.OperasyonelSahibi.ToLower().Contains(s))  ||
                (p.BilgiSinifi         != null && p.BilgiSinifi.ToLower().Contains(s))         ||
                (p.Gizlilik            != null && p.Gizlilik.ToLower().Contains(s))            ||
                (p.Butunluk            != null && p.Butunluk.ToLower().Contains(s))            ||
                (p.Erisilebilirlik     != null && p.Erisilebilirlik.ToLower().Contains(s))     ||
                (p.EtkilenenKisiSayisi != null && p.EtkilenenKisiSayisi.ToLower().Contains(s)) ||
                (p.ToplumsalSonuc      != null && p.ToplumsalSonuc.ToLower().Contains(s))      ||
                (p.KurumsalSonuc       != null && p.KurumsalSonuc.ToLower().Contains(s))       ||
                (p.SektorelEtki        != null && p.SektorelEtki.ToLower().Contains(s))        ||
                (p.BagimliVarlik       != null && p.BagimliVarlik.ToLower().Contains(s))       ||
                (p.Rpo                 != null && p.Rpo.ToLower().Contains(s))                 ||
                (p.Rto                 != null && p.Rto.ToLower().Contains(s))                 ||
                (p.Mtpd                != null && p.Mtpd.ToLower().Contains(s))                ||
                (p.KurtarmaPlanlari    != null && p.KurtarmaPlanlari.ToLower().Contains(s))    ||
                (p.YedeklemeTipi       != null && p.YedeklemeTipi.ToLower().Contains(s))       ||
                (p.YedeklemeTuru       != null && p.YedeklemeTuru.ToLower().Contains(s))       ||
                (p.YedeklemeSikligi    != null && p.YedeklemeSikligi.ToLower().Contains(s))    ||
                (p.YedeklerinSaklamaSuresi != null && p.YedeklerinSaklamaSuresi.ToLower().Contains(s)) ||
                (p.YedeklemeAlani      != null && p.YedeklemeAlani.ToLower().Contains(s))      ||
                (p.YedektenDonusPlani  != null && p.YedektenDonusPlani.ToLower().Contains(s)) ||
                (p.YedeklemeSorumlusu  != null && p.YedeklemeSorumlusu.ToLower().Contains(s)) ||
                (p.Kriptoloji          != null && p.Kriptoloji.ToLower().Contains(s))          ||
                (p.KriptolojiTuru      != null && p.KriptolojiTuru.ToLower().Contains(s))      ||
                (p.KullanilanKriptoloji != null && p.KullanilanKriptoloji.ToLower().Contains(s)) ||
                (p.AnahtarSorumlusu    != null && p.AnahtarSorumlusu.ToLower().Contains(s))   ||
                (p.KisiselVeriBarindirma    != null && p.KisiselVeriBarindirma.ToLower().Contains(s))    ||
                (p.AnlikMesajlasmaKullanimi != null && p.AnlikMesajlasmaKullanimi.ToLower().Contains(s)) ||
                (p.BulutBilisim             != null && p.BulutBilisim.ToLower().Contains(s))            ||
                (p.YeniGelismelerveTedarik  != null && p.YeniGelismelerveTedarik.ToLower().Contains(s)) ||
                (p.KritikAltyapiSistemi     != null && p.KritikAltyapiSistemi.ToLower().Contains(s))    ||
                (p.IpAdresi            != null && p.IpAdresi.ToLower().Contains(s))            ||
                (p.UrlAdresi           != null && p.UrlAdresi.ToLower().Contains(s))           ||
                (p.YazilimSurumu       != null && p.YazilimSurumu.ToLower().Contains(s))       ||
                (p.YazilimYayincisi    != null && p.YazilimYayincisi.ToLower().Contains(s))    ||
                (p.LisansTakipSorumlusu != null && p.LisansTakipSorumlusu.ToLower().Contains(s)) ||
                (p.DestekDurumu        != null && p.DestekDurumu.ToLower().Contains(s))        ||
                (p.DestekAlinanTedarikci != null && p.DestekAlinanTedarikci.ToLower().Contains(s)) ||
                (p.BakimSuresi         != null && p.BakimSuresi.ToLower().Contains(s))         ||
                (p.BakimKapsami        != null && p.BakimKapsami.ToLower().Contains(s))        ||
                (p.YaziliminYuklendigiDonanimlar != null && p.YaziliminYuklendigiDonanimlar.ToLower().Contains(s)) ||
                (p.VeritabaniveSurumu  != null && p.VeritabaniveSurumu.ToLower().Contains(s))  ||
                (p.VeritabaniVersiyonu != null && p.VeritabaniVersiyonu.ToLower().Contains(s)) ||
                (p.Notlar              != null && p.Notlar.ToLower().Contains(s))
            );
        }

        return await query.ToListAsync();
    }

    public async Task<DataTablesResponse<ListUygulamaDto>> ProcessTableRequestAsync(DataTablesRequest request,
        Expression<Func<ListUygulamaDto, bool>>? filter = null)
    {
        var query = context.UygulamaDetay.AsQueryable();

        query = request.FilterBag.ApplyFilters(query);

        if (filter != null)
            query = query.Where(filter);
  

        return await DataTablesHelper.ProcessAsync(query, request);
    }

    // ---------------------------------------------------------------
    // Rapor metodu (doğrudan entity üzerinden çalışır)
    // ---------------------------------------------------------------

    public async Task<List<RaporAnasayfa>> GetRaporUygulamalarAsync()
    {
        var uygulamalar = context.Uygulamalar
            .OrderByDescending(a => a.EnvantereGirisTarihi)
            .Take(3)
            .Select(a => new RaporAnasayfa
            {
                VarlikSahibi = a.VarlikSahibi,
                Kategori = "Uygulamalar",
                EklenmeTarihi = a.EnvantereGirisTarihi!.Value
            });

        return await uygulamalar.ToListAsync();
    }
}
