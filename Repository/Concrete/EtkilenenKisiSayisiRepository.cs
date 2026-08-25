using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class EtkilenenKisiSayisiRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<EtkilenenKisiSayisi, int>(context), IEtkilenenKisiSayisiRepository; 