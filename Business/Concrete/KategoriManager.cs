using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.Kategori;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class KategoriManager : IKategoriService
{
    private readonly IVarlikEnvanteriUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public KategoriManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ListKategoriDto>> GetAllAsync()
    {
        var repository = _unitOfWork.GetRepository<Kategori, int>();
        var entities = await repository.GetAllWithOptionsAsync(null, x => x.OrderBy(k => k.UstId), x =>
        x.Ust!);
        return _mapper.Map<List<ListKategoriDto>>(entities);
    }

    public async Task<List<ListKategoriDto>> GetAllByUstIdAsync(int ustId)
    {
        var entities = await _unitOfWork.GetRepository<Kategori, int>().GetAllAsync(x => x.UstId == ustId);
        return _mapper.Map<List<ListKategoriDto>>(entities);
    }

    public async Task<UpdateKategoriDto> GetByIdAsync(int id)
    {
        var entity = await _unitOfWork.GetRepository<Kategori, int>().GetAsync(x => x.Id == id);
        return _mapper.Map<UpdateKategoriDto>(entity);
    }

    public async Task<CreateKategoriDto> AddAsync(CreateKategoriDto dto)
    {
        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = _mapper.Map<Kategori>(dto);
            var repository = _unitOfWork.GetRepository<Kategori, int>();

            await repository.AddAsync(entity);
            return _mapper.Map<CreateKategoriDto>(entity);
        });
    }

    public async Task<UpdateKategoriDto> UpdateAsync(UpdateKategoriDto dto)
    {
        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = _unitOfWork.GetRepository<Kategori, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"Kategori with id {dto.Id} not found");

            _mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return _mapper.Map<UpdateKategoriDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = _unitOfWork.GetRepository<Kategori, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Kategori with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = _unitOfWork.GetRepository<Kategori, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}