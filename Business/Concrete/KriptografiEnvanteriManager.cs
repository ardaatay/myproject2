using AutoMapper;
using Business.Abstract;
using Core.Aspects;
using Core.Exceptions;
using Dto.DTOs;
using Dto.KriptografiEnvanteri;
using Entity.Concrete;
using Microsoft.AspNetCore.Http;
using Repository.Abstract;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class KriptografiEnvanteriManager(
    IVarlikEnvanteriUnitOfWork unitOfWork,
    IKriptografiEnvanteriRepository kriptografiEnvanteriRepository,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor)
    : IKriptografiEnvanteriService
{
    public async Task<DataTablesResponse<ListKriptografiEnvanteriDto>> GetAllAsync(DataTablesRequest request)
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await kriptografiEnvanteriRepository.ProcessTableRequestAsync(request);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await kriptografiEnvanteriRepository.ProcessTableRequestAsync(request,
                x => x.VarlikSahibiId == Convert.ToInt32(birimId));
        }
    }

    public async Task<List<ListKriptografiEnvanteriDto>> GetAllExcelAsync()
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await kriptografiEnvanteriRepository.GetListWithDetailsAsync();
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await kriptografiEnvanteriRepository.GetListWithDetailsAsync(x =>
                (x.VarlikSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<List<ListKriptografiEnvanteriDto>> GetAllExcelAsync(string search)
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await kriptografiEnvanteriRepository.GetListWithDetailsAsync(search);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await kriptografiEnvanteriRepository.GetListWithDetailsAsync(
                search,
                x =>
                    (x.VarlikSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<UpdateKriptografiEnvanteriDto> GetByIdAsync(int id)
    {
        var repository = unitOfWork.GetRepository<KriptografiEnvanteri, int>();

        var user = httpContextAccessor.HttpContext.User;

        KriptografiEnvanteri? entity;

        if (user.IsInRole("ADMIN"))
        {
            entity = await repository.GetAsync(x => x.Id == id);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            entity = await repository.GetAsync(x => x.Id == id && x.VarlikSahibiId == Convert.ToInt32(birimId));
        }

        if (entity == null)
            throw new NotFoundException($"{id} numaralı Kriptografi Envanteri bulunamadı.");

        return mapper.Map<UpdateKriptografiEnvanteriDto>(entity);
    }

    [LogAspect]
    public async Task<CreateKriptografiEnvanteriDto> AddAsync(CreateKriptografiEnvanteriDto dto)
    {
        var repository = unitOfWork.GetRepository<KriptografiEnvanteri, int>();
        var entity = mapper.Map<KriptografiEnvanteri>(dto);

        var user = httpContextAccessor.HttpContext.User;
        //var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;
        //var birimAdi = user.Claims.FirstOrDefault(c => c.Type == "BirimAdi")?.Value!;
        
        entity.CreatedDate = DateTime.Now;
        entity.Aktif = true;

        await repository.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();
        return mapper.Map<CreateKriptografiEnvanteriDto>(entity);
    }

    [LogAspect]
    public async Task<UpdateKriptografiEnvanteriDto> UpdateAsync(UpdateKriptografiEnvanteriDto dto)
    {
        var repository = unitOfWork.GetRepository<KriptografiEnvanteri, int>();
        var entity = await repository.GetByIdAsync(dto.Id);

        if (entity == null)
            throw new NotFoundException($"{dto.Id} numaralı Kriptografi Envanteri bulunamadı.");

        mapper.Map(dto, entity);

        entity.UpdatedDate = DateTime.Now;
        entity.DeletedDate = null;
        entity.Aktif = true;

        await repository.UpdateAsync(entity);
        await unitOfWork.SaveChangesAsync();
        return mapper.Map<UpdateKriptografiEnvanteriDto>(entity);
    }

    [LogAspect]
    public async Task DeleteAsync(int id)
    {
        var repository = unitOfWork.GetRepository<KriptografiEnvanteri, int>();
        var entity = await repository.GetByIdAsync(id);

        if (entity == null)
            throw new NotFoundException($"{id} numaralı Kriptografi Envanteri bulunamadı.");

        entity.Aktif = false;
        entity.DeletedDate = DateTime.Now;

        await repository.UpdateAsync(entity);
        await unitOfWork.SaveChangesAsync();
    }

    [LogAspect]
    public async Task DeleteDatabaseAsync(int id)
    {
        var repository = unitOfWork.GetRepository<KriptografiEnvanteri, int>();
        var entity = await repository.GetByIdAsync(id);

        if (entity == null)
            throw new NotFoundException($"{id} numaralı Kriptografi Envanteri bulunamadı.");

        entity.SilinsinMi = true;
        entity.DeletedDate = DateTime.Now;

        await repository.UpdateAsync(entity);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<KriptografiEnvanteri, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}