using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class DestekDurumuRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<DestekDurumu, int>(context), IDestekDurumuRepository
{
    // DestekDurumu'na özel metodların implementasyonları buraya eklenebilir
} 