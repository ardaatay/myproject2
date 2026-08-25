using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class BilgiSinifiRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<BilgiSinifi, int>(context), IBilgiSinifiRepository
{
    // BilgiSinifi'ne özel metodların implementasyonları buraya eklenebilir
} 