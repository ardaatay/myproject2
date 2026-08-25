using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.AnahtarSorumlusu;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class AnahtarSorumlusuManager : IAnahtarSorumlusuService
{
    private readonly IVarlikEnvanteriUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AnahtarSorumlusuManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ListAnahtarSorumlusuDto>> GetAllAsync()
    {
        var entities = await _unitOfWork.GetRepository<AnahtarSorumlusu, int>().GetAllAsync();
        return _mapper.Map<List<ListAnahtarSorumlusuDto>>(entities);
    }

    public async Task<UpdateAnahtarSorumlusuDto> GetByIdAsync(int id)
    {
        var entity = await _unitOfWork.GetRepository<AnahtarSorumlusu, int>().GetAsync(x => x.Id == id);
        return _mapper.Map<UpdateAnahtarSorumlusuDto>(entity);
    }

    public async Task<CreateAnahtarSorumlusuDto> AddAsync(CreateAnahtarSorumlusuDto dto)
    {
        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = _mapper.Map<AnahtarSorumlusu>(dto);
            var repository = _unitOfWork.GetRepository<AnahtarSorumlusu, int>();

            await repository.AddAsync(entity);
            return _mapper.Map<CreateAnahtarSorumlusuDto>(entity);
        });
    }

    public async Task<UpdateAnahtarSorumlusuDto> UpdateAsync(UpdateAnahtarSorumlusuDto dto)
    {
        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = _unitOfWork.GetRepository<AnahtarSorumlusu, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"AnahtarSorumlusu with id {dto.Id} not found");

            _mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return _mapper.Map<UpdateAnahtarSorumlusuDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = _unitOfWork.GetRepository<AnahtarSorumlusu, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"AnahtarSorumlusu with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = _unitOfWork.GetRepository<AnahtarSorumlusu, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}