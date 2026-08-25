using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class DurumRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<Durum, int>(context), IDurumRepository
{
    // Durum'a özel metodların implementasyonları buraya eklenebilir
} 