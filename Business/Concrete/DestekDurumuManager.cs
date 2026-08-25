using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.DestekDurumu;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class DestekDurumuManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper) : IDestekDurumuService
{
    public async Task<List<ListDestekDurumuDto>> GetAllAsync()
    {
        var entities = await unitOfWork.GetRepository<DestekDurumu, int>().GetAllAsync();
        return mapper.Map<List<ListDestekDurumuDto>>(entities);
    }

    public async Task<UpdateDestekDurumuDto> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.GetRepository<DestekDurumu, int>().GetAsync(x => x.Id == id);
        return mapper.Map<UpdateDestekDurumuDto>(entity);
    }

    public async Task<CreateDestekDurumuDto> AddAsync(CreateDestekDurumuDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<DestekDurumu>(dto);
            var repository = unitOfWork.GetRepository<DestekDurumu, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateDestekDurumuDto>(entity);
        });
    }

    public async Task<UpdateDestekDurumuDto> UpdateAsync(UpdateDestekDurumuDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<DestekDurumu, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"DestekDurumu with id {dto.Id} not found");

            mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateDestekDurumuDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<DestekDurumu, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"DestekDurumu with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<DestekDurumu, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}