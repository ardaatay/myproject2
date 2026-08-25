using System.Linq.Expressions;
using Core.Repository;
using Core.Util;
using Dto.BasiliBilgi;
using Dto.DTOs;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Repository.Abstract;
using Repository.Context;
using Util.Query;

namespace Repository.Concrete;

public class BasiliBilgiRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<BasiliBilgi, int>(context), IBasiliBilgiRepository
{
    // BasiliBilgi'ye özel metodların implementasyonları buraya eklenebilir
    public async Task<List<ListBasiliBilgiDto>> GetListWithDetailsAsync(
        Expression<Func<ListBasiliBilgiDto, bool>>? filter = null)
    {
        var query = context.BasiliBilgiDetay.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<List<ListBasiliBilgiDto>> GetListWithDetailsAsync(
        string search, FilterBag filterBag,
        Expression<Func<ListBasiliBilgiDto, bool>>? filter = null)
    {
        var query = context.BasiliBilgiDetay.AsQueryable();

        query = filterBag.ApplyFilters(query);

        if (!string.IsNullOrEmpty(search))
        {
            var lowerSearch = search.ToLower();

            query = query.Where(p =>
                (p.Kategori != null && p.Kategori.ToLower().Contains(lowerSearch)) ||
                (p.AltKategori != null && p.AltKategori.ToLower().Contains(lowerSearch)) ||
                (p.VarlikAdi != null && p.VarlikAdi.ToLower().Contains(lowerSearch)) ||
                (p.KullanimAmaci != null && p.KullanimAmaci.ToLower().Contains(lowerSearch)) ||
                (p.Miktar != null && p.Miktar.ToString()!.Contains(lowerSearch)) ||
                (p.Durum != null && p.Durum.ToLower().Contains(lowerSearch)) ||
                (p.Konum != null && p.Konum.ToLower().Contains(lowerSearch)) ||
                (p.VarlikSahibi != null && p.VarlikSahibi.ToLower().Contains(lowerSearch)) ||
                (p.VarlikSahibiAltDepartman != null && p.VarlikSahibiAltDepartman.ToLower().Contains(lowerSearch)) ||
                (p.OperasyonelSahibi != null && p.OperasyonelSahibi.ToLower().Contains(lowerSearch)) ||
                (p.BilgiSinifi != null && p.BilgiSinifi.ToLower().Contains(lowerSearch)) ||
                (p.Gizlilik != null && p.Gizlilik.ToLower().Contains(lowerSearch)) ||
                (p.Butunluk != null && p.Butunluk.ToLower().Contains(lowerSearch)) ||
                (p.Erisilebilirlik != null && p.Erisilebilirlik.ToLower().Contains(lowerSearch)) ||
                (p.EtkilenenKisiSayisi != null && p.EtkilenenKisiSayisi.ToLower().Contains(lowerSearch)) ||
                (p.ToplumsalSonuc != null && p.ToplumsalSonuc.ToLower().Contains(lowerSearch)) ||
                (p.KurumsalSonuc != null && p.KurumsalSonuc.ToLower().Contains(lowerSearch)) ||
                (p.SektorelEtki != null && p.SektorelEtki.ToLower().Contains(lowerSearch)) ||
                (p.BagimliVarlik != null && p.BagimliVarlik.ToLower().Contains(lowerSearch)) ||
                (p.Rpo != null && p.Rpo.ToString().Contains(lowerSearch)) ||
                (p.Rto != null && p.Rto.ToString().Contains(lowerSearch)) ||
                (p.Mtpd != null && p.Mtpd.ToString().Contains(lowerSearch)) ||
                (p.KurtarmaPlanlari != null && p.KurtarmaPlanlari.ToLower().Contains(lowerSearch)) ||
                (p.KisiselVeriBarindirma != null && p.KisiselVeriBarindirma.ToLower().Contains(lowerSearch)) ||
                (p.SaklamaSuresi != null && p.SaklamaSuresi.ToLower().Contains(lowerSearch)) ||
                (p.Notlar != null && p.Notlar.ToLower().Contains(lowerSearch)) ||
                (p.EnvantereGirisTarihi != null && p.EnvantereGirisTarihi.ToString()!.Contains(lowerSearch)) ||
                (p.EnvanterGuncellemeTarihi != null && p.EnvanterGuncellemeTarihi.ToString()!.Contains(lowerSearch)) ||
                (p.EnvanterdenCikisTarihi != null && p.EnvanterdenCikisTarihi.ToString()!.Contains(lowerSearch))
            );
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<DataTablesResponse<ListBasiliBilgiDto>> ProcessTableRequestAsync(DataTablesRequest request,
        Expression<Func<ListBasiliBilgiDto, bool>>? filter = null)
    {
        var query = context.BasiliBilgiDetay.AsQueryable();

        query = request.FilterBag.ApplyFilters(query);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await DataTablesHelper.ProcessAsync(query, request);
    }
}