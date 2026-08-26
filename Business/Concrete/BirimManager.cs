using AutoMapper;
using Business.Abstract;
using Core.Aspects;
using Core.Exceptions;
using Dto.Birim;
using Dto.DTOs;
using Entity.Concrete;
using Repository.Abstract;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class BirimManager(
    IVarlikEnvanteriUnitOfWork unitOfWork,
    IBirimRepository birimRepository,
    IMapper mapper) : IBirimService
{
    public async Task<DataTablesResponse<ListBirimDto>> GetAllAsync(DataTablesRequest request)
    {
        return await birimRepository.ProcessTableRequestAsync(request);
    }

    public async Task<List<ListBirimDto>> GetAgacAsync()
    {
        return await birimRepository.GetAgacAsync();
    }

    public async Task<List<BirimSecimDto>> GetUstBirimlerAsync()
    {
        return await birimRepository.GetKokBirimlerAsync();
    }

    public async Task<List<BirimSecimDto>> GetAltBirimByParentIdAsync(int ustId)
    {
        return await birimRepository.GetAltAgacAsync(ustId);
    }

    public async Task<List<BirimSecimDto>> GetUstBirimSecenekleriAsync(int? haricId = null)
    {
        var agac = await birimRepository.GetAgacAsync();

        if (haricId.HasValue)
        {
            var birim = await birimRepository.GetByIdAsync(haricId.Value);

            // Bir birim kendi alt ağacına taşınamaz; o dal seçeneklerden çıkarılır.
            // Aralık zaten yüklenmiş listede olduğu için ek sorgu gerekmez.
            if (birim is not null)
                agac = agac
                    .Where(b => b.Sol < birim.Sol || b.Sag > birim.Sag)
                    .ToList();
        }

        return agac
            .Select(b => new BirimSecimDto { Id = b.Id, Ad = b.TamYol })
            .ToList();
    }

    public async Task<UpdateBirimDto?> GetByIdAsync(int id)
    {
        var entity = await birimRepository.GetAsync(x => x.Id == id);
        return entity is null ? null : mapper.Map<UpdateBirimDto>(entity);
    }

    [LogAspect]
    public async Task<CreateBirimDto> AddAsync(CreateBirimDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await UstBirimiDogrulaAsync(dto.UstId);

            var entity = mapper.Map<Birim>(dto);
            entity.CreatedDate = DateTime.Now;
            entity.Durum = true;

            // Sol, Sag, Seviye ve Yol türev sütunlardır; kimlik atandıktan sonra
            // ağaç yeniden numaralandırılırken hesaplanır. Yol boş bırakılamaz,
            // sütun zorunlu.
            entity.Yol = string.Empty;

            var repository = unitOfWork.GetRepository<Birim, int>();
            await repository.AddAsync(entity);

            // Kiracı damgası kaydetme sırasında vurulduğu için organizasyon
            // ancak eklemeden sonra kesinleşir.
            await birimRepository.AgaciYenidenKurAsync(entity.OrganizasyonId);

            return mapper.Map<CreateBirimDto>(entity);
        });
    }

    [LogAspect]
    public async Task<UpdateBirimDto> UpdateAsync(UpdateBirimDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Birim, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id)
                         ?? throw new NotFoundException($"Birim {dto.Id} bulunamadı.");

            var hedef = await UstBirimiDogrulaAsync(dto.UstId);

            if (dto.UstId != entity.UstId)
            {
                if (dto.UstId == entity.Id)
                    throw new Core.Exceptions.ValidationException("Bir birim kendisine bağlanamaz.");

                // Nested set aralığı iç içeliği doğrudan söyler: hedef üst birim
                // taşınacak dalın aralığına düşüyorsa ağaç kendi içine kapanır.
                if (hedef is not null && hedef.Sol > entity.Sol && hedef.Sag < entity.Sag)
                    throw new Core.Exceptions.ValidationException(
                        "Bir birim kendi alt birimlerinden birine bağlanamaz.");
            }

            entity.Ad = dto.Ad;
            entity.Kod = dto.Kod;
            entity.Sira = dto.Sira;
            entity.UstId = dto.UstId;
            entity.UpdatedDate = DateTime.Now;

            await repository.UpdateAsync(entity);

            // Yalnızca üst birim değil, Ad ve Sıra da kardeş düzenini
            // değiştirebildiği için numaralandırma her güncellemede tazelenir.
            // İşlem tutarlıysa hiçbir satıra dokunulmaz.
            await birimRepository.AgaciYenidenKurAsync(entity.OrganizasyonId);

            return mapper.Map<UpdateBirimDto>(entity);
        });
    }

    [LogAspect]
    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Birim, int>();
            var entity = await repository.GetAsync(x => x.Id == id)
                         ?? throw new NotFoundException($"Birim {id} bulunamadı.");

            // Aktif alt birimi olan bir birim pasife alınamaz; önce alt ağaç boşaltılmalı.
            if (entity.Durum)
            {
                var altlar = await birimRepository.GetDogrudanAltBirimlerAsync(id);
                if (altlar.Any(b => b.Durum))
                    throw new Core.Exceptions.ValidationException(
                        "Aktif alt birimi olan bir birim pasife alınamaz. Önce alt birimleri pasife alın.");
            }

            entity.DeletedDate = entity.Durum ? DateTime.Now : null;

            if (!entity.Durum)
                entity.UpdatedDate = DateTime.Now;

            entity.Durum = !entity.Durum;

            // Pasife alma yalnızca durumu değiştirir, satır ağaçta kalır;
            // numaralandırma etkilenmez.
            await repository.UpdateAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        return await birimRepository.AnyAsync(x => x.Id == id);
    }

    private async Task<Birim?> UstBirimiDogrulaAsync(int? ustId)
    {
        if (!ustId.HasValue)
            return null;

        return await birimRepository.GetByIdAsync(ustId.Value)
               ?? throw new NotFoundException($"Üst birim {ustId.Value} bulunamadı.");
    }
}
