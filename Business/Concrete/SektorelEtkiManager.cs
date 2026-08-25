using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.SektorelEtki;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class SektorelEtkiManager : ISektorelEtkiService
{
    private readonly IVarlikEnvanteriUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SektorelEtkiManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ListSektorelEtkiDto>> GetAllAsync()
    {
        var entities = await _unitOfWork.GetRepository<SektorelEtki, int>().GetAllAsync();
        return _mapper.Map<List<ListSektorelEtkiDto>>(entities);
    }

    public async Task<UpdateSektorelEtkiDto> GetByIdAsync(int id)
    {
        var entity = await _unitOfWork.GetRepository<SektorelEtki, int>().GetAsync(x => x.Id == id);
        return _mapper.Map<UpdateSektorelEtkiDto>(entity);
    }

    public async Task<CreateSektorelEtkiDto> AddAsync(CreateSektorelEtkiDto dto)
    {
        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = _mapper.Map<SektorelEtki>(dto);
            var repository = _unitOfWork.GetRepository<SektorelEtki, int>();

            await repository.AddAsync(entity);
            return _mapper.Map<CreateSektorelEtkiDto>(entity);
        });
    }

    public async Task<UpdateSektorelEtkiDto> UpdateAsync(UpdateSektorelEtkiDto dto)
    {
        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = _unitOfWork.GetRepository<SektorelEtki, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"SektorelEtki with id {dto.Id} not found");

            _mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return _mapper.Map<UpdateSektorelEtkiDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = _unitOfWork.GetRepository<SektorelEtki, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"SektorelEtki with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = _unitOfWork.GetRepository<SektorelEtki, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}