using Core.Repository;
using Dto.Kullanici;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class KullaniciRolRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<KullaniciRol, int>(context), IKullaniciRolRepository
{
    public async Task<KullaniciRolAtamaDto?> KullaniciRolleriniGetirAsync(int kullaniciId)
    {
        var kullanici = await context.Kullanicilar
            .Where(k => k.Id == kullaniciId)
            .FirstOrDefaultAsync();

        if (kullanici == null)
            return null;

        var kullaniciRolleri = await context.KullaniciRoller
            .Where(kr => kr.KullaniciId == kullaniciId)
            .Select(kr => kr.RolId)
            .ToListAsync();

        var tumRoller = await context.Roller
            .Select(r => new RolSecimDto()
            {
                RolId = r.Id,
                RolAdi = r.Ad,
                Secildi = kullaniciRolleri.Contains(r.Id)
            })
            .ToListAsync();

        return new KullaniciRolAtamaDto()
        {
            KullaniciId = kullanici.Id,
            KullaniciAdi = kullanici.Username,
            Roller = tumRoller
        };
    }
}