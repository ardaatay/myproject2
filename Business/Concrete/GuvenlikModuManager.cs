using Business.Abstract;
using Entity.Concrete;
using Repository.Abstract;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class GuvenlikModuManager(

    IVarlikEnvanteriUnitOfWork unitOfWork) : IGuvenlikModuService
{
    public async Task<bool> UpdateGuvenlikModu(bool durum)
    {
        var repository = unitOfWork.GetRepository<GuvenlikModu, int>();
        var entity = await repository.GetByIdAsync(1);
        if (entity == null) return false;
        entity.Durum = durum;

        await repository.UpdateAsync(entity);

        return entity.Durum;
    }

    public async Task<bool> GetGuvenlikModuDurumu()
    {
        var repository = unitOfWork.GetRepository<GuvenlikModu, int>();
        var entity = await repository.GetByIdAsync(1);

        return entity?.Durum ?? false;
    }
}