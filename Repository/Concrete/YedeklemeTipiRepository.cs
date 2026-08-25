using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class YedeklemeTipiRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<YedeklemeTipi, int>(context), IYedeklemeTipiRepository;