using System.Linq.Expressions;
using Core.Repository;
using Dto.AgveSistem;
using Dto.DTOs;
using Dto.Rapor;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Repository.Abstract;
using Repository.Context;
using Core.Util;
using Util.Query;

namespace Repository.Concrete;

public class AgveSistemRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<AgveSistem, int>(context), IAgveSistemRepository
{
    // ---------------------------------------------------------------
    // View üzerinden sorgular
    // ---------------------------------------------------------------

    public async Task<List<ListAgveSistemDto>> GetListWithDetailsAsync(
        Expression<Func<ListAgveSistemDto, bool>>? filter = null)
    {
        var query = context.AgveSistemDetay.AsQueryable();

        if (filter != null)
            query = query.Where(filter);

        return await query.ToListAsync();
    }

    public async Task<List<ListAgveSistemDto>> GetListWithDetailsAsync(
        string search,
        FilterBag filterBag,
        Expression<Func<ListAgveSistemDto, bool>>? filter = null)
    {
        var query = context.AgveSistemDetay.AsQueryable();

        query = filterBag.ApplyFilters(query);

        if (filter != null)
            query = query.Where(filter);
        

        if (!string.IsNullOrEmpty(search))
        {
            var s = search.ToLower();
            query = query.Where(p =>
                (p.Kategori != null && p.Kategori.ToLower().Contains(s)) ||
                (p.AltKategori != null && p.AltKategori.ToLower().Contains(s)) ||
                (p.VarlikAdi != null && p.VarlikAdi.ToLower().Contains(s)) ||
                (p.KullanimAmaci != null && p.KullanimAmaci.ToLower().Contains(s)) ||
                (p.Durum != null && p.Durum.ToLower().Contains(s)) ||
                (p.Konum != null && p.Konum.ToLower().Contains(s)) ||
                (p.VarlikSahibi != null && p.VarlikSahibi.ToLower().Contains(s)) ||
                (p.VarlikSahibiAltDepartman != null && p.VarlikSahibiAltDepartman.ToLower().Contains(s)) ||
                (p.OperasyonelSahibi != null && p.OperasyonelSahibi.ToLower().Contains(s)) ||
                (p.BilgiSinifi != null && p.BilgiSinifi.ToLower().Contains(s)) ||
                (p.Gizlilik != null && p.Gizlilik.ToLower().Contains(s)) ||
                (p.Butunluk != null && p.Butunluk.ToLower().Contains(s)) ||
                (p.Erisilebilirlik != null && p.Erisilebilirlik.ToLower().Contains(s)) ||
                (p.EtkilenenKisiSayisi != null && p.EtkilenenKisiSayisi.ToLower().Contains(s)) ||
                (p.ToplumsalSonuc != null && p.ToplumsalSonuc.ToLower().Contains(s)) ||
                (p.KurumsalSonuc != null && p.KurumsalSonuc.ToLower().Contains(s)) ||
                (p.SektorelEtki != null && p.SektorelEtki.ToLower().Contains(s)) ||
                (p.BagimliVarlik != null && p.BagimliVarlik.ToLower().Contains(s)) ||
                (p.KurtarmaPlanlari != null && p.KurtarmaPlanlari.ToLower().Contains(s)) ||
                (p.YedeklemeTipi != null && p.YedeklemeTipi.ToLower().Contains(s)) ||
                (p.YedeklemeTuru != null && p.YedeklemeTuru.ToLower().Contains(s)) ||
                (p.YedeklemeSikligi != null && p.YedeklemeSikligi.ToLower().Contains(s)) ||
                (p.YedeklemeAlani != null && p.YedeklemeAlani.ToLower().Contains(s)) ||
                (p.YedektenDonusPlani != null && p.YedektenDonusPlani.ToLower().Contains(s)) ||
                (p.YedeklemeSorumlusu != null && p.YedeklemeSorumlusu.ToLower().Contains(s)) ||
                (p.Kriptoloji != null && p.Kriptoloji.ToLower().Contains(s)) ||
                (p.KriptolojiTuru != null && p.KriptolojiTuru.ToLower().Contains(s)) ||
                (p.KullanilanKriptoloji != null && p.KullanilanKriptoloji.ToLower().Contains(s)) ||
                (p.AnahtarSorumlusu != null && p.AnahtarSorumlusu.ToLower().Contains(s)) ||
                (p.IpAdresi != null && p.IpAdresi.ToLower().Contains(s)) ||
                (p.IsletimSistemi != null && p.IsletimSistemi.ToLower().Contains(s)) ||
                (p.LisansTakipSorumlusu != null && p.LisansTakipSorumlusu.ToLower().Contains(s)) ||
                (p.MarkaModel != null && p.MarkaModel.ToLower().Contains(s)) ||
                (p.SeriNumarasi != null && p.SeriNumarasi.ToLower().Contains(s)) ||
                (p.ZimmetSahibi != null && p.ZimmetSahibi.ToLower().Contains(s)) ||
                (p.Notlar != null && p.Notlar.ToLower().Contains(s)) ||
                (p.YedeklerinSaklamaSuresi != null && p.YedeklerinSaklamaSuresi.ToLower().Contains(s)) ||
                (p.KisiselVeriBarindirma != null && p.KisiselVeriBarindirma.ToLower().Contains(s)) ||
                (p.AnlikMesajlasmaKullanimi != null && p.AnlikMesajlasmaKullanimi.ToLower().Contains(s)) ||
                (p.BulutBilisim != null && p.BulutBilisim.ToLower().Contains(s)) ||
                (p.YeniGelismelerveTedarik != null && p.YeniGelismelerveTedarik.ToLower().Contains(s)) ||
                (p.KritikAltyapiSistemi != null && p.KritikAltyapiSistemi.ToLower().Contains(s))
            );
        }

        return await query.ToListAsync();
    }

    public async Task<DataTablesResponse<ListAgveSistemDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListAgveSistemDto, bool>>? filter = null)
    {
        var query = context.AgveSistemDetay.AsQueryable();

        query = request.FilterBag.ApplyFilters(query);

        if (filter != null)
            query = query.Where(filter);


        return await DataTablesHelper.ProcessAsync(query, request);
    }

    // ---------------------------------------------------------------
    // Rapor metodu (doğrudan entity üzerinden çalışır)
    // ---------------------------------------------------------------

    public async Task<List<RaporAnasayfa>> GetRaporAgveSistemlerAsync()
    {
        var agVeSistemler = context.AgveSistemler
            .OrderByDescending(a => a.EnvantereGirisTarihi)
            .Take(3)
            .Select(a => new RaporAnasayfa
            {
                VarlikSahibi = a.VarlikSahibi,
                Kategori = "Ağ ve Sistemler",
                EklenmeTarihi = a.EnvantereGirisTarihi!.Value
            });

        return await agVeSistemler.ToListAsync();
    }
}