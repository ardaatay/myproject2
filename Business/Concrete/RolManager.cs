using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.Kullanici;
using Dto.Rol;
using Entity.Concrete;
using Repository.Abstract;
using Repository.UnitOfWork;

namespace Business.Concrete
{
    public class RolManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper, IRolRepository rolRepository)
        : IRolService
    {
        public async Task<List<ListRolDto>> GetAllAsync()
        {
            var entities = await unitOfWork.GetRepository<Rol, int>().GetAllAsync();
            return mapper.Map<List<ListRolDto>>(entities);
        }

        public async Task<UpdateRolDto> GetByIdAsync(int id)
        {
            var entity = await unitOfWork.GetRepository<Rol, int>().GetAsync(x => x.Id == id);
            return mapper.Map<UpdateRolDto>(entity);
        }

        public async Task<CreateRolDto> AddAsync(CreateRolDto dto)
        {
            return await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var entity = mapper.Map<Rol>(dto);
                var repository = unitOfWork.GetRepository<Rol, int>();

                await repository.AddAsync(entity);
                return mapper.Map<CreateRolDto>(entity);
            });
        }

        public async Task<UpdateRolDto> UpdateAsync(UpdateRolDto dto)
        {
            return await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var repository = unitOfWork.GetRepository<Rol, int>();
                var entity = await repository.GetAsync(x => x.Id == dto.Id);

                if (entity == null)
                    throw new NotFoundException($"Rol with id {dto.Id} not found");

                mapper.Map(dto, entity);
                await repository.UpdateAsync(entity);
                return mapper.Map<UpdateRolDto>(entity);
            });
        }

        public async Task DeleteAsync(int id)
        {
            await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var repository = unitOfWork.GetRepository<Rol, int>();
                var entity = await repository.GetAsync(x => x.Id == id);

                if (entity == null)
                    throw new NotFoundException($"Rol with id {id} not found");

                await repository.DeleteAsync(entity);
            });
        }

        public async Task<bool> AnyAsync(int id)
        {
            var repository = unitOfWork.GetRepository<Rol, int>();
            return await repository.AnyAsync(x => x.Id == id);
        }

        public async Task RolleriKaydetAsync(KullaniciRolAtamaDto model)
        {
            await rolRepository.RolleriKaydetAsync(model);
        }
    }
}