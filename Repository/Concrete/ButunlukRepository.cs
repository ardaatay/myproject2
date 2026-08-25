using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class ButunlukRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<Butunluk, int>(context), IButunlukRepository
{
    // Butunluk'a özel metodların implementasyonları buraya eklenebilir
} 