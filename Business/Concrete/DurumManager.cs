using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.Durum;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class DurumManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper) : IDurumService
{
    public async Task<List<ListDurumDto>> GetAllAsync()
    {
        var entities = await unitOfWork.GetRepository<Durum, int>().GetAllAsync();
        return mapper.Map<List<ListDurumDto>>(entities);
    }

    public async Task<UpdateDurumDto> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.GetRepository<Durum, int>().GetAsync(x => x.Id == id);
        return mapper.Map<UpdateDurumDto>(entity);
    }

    public async Task<CreateDurumDto> AddAsync(CreateDurumDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<Durum>(dto);
            var repository = unitOfWork.GetRepository<Durum, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateDurumDto>(entity);
        });
    }

    public async Task<UpdateDurumDto> UpdateAsync(UpdateDurumDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Durum, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"Durum with id {dto.Id} not found");

            mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateDurumDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Durum, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Durum with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<Durum, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}