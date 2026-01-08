using Microsoft.EntityFrameworkCore;
using RSS_Feeds.Data.Models;

namespace RSS_Feeds.Data.Repositories
{
    public interface IRepositoryUsuarioFeed
    {
        Task<bool> UpsertAsync(UsuarioFeed entity, bool isUpdating);
        Task<bool> CreateAsync(UsuarioFeed entity);
        Task<bool> DeleteAsync(UsuarioFeed entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<UsuarioFeed>> ReadAsync();
        Task<UsuarioFeed?> FindAsync(int id);
        Task<bool> UpdateAsync(UsuarioFeed entity);
        Task<bool> UpdateManyAsync(IEnumerable<UsuarioFeed> entities);
        Task<bool> ExistsAsync(UsuarioFeed entity);
        Task<bool> CheckBeforeSavingAsync(UsuarioFeed entity);
    }

    public class RepositoryUsuarioFeed : RepositoryBase<UsuarioFeed>, IRepositoryUsuarioFeed
    {
        public RepositoryUsuarioFeed(RssDbContext ctx) : base(ctx) { }

        public async Task<bool> CheckBeforeSavingAsync(UsuarioFeed entity)
        {
            if (entity.UsuarioId <= 0 || entity.FeedId <= 0)
                return false;

            var exists = await ExistsAsync(entity);
            return await UpsertAsync(entity, exists);
        }

        public async Task<bool> ExistsAsync(UsuarioFeed entity)
        {
            return await DbContext.UsuarioFeeds.AnyAsync(x =>
                x.UsuarioId == entity.UsuarioId &&
                x.FeedId == entity.FeedId
            );
        }

        public Task<bool> UpsertAsync(UsuarioFeed entity, bool isUpdating) => base.UpsertAsync(entity, isUpdating);
        public Task<bool> CreateAsync(UsuarioFeed entity) => base.CreateAsync(entity);
        public Task<bool> UpdateAsync(UsuarioFeed entity) => base.UpdateAsync(entity);
        public Task<bool> UpdateManyAsync(IEnumerable<UsuarioFeed> entities) => base.UpdateManyAsync(entities);
        public Task<IEnumerable<UsuarioFeed>> ReadAsync() => base.ReadAsync();
        public new Task<UsuarioFeed?> FindAsync(int id) => base.FindAsync(id);
        public Task<bool> DeleteAsync(UsuarioFeed entity) => base.DeleteAsync(entity);

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);
            return entity is not null && await DeleteAsync(entity);
        }
    }
}
