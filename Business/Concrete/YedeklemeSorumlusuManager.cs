using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.YedeklemeSorumlusu;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class YedeklemeSorumlusuManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper)
    : IYedeklemeSorumlusuService
{
    public async Task<List<ListYedeklemeSorumlusuDto>> GetAllAsync()
    {
        var entities = await unitOfWork.GetRepository<YedeklemeSorumlusu, int>().GetAllAsync();
        return mapper.Map<List<ListYedeklemeSorumlusuDto>>(entities);
    }

    public async Task<UpdateYedeklemeSorumlusuDto> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.GetRepository<YedeklemeSorumlusu, int>().GetAsync(x => x.Id == id);
        return mapper.Map<UpdateYedeklemeSorumlusuDto>(entity);
    }

    public async Task<CreateYedeklemeSorumlusuDto> AddAsync(CreateYedeklemeSorumlusuDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<YedeklemeSorumlusu>(dto);
            var repository = unitOfWork.GetRepository<YedeklemeSorumlusu, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateYedeklemeSorumlusuDto>(entity);
        });
    }

    public async Task<UpdateYedeklemeSorumlusuDto> UpdateAsync(UpdateYedeklemeSorumlusuDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<YedeklemeSorumlusu, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"YedeklemeSorumlusu with id {dto.Id} not found");

            mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateYedeklemeSorumlusuDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<YedeklemeSorumlusu, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"YedeklemeSorumlusu with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<YedeklemeSorumlusu, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}