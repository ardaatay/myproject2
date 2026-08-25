using AutoMapper;
using Business.Abstract;
using Dto.DTOs;
using Dto.KullaniciBirim;
using Entity.Concrete;
using Repository.Abstract;

namespace Business.Concrete;

public class KullaniciBirimManager(
    IKullaniciBirimRepository kullaniciBirimRepository,
    IMapper mapper) : IKullaniciBirimService
{
    public async Task<DataTablesResponse<ListKullaniciBirimDto>> GetAllAsync(DataTablesRequest request)
    {
        return await kullaniciBirimRepository.ProcessTableRequestAsync(request);
    }

    public async Task<List<KullaniciBirimDto>> GetByKullaniciIdAsync(int kullaniciId)
    {
        var kullaniciBirimler =
            await kullaniciBirimRepository.GetAllWithOptionsAsync(x => x.KullaniciId == kullaniciId && x.Durum == true,
                x => x.OrderBy(a => a.BirimAd),
                x => x.Kullanici!);

        return mapper.Map<List<KullaniciBirimDto>>(kullaniciBirimler);
    }

    public async Task<UpdateKullaniciBirimDto?> GetByKullaniciIdAndBirimIdAsync(int kullaniciId, int birimId)
    {
        var kullaniciBirim =
            await kullaniciBirimRepository.GetAsync(x => x.KullaniciId == kullaniciId && x.BirimId == birimId);

        return mapper.Map<UpdateKullaniciBirimDto>(kullaniciBirim);
    }

    public async Task<ListKullaniciBirimDto?> GetByIdAsync(int id)
    {
        var kullaniciBirim = await kullaniciBirimRepository.GetAsync(x => x.Id == id, x => x.Kullanici!);

        return mapper.Map<ListKullaniciBirimDto>(kullaniciBirim);
    }

    public async Task AddAsync(CreateKullaniciBirimDto createKullaniciBirimDto)
    {
        var kullaniciBirim = await kullaniciBirimRepository.GetByKullaniciIdAsync(createKullaniciBirimDto.KullaniciId);
        foreach (var birim in kullaniciBirim)
        {
            if (birim.BirimId == createKullaniciBirimDto.BirimId)
            {
                throw new InvalidOperationException("Kullanıcı bu birimde zaten bulunuyor.");
            }
        }

        var newKullaniciBirim = mapper.Map<KullaniciBirim>(createKullaniciBirimDto);
        await kullaniciBirimRepository.AddAsync(newKullaniciBirim);
    }

    public async Task UpdateAsync(UpdateKullaniciBirimDto updateKullaniciBirimDto)
    {
        var kullaniciBirim = await kullaniciBirimRepository.GetByIdAsync(updateKullaniciBirimDto.Id);
        if (kullaniciBirim is null)
        {
            throw new InvalidOperationException("Kullanıcı birim bulunamadı.");
        }

        kullaniciBirim.BirimId = updateKullaniciBirimDto.BirimId;
        kullaniciBirim.BirimAd = updateKullaniciBirimDto.BirimAd;
        kullaniciBirim.Durum = true;

        await kullaniciBirimRepository.UpdateAsync(kullaniciBirim);
    }

    public async Task DeleteAsync(int id)
    {
        var kullaniciBirim = await kullaniciBirimRepository.GetByIdAsync(id);
        if (kullaniciBirim is null)
        {
            throw new InvalidOperationException("Kullanıcı birim bulunamadı.");
        }

        kullaniciBirim.Durum = !kullaniciBirim.Durum;

        await kullaniciBirimRepository.UpdateAsync(kullaniciBirim);
    }
}