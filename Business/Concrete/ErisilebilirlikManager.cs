using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.Erisilebilirlik;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class ErisilebilirlikManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper) : IErisilebilirlikService
{
    public async Task<List<ListErisilebilirlikDto>> GetAllAsync()
    {
        var entities = await unitOfWork.GetRepository<Erisilebilirlik, int>().GetAllAsync();
        return mapper.Map<List<ListErisilebilirlikDto>>(entities);
    }

    public async Task<UpdateErisilebilirlikDto> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.GetRepository<Erisilebilirlik, int>().GetAsync(x => x.Id == id);
        return mapper.Map<UpdateErisilebilirlikDto>(entity);
    }

    public async Task<CreateErisilebilirlikDto> AddAsync(CreateErisilebilirlikDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<Erisilebilirlik>(dto);
            var repository = unitOfWork.GetRepository<Erisilebilirlik, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateErisilebilirlikDto>(entity);
        });
    }

    public async Task<UpdateErisilebilirlikDto> UpdateAsync(UpdateErisilebilirlikDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Erisilebilirlik, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"Erisilebilirlik with id {dto.Id} not found");

            mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateErisilebilirlikDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Erisilebilirlik, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Erisilebilirlik with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<Erisilebilirlik, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}