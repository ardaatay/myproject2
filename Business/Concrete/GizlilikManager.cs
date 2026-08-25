using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.Gizlilik;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class GizlilikManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper) : IGizlilikService
{
    public async Task<List<ListGizlilikDto>> GetAllAsync()
    {
        var entities = await unitOfWork.GetRepository<Gizlilik, int>().GetAllAsync();
        return mapper.Map<List<ListGizlilikDto>>(entities);
    }

    public async Task<UpdateGizlilikDto> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.GetRepository<Gizlilik, int>().GetAsync(x => x.Id == id);
        return mapper.Map<UpdateGizlilikDto>(entity);
    }

    public async Task<CreateGizlilikDto> AddAsync(CreateGizlilikDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<Gizlilik>(dto);
            var repository = unitOfWork.GetRepository<Gizlilik, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateGizlilikDto>(entity);
        });
    }

    public async Task<UpdateGizlilikDto> UpdateAsync(UpdateGizlilikDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Gizlilik, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"Gizlilik with id {dto.Id} not found");

            mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateGizlilikDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Gizlilik, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Gizlilik with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<Gizlilik, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}