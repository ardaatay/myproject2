using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class ToplumsalSonucRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<ToplumsalSonuc, int>(context), IToplumsalSonucRepository;