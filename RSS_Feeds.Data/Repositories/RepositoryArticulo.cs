using Microsoft.EntityFrameworkCore;
using RSS_Feeds.Data.Models;

namespace RSS_Feeds.Data.Repositories
{
    public interface IRepositoryArticulo
    {
        Task<bool> UpsertAsync(Articulo entity, bool isUpdating);
        Task<bool> CreateAsync(Articulo entity);
        Task<bool> DeleteAsync(Articulo entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Articulo>> ReadAsync();
        Task<Articulo?> FindAsync(int id);
        Task<bool> UpdateAsync(Articulo entity);
        Task<bool> UpdateManyAsync(IEnumerable<Articulo> entities);
        Task<bool> ExistsAsync(Articulo entity);
        Task<bool> CheckBeforeSavingAsync(Articulo entity);
    }

    public class RepositoryArticulo : RepositoryBase<Articulo>, IRepositoryArticulo
    {
        public RepositoryArticulo(RssDbContext ctx) : base(ctx) { }

        public async Task<bool> CheckBeforeSavingAsync(Articulo entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Titulo))
                return false;
            if (string.IsNullOrWhiteSpace(entity.Link))
                return false;

            var exists = await ExistsAsync(entity);
            return await UpsertAsync(entity, exists);
        }

        public new async Task<bool> ExistsAsync(Articulo entity)
            => await DbContext.Articulos.AnyAsync(x => x.Id == entity.Id);

        public Task<bool> UpsertAsync(Articulo entity, bool isUpdating) => base.UpsertAsync(entity, isUpdating);
        public Task<bool> CreateAsync(Articulo entity) => base.CreateAsync(entity);
        public Task<bool> UpdateAsync(Articulo entity) => base.UpdateAsync(entity);
        public Task<bool> UpdateManyAsync(IEnumerable<Articulo> entities) => base.UpdateManyAsync(entities);
        public Task<IEnumerable<Articulo>> ReadAsync() => base.ReadAsync();
        public new Task<Articulo?> FindAsync(int id) => base.FindAsync(id);
        public Task<bool> DeleteAsync(Articulo entity) => base.DeleteAsync(entity);

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);
            return entity is not null && await DeleteAsync(entity);
        }
    }
}
