using System.Linq.Expressions;
using Core.Repository;
using Core.Util;
using Dto.DTOs;
using Dto.KriptografiEnvanteri;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class KriptografiEnvanteriRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<KriptografiEnvanteri, int>(context), IKriptografiEnvanteriRepository
{
    // KriptografiEnvanteri'ne özel metodların implementasyonları buraya eklenebilir
    public async Task<List<ListKriptografiEnvanteriDto>> GetListWithDetailsAsync(
        Expression<Func<ListKriptografiEnvanteriDto, bool>>? filter = null)
    {
        var query = context.KriptografiEnvanteriDetay.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<List<ListKriptografiEnvanteriDto>> GetListWithDetailsAsync(
        string search,
        Expression<Func<ListKriptografiEnvanteriDto, bool>>? filter = null)
    {
        var query = context.KriptografiEnvanteriDetay.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            var lowerSearch = search.ToLower();

            query = query.Where(p =>
                (p.VarlikAdi != null && p.VarlikAdi.ToLower().Contains(lowerSearch)) ||
                (p.VarlikSahibi != null && p.VarlikSahibi.ToLower().Contains(lowerSearch)) ||
                (p.VarlikSahibiAltDepartman != null && p.VarlikSahibiAltDepartman.ToLower().Contains(lowerSearch)) ||
                (p.UretimYeri != null && p.UretimYeri.ToLower().Contains(lowerSearch)) ||
                (p.KullanimAmaci != null && p.KullanimAmaci.ToLower().Contains(lowerSearch)) ||
                (p.OlusturmaTarihi != null && p.OlusturmaTarihi.Value.ToString("dd.MM.yyyy").Contains(lowerSearch)) ||
                (p.KullanimSuresi != null && p.KullanimSuresi.ToLower().Contains(lowerSearch)) ||
                (p.AnahtarSorumlusu != null && p.AnahtarSorumlusu.ToString().Contains(lowerSearch)) ||
                (p.AnahtarSaklamaAlani != null && p.AnahtarSaklamaAlani.ToLower().Contains(lowerSearch)) ||
                (p.DestekAlinanTedarikci != null && p.DestekAlinanTedarikci.ToLower().Contains(lowerSearch)) ||
                (p.DonanimYazilim != null && p.DonanimYazilim.ToLower().Contains(lowerSearch)) ||
                (p.Algoritma != null && p.Algoritma.ToLower().Contains(lowerSearch)) ||
                (p.OrtakKriterler != null && p.OrtakKriterler.ToLower().Contains(lowerSearch)) ||
                (p.KullanimSeviyesi != null && p.KullanimSeviyesi.ToLower().Contains(lowerSearch)) ||
                (p.KullanimKabiliyetleri != null && p.KullanimKabiliyetleri.ToLower().Contains(lowerSearch)) ||
                (p.Notlar != null && p.Notlar.ToLower().Contains(lowerSearch)) ||
                (p.CreatedDate != null && p.CreatedDate.Value.ToString("dd.MM.yyyy").Contains(lowerSearch)) ||
                (p.UpdatedDate != null && p.UpdatedDate.Value.ToString("dd.MM.yyyy").Contains(lowerSearch)) ||
                (p.DeletedDate != null && p.DeletedDate.Value.ToString("dd.MM.yyyy").Contains(lowerSearch))
            );
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<DataTablesResponse<ListKriptografiEnvanteriDto>> ProcessTableRequestAsync(
        DataTablesRequest request, Expression<Func<ListKriptografiEnvanteriDto, bool>>? filter = null)
    {
        var query = context.KriptografiEnvanteriDetay.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await DataTablesHelper.ProcessAsync(query, request);
    }
}