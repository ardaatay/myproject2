using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.YedeklemeTipi;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class YedeklemeTipiManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper) : IYedeklemeTipiService
{
    public async Task<List<ListYedeklemeTipiDto>> GetAllAsync()
    {
        var entities = await unitOfWork.GetRepository<YedeklemeTipi, int>().GetAllAsync();
        return mapper.Map<List<ListYedeklemeTipiDto>>(entities);
    }

    public async Task<UpdateYedeklemeTipiDto> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.GetRepository<YedeklemeTipi, int>().GetAsync(x => x.Id == id);
        return mapper.Map<UpdateYedeklemeTipiDto>(entity);
    }

    public async Task<CreateYedeklemeTipiDto> AddAsync(CreateYedeklemeTipiDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<YedeklemeTipi>(dto);
            var repository = unitOfWork.GetRepository<YedeklemeTipi, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateYedeklemeTipiDto>(entity);
        });
    }

    public async Task<UpdateYedeklemeTipiDto> UpdateAsync(UpdateYedeklemeTipiDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<YedeklemeTipi, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"YedeklemeTipi with id {dto.Id} not found");

            mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateYedeklemeTipiDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<YedeklemeTipi, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"YedeklemeTipi with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<YedeklemeTipi, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}