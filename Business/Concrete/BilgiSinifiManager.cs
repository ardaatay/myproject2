using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.BilgiSinifi;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class BilgiSinifiManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper) : IBilgiSinifiService
{
    public async Task<List<ListBilgiSinifiDto>> GetAllAsync()
    {
        var entities = await unitOfWork.GetRepository<BilgiSinifi, int>().GetAllAsync();
        return mapper.Map<List<ListBilgiSinifiDto>>(entities);
    }

    public async Task<UpdateBilgiSinifiDto> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.GetRepository<BilgiSinifi, int>().GetAsync(x => x.Id == id);
        return mapper.Map<UpdateBilgiSinifiDto>(entity);
    }

    public async Task<CreateBilgiSinifiDto> AddAsync(CreateBilgiSinifiDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<BilgiSinifi>(dto);
            var repository = unitOfWork.GetRepository<BilgiSinifi, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateBilgiSinifiDto>(entity);
        });
    }

    public async Task<UpdateBilgiSinifiDto> UpdateAsync(UpdateBilgiSinifiDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<BilgiSinifi, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"BilgiSinifi with id {dto.Id} not found");

            mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateBilgiSinifiDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<BilgiSinifi, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"BilgiSinifi with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<BilgiSinifi, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}