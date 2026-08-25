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
            // Bir birim kendi alt ağacına taşınamaz; o dal seçeneklerden çıkarılır.
            var birim = await birimRepository.GetByIdAsync(haricId.Value);
            if (birim is not null)
            {
                var yasakli = (await birimRepository.GetAltAgacEntityAsync(birim.Yol))
                    .Select(b => b.Id)
                    .ToHashSet();

                agac = agac.Where(b => !yasakli.Contains(b.Id)).ToList();
            }
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
            var ust = await UstBirimiDogrulaAsync(dto.UstId);

            var entity = mapper.Map<Birim>(dto);
            entity.CreatedDate = DateTime.Now;
            entity.Durum = true;
            entity.Seviye = ust is null ? 0 : ust.Seviye + 1;
            entity.Yol = string.Empty; // kimlik atandıktan sonra hesaplanır

            var repository = unitOfWork.GetRepository<Birim, int>();
            await repository.AddAsync(entity);

            // Yol, kendi kimliğini içerdiği için ancak ekleme sonrası kesinleşir.
            entity.Yol = YolHesapla(ust?.Yol, entity.Id);
            await repository.UpdateAsync(entity);

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

            var eskiUstId = entity.UstId;
            var eskiYol = entity.Yol;
            var eskiSeviye = entity.Seviye;

            if (dto.UstId != eskiUstId)
            {
                if (dto.UstId == entity.Id)
                    throw new Core.Exceptions.ValidationException("Bir birim kendisine bağlanamaz.");

                var altAgac = await birimRepository.GetAltAgacEntityAsync(eskiYol);
                if (dto.UstId.HasValue && altAgac.Any(b => b.Id == dto.UstId.Value))
                    throw new Core.Exceptions.ValidationException(
                        "Bir birim kendi alt birimlerinden birine bağlanamaz.");
            }

            var yeniUst = await UstBirimiDogrulaAsync(dto.UstId);

            entity.Ad = dto.Ad;
            entity.Kod = dto.Kod;
            entity.Sira = dto.Sira;
            entity.UstId = dto.UstId;
            entity.Seviye = yeniUst is null ? 0 : yeniUst.Seviye + 1;
            entity.Yol = YolHesapla(yeniUst?.Yol, entity.Id);
            entity.UpdatedDate = DateTime.Now;

            await repository.UpdateAsync(entity);

            if (entity.Yol != eskiYol)
                await AltAgaciTasiAsync(eskiYol, entity.Yol, entity.Seviye - eskiSeviye, entity.Id);

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

            await repository.UpdateAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        return await birimRepository.AnyAsync(x => x.Id == id);
    }

    private static string YolHesapla(string? ustYol, int id) =>
        string.IsNullOrEmpty(ustYol) ? $"/{id}/" : $"{ustYol}{id}/";

    private async Task<Birim?> UstBirimiDogrulaAsync(int? ustId)
    {
        if (!ustId.HasValue)
            return null;

        return await birimRepository.GetByIdAsync(ustId.Value)
               ?? throw new NotFoundException($"Üst birim {ustId.Value} bulunamadı.");
    }

    /// <summary>
    /// Üst birim değiştiğinde alt ağacın tamamındaki Yol ve Seviye değerlerini yeniden yazar.
    /// </summary>
    private async Task AltAgaciTasiAsync(string eskiYol, string yeniYol, int seviyeFarki, int kokId)
    {
        var altAgac = (await birimRepository.GetAltAgacEntityAsync(eskiYol))
            .Where(b => b.Id != kokId)
            .ToList();

        if (altAgac.Count == 0)
            return;

        foreach (var alt in altAgac)
        {
            alt.Yol = string.Concat(yeniYol, alt.Yol.AsSpan(eskiYol.Length));
            alt.Seviye += seviyeFarki;
            alt.UpdatedDate = DateTime.Now;
        }

        await unitOfWork.GetRepository<Birim, int>().UpdateRangeAsync(altAgac);
    }
}
