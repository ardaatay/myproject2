using System.Linq.Expressions;
using Core.Repository;
using Core.Util;
using Dto.DTOs;
using Dto.Veritabani;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Repository.Abstract;
using Repository.Context;
using Util.Query;

namespace Repository.Concrete;

public class VeritabaniRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<Veritabani, int>(context), IVeritabaniRepository
{
    // Veritabani'na özel metodların implementasyonları buraya eklenebilir
    public async Task<List<ListVeritabaniDto>> GetListWithDetailsAsync(
        Expression<Func<ListVeritabaniDto, bool>>? filter = null)
    {
        var query = context.VeritabaniDetay.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<List<ListVeritabaniDto>> GetListWithDetailsAsync(
        string search, FilterBag filterBag,
        Expression<Func<ListVeritabaniDto, bool>>? filter = null)
    {
        var query = context.VeritabaniDetay.AsQueryable();

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
                (p.YedeklemeTipi != null && p.YedeklemeTipi.ToLower().Contains(lowerSearch)) ||
                (p.YedeklemeTuru != null && p.YedeklemeTuru.ToLower().Contains(lowerSearch)) ||
                (p.YedeklemeSikligi != null && p.YedeklemeSikligi.ToLower().Contains(lowerSearch)) ||
                (p.YedeklerinSaklamaSuresi != null && p.YedeklerinSaklamaSuresi.ToString().Contains(lowerSearch)) ||
                (p.YedeklemeAlani != null && p.YedeklemeAlani.ToLower().Contains(lowerSearch)) ||
                (p.YedektenDonusPlani != null && p.YedektenDonusPlani.ToLower().Contains(lowerSearch)) ||
                (p.YedeklemeSorumlusu != null && p.YedeklemeSorumlusu.ToLower().Contains(lowerSearch)) ||
                (p.Kriptoloji != null && p.Kriptoloji.ToLower().Contains(lowerSearch)) ||
                (p.KriptolojiTuru != null && p.KriptolojiTuru.ToLower().Contains(lowerSearch)) ||
                (p.KullanilanKriptoloji != null && p.KullanilanKriptoloji.ToLower().Contains(lowerSearch)) ||
                (p.AnahtarSorumlusu != null && p.AnahtarSorumlusu.ToLower().Contains(lowerSearch)) ||
                (p.KisiselVeriBarindirma != null && p.KisiselVeriBarindirma.ToLower().Contains(lowerSearch)) ||
                (p.BulutBilisim != null && p.BulutBilisim.ToLower().Contains(lowerSearch)) ||
                (p.YeniGelismelerveTedarik != null && p.YeniGelismelerveTedarik.ToLower().Contains(lowerSearch)) ||
                (p.KritikAltyapiSistemi != null && p.KritikAltyapiSistemi.ToLower().Contains(lowerSearch)) ||
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

    public async Task<DataTablesResponse<ListVeritabaniDto>> ProcessTableRequestAsync(DataTablesRequest request,
        Expression<Func<ListVeritabaniDto, bool>>? filter = null)
    {
        var query = context.VeritabaniDetay.AsQueryable();

        query = request.FilterBag.ApplyFilters(query);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await DataTablesHelper.ProcessAsync(query, request);
    }
}