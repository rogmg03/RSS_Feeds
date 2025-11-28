using Microsoft.EntityFrameworkCore;
using RSS_Feeds.Data.Models;

namespace RSS_Feeds.Data.Repositories
{
    public interface IRepositoryUsuarioArticulosGuardado
    {
        Task<bool> UpsertAsync(UsuarioArticulosGuardado entity, bool isUpdating);
        Task<bool> CreateAsync(UsuarioArticulosGuardado entity);
        Task<bool> DeleteAsync(UsuarioArticulosGuardado entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<UsuarioArticulosGuardado>> ReadAsync();
        Task<UsuarioArticulosGuardado?> FindAsync(int id);
        Task<bool> UpdateAsync(UsuarioArticulosGuardado entity);
        Task<bool> UpdateManyAsync(IEnumerable<UsuarioArticulosGuardado> entities);
        Task<bool> ExistsAsync(UsuarioArticulosGuardado entity);
        Task<bool> CheckBeforeSavingAsync(UsuarioArticulosGuardado entity);
    }

    public class RepositoryUsuarioArticulosGuardado
        : RepositoryBase<UsuarioArticulosGuardado>, IRepositoryUsuarioArticulosGuardado
    {
        public RepositoryUsuarioArticulosGuardado(RssDbContext ctx) : base(ctx) { }

        public async Task<bool> CheckBeforeSavingAsync(UsuarioArticulosGuardado entity)
        {
            if (entity.UsuarioId <= 0 || entity.ArticuloId <= 0)
                return false;

            var exists = await ExistsAsync(entity);
            return await UpsertAsync(entity, exists);
        }

        // Usa el DbSet que creó el scaffold en el contexto:
        // public virtual DbSet<UsuarioArticulosGuardado> UsuarioArticulosGuardados { get; set; } = null!;
        public new async Task<bool> ExistsAsync(UsuarioArticulosGuardado entity)
            => await DbContext.UsuarioArticulosGuardados.AnyAsync(x => x.Id == entity.Id);

        public Task<bool> UpsertAsync(UsuarioArticulosGuardado entity, bool isUpdating)
            => base.UpsertAsync(entity, isUpdating);

        public Task<bool> CreateAsync(UsuarioArticulosGuardado entity)
            => base.CreateAsync(entity);

        public Task<bool> UpdateAsync(UsuarioArticulosGuardado entity)
            => base.UpdateAsync(entity);

        public Task<bool> UpdateManyAsync(IEnumerable<UsuarioArticulosGuardado> entities)
            => base.UpdateManyAsync(entities);

        public Task<IEnumerable<UsuarioArticulosGuardado>> ReadAsync()
            => base.ReadAsync();

        public new Task<UsuarioArticulosGuardado?> FindAsync(int id)
            => base.FindAsync(id);

        public Task<bool> DeleteAsync(UsuarioArticulosGuardado entity)
            => base.DeleteAsync(entity);

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);
            return entity is not null && await DeleteAsync(entity);
        }
    }
}
