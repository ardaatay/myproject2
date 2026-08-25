using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class GuvenlikModuRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<GuvenlikModu, int>(context), IGuvenlikModuRepository
{
}