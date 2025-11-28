using RSS_Feeds.Data.Models;
using RSS_Feeds.Data.Repositories;

namespace RSS_Feeds.Core.BusinessLogic;

public interface IFeedBusiness
{
    Task<IEnumerable<Feed>> GetAsync();
    Task<IEnumerable<Feed>> GetByIdAsync(int id);
    Task<bool> SaveAsync(Feed entity);
    Task<bool> DeleteAsync(int id);
}

public class FeedBusiness(IRepositoryFeed repo) : IFeedBusiness
{
    private readonly IRepositoryFeed _repo = repo;

    public Task<IEnumerable<Feed>> GetAsync()
        => _repo.ReadAsync();

    public async Task<IEnumerable<Feed>> GetByIdAsync(int id)
    {
        var item = await _repo.FindAsync(id);
        return item is null ? Enumerable.Empty<Feed>() : new[] { item };
    }

    public Task<bool> SaveAsync(Feed entity)
        => _repo.CheckBeforeSavingAsync(entity);

    public Task<bool> DeleteAsync(int id)
        => _repo.DeleteAsync(id);
}
