using System.Globalization;
using System.Linq.Expressions;
using Core.Repository;
using Core.Util;
using Dto.DTOs;
using Dto.Personel;
using Dto.Rapor;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Repository.Abstract;
using Repository.Context;
using Util.Query;

namespace Repository.Concrete;

public class PersonelRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<Personel, int>(context), IPersonelRepository
{
    public async Task<List<ListPersonelDto>> GetListWithDetailsAsync(
        Expression<Func<ListPersonelDto, bool>>? filter = null)
    {
        var query = context.PersonelDetay.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<List<ListPersonelDto>> GetListWithDetailsAsync(
        string search, FilterBag filterBag,
        Expression<Func<ListPersonelDto, bool>>? filter = null)
    {
        var query = context.PersonelDetay.AsQueryable();

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
                (p.Mtpd != null && p.Mtpd.ToLower().Contains(s)) ||
                (p.KurtarmaPlanlari != null && p.KurtarmaPlanlari.ToLower().Contains(s)) ||
                (p.VekaletEdilmeDurumu != null && p.VekaletEdilmeDurumu.ToLower().Contains(s)) ||
                (p.Notlar != null && p.Notlar.ToLower().Contains(s))
            );
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<DataTablesResponse<ListPersonelDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListPersonelDto, bool>>? filter = null)
    {
        var query = context.PersonelDetay.AsQueryable();

        query = request.FilterBag.ApplyFilters(query);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await DataTablesHelper.ProcessAsync(query, request);
    }


    public async Task<List<RaporAnasayfa>> GetRaporPersonelAsync()
    {
        var personeller = context.Personeller.OrderByDescending(a => a.EnvantereGirisTarihi).Take(3)
            .Select(a => new RaporAnasayfa
            {
                VarlikSahibi = a.VarlikSahibi,
                Kategori = "Personel",
                EklenmeTarihi = a.EnvantereGirisTarihi!.Value
            });

        return await personeller.ToListAsync();
    }
}
