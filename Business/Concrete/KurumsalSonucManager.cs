using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.KurumsalSonuc;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class KurumsalSonucManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper) : IKurumsalSonucService
{
    public async Task<List<ListKurumsalSonucDto>> GetAllAsync()
    {
        var entities = await unitOfWork.GetRepository<KurumsalSonuc, int>().GetAllAsync();
        return mapper.Map<List<ListKurumsalSonucDto>>(entities);
    }

    public async Task<UpdateKurumsalSonucDto> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.GetRepository<KurumsalSonuc, int>().GetAsync(x => x.Id == id);
        return mapper.Map<UpdateKurumsalSonucDto>(entity);
    }

    public async Task<CreateKurumsalSonucDto> AddAsync(CreateKurumsalSonucDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<KurumsalSonuc>(dto);
            var repository = unitOfWork.GetRepository<KurumsalSonuc, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateKurumsalSonucDto>(entity);
        });
    }

    public async Task<UpdateKurumsalSonucDto> UpdateAsync(UpdateKurumsalSonucDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<KurumsalSonuc, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"KurumsalSonuc with id {dto.Id} not found");

            mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateKurumsalSonucDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<KurumsalSonuc, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"KurumsalSonuc with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<KurumsalSonuc, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}