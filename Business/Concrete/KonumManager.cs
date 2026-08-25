using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.Konum;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class KonumManager : IKonumService
{
    private readonly IVarlikEnvanteriUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public KonumManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ListKonumDto>> GetAllAsync()
    {
        var entities = await _unitOfWork.GetRepository<Konum, int>().GetAllAsync();
        return _mapper.Map<List<ListKonumDto>>(entities);
    }

    public async Task<UpdateKonumDto> GetByIdAsync(int id)
    {
        var entity = await _unitOfWork.GetRepository<Konum, int>().GetAsync(x => x.Id == id);
        return _mapper.Map<UpdateKonumDto>(entity);
    }

    public async Task<CreateKonumDto> AddAsync(CreateKonumDto dto)
    {
        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = _mapper.Map<Konum>(dto);
            var repository = _unitOfWork.GetRepository<Konum, int>();

            await repository.AddAsync(entity);
            return _mapper.Map<CreateKonumDto>(entity);
        });
    }

    public async Task<UpdateKonumDto> UpdateAsync(UpdateKonumDto dto)
    {
        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = _unitOfWork.GetRepository<Konum, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"Konum with id {dto.Id} not found");

            _mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return _mapper.Map<UpdateKonumDto>(entity);
        });
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = _unitOfWork.GetRepository<Konum, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Konum with id {id} not found");

            await repository.DeleteAsync(entity);
        });

        return true;
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = _unitOfWork.GetRepository<Konum, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}