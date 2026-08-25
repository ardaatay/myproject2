using Core.Repository;
using Entity.Concrete;

namespace Repository.Abstract;

public interface ILogRepository
{
    void AddLog(Log entity);
}