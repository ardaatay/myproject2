using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.ToplumsalSonuc;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class ToplumsalSonucManager : IToplumsalSonucService
{
    private readonly IVarlikEnvanteriUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ToplumsalSonucManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ListToplumsalSonucDto>> GetAllAsync()
    {
        var entities = await _unitOfWork.GetRepository<ToplumsalSonuc, int>().GetAllAsync();
        return _mapper.Map<List<ListToplumsalSonucDto>>(entities);
    }

    public async Task<UpdateToplumsalSonucDto> GetByIdAsync(int id)
    {
        var entity = await _unitOfWork.GetRepository<ToplumsalSonuc, int>().GetAsync(x => x.Id == id);
        return _mapper.Map<UpdateToplumsalSonucDto>(entity);
    }

    public async Task<CreateToplumsalSonucDto> AddAsync(CreateToplumsalSonucDto dto)
    {
        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = _mapper.Map<ToplumsalSonuc>(dto);
            var repository = _unitOfWork.GetRepository<ToplumsalSonuc, int>();

            await repository.AddAsync(entity);
            return _mapper.Map<CreateToplumsalSonucDto>(entity);
        });
    }

    public async Task<UpdateToplumsalSonucDto> UpdateAsync(UpdateToplumsalSonucDto dto)
    {
        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = _unitOfWork.GetRepository<ToplumsalSonuc, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"ToplumsalSonuc with id {dto.Id} not found");

            _mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return _mapper.Map<UpdateToplumsalSonucDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = _unitOfWork.GetRepository<ToplumsalSonuc, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"ToplumsalSonuc with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = _unitOfWork.GetRepository<ToplumsalSonuc, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}