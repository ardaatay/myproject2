using System.Globalization;
using System.Linq.Expressions;
using Core.Repository;
using Core.Util;
using Dto.DTOs;
using Dto.FizikselMekan;
using Dto.Rapor;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Repository.Abstract;
using Repository.Context;
using Util.Query;

namespace Repository.Concrete;

public class FizikselMekanRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<FizikselMekan, int>(context), IFizikselMekanRepository
{
    public async Task<List<ListFizikselMekanDto>> GetListWithDetailsAsync(
        Expression<Func<ListFizikselMekanDto, bool>>? filter = null)
    {
        var query = context.FizikselMekanDetay.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<List<ListFizikselMekanDto>> GetListWithDetailsAsync(
        string search, FilterBag filterBag,
        Expression<Func<ListFizikselMekanDto, bool>>? filter = null)
    {
        var query = context.FizikselMekanDetay.AsQueryable();
        
        query = filterBag.ApplyFilters(query);

        if (!string.IsNullOrEmpty(search))
        {
            var s = search.ToLower();

            query = query.Where(p =>
                (p.Kategori != null && p.Kategori.ToLower().Contains(s)) ||
                (p.AltKategori != null && p.AltKategori.ToLower().Contains(s)) ||
                (p.VarlikAdi != null && p.VarlikAdi.ToLower().Contains(s)) ||
                (p.KullanimAmaci != null && p.KullanimAmaci.ToLower().Contains(s)) ||
                (p.Miktar != null && p.Miktar.ToString()!.Contains(s)) ||
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
                (p.Rpo != null && p.Rpo.ToLower().Contains(s)) ||
                (p.Rto != null && p.Rto.ToLower().Contains(s)) ||
                (p.Mtpd != null && p.Mtpd.ToLower().Contains(s)) ||
                (p.KurtarmaPlanlari != null && p.KurtarmaPlanlari.ToLower().Contains(s)) ||
                (p.KisiselVeriBarindirma != null && p.KisiselVeriBarindirma.ToLower().Contains(s)) ||
                (p.BasiliBilgi != null && p.BasiliBilgi.ToLower().Contains(s)) ||
                (p.Notlar != null && p.Notlar.ToLower().Contains(s))
            );
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<DataTablesResponse<ListFizikselMekanDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListFizikselMekanDto, bool>>? filter = null)
    {
        var query = context.FizikselMekanDetay.AsQueryable();

        query = request.FilterBag.ApplyFilters(query);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await DataTablesHelper.ProcessAsync(query, request);
    }

    public async Task<List<RaporAnasayfa>> GetRaporFizikselMekanlarAsync()
    {
        var fizikselMekanlar = context.FizikselMekanlar.OrderByDescending(a => a.EnvantereGirisTarihi).Take(3)
            .Select(a => new RaporAnasayfa
            {
                VarlikSahibi = a.VarlikSahibi,
                Kategori = "Fiziksel Mekanlar",
                EklenmeTarihi = a.EnvantereGirisTarihi.GetValueOrDefault()
            });

        return await fizikselMekanlar.ToListAsync();
    }
}
