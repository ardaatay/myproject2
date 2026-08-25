using Core.Repository;
using Dto.Kullanici;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class RolRepository(VarlikEnvanteriDbContext context) : GenericRepository<Rol, int>(context), IRolRepository
{
    public async Task RolleriKaydetAsync(KullaniciRolAtamaDto model)
    {
        // Önce kullanıcının mevcut rollerini sil
        var mevcutRoller = await context.KullaniciRoller
            .Where(kr => kr.KullaniciId == model.KullaniciId)
            .ToListAsync();

        context.KullaniciRoller.RemoveRange(mevcutRoller);

        // Seçilen rolleri ekle
        foreach (var rol in (model.Roller ?? Enumerable.Empty<RolSecimDto>()).Where(r => r.Secildi))
        {
            await context.KullaniciRoller.AddAsync(new KullaniciRol
            {
                KullaniciId = model.KullaniciId,
                RolId = rol.RolId,
                Durum = true
            });
        }

        await context.SaveChangesAsync();
    }
}