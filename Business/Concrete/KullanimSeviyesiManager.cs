using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.KullanimSeviyesi;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class KullanimSeviyesiManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper) : IKullanimSeviyesiService
{
    public async Task<List<ListKullanimSeviyesiDto>> GetAllAsync()
    {
        var entities = await unitOfWork.GetRepository<KullanimSeviyesi, int>().GetAllAsync();
        return mapper.Map<List<ListKullanimSeviyesiDto>>(entities);
    }

    public async Task<UpdateKullanimSeviyesiDto> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.GetRepository<KullanimSeviyesi, int>().GetAsync(x => x.Id == id);
        return mapper.Map<UpdateKullanimSeviyesiDto>(entity);
    }

    public async Task<CreateKullanimSeviyesiDto> AddAsync(CreateKullanimSeviyesiDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<KullanimSeviyesi>(dto);
            var repository = unitOfWork.GetRepository<KullanimSeviyesi, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateKullanimSeviyesiDto>(entity);
        });
    }

    public async Task<UpdateKullanimSeviyesiDto> UpdateAsync(UpdateKullanimSeviyesiDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<KullanimSeviyesi, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"KullanimSeviyesi with id {dto.Id} not found");

            mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateKullanimSeviyesiDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<KullanimSeviyesi, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"KullanimSeviyesi with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<KullanimSeviyesi, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}