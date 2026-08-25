using Core.UnitOfWork;
using Repository.Context;

namespace Repository.UnitOfWork;

public interface IVarlikEnvanteriUnitOfWork : IUnitOfWork
{
    VarlikEnvanteriDbContext Context { get; }
} 