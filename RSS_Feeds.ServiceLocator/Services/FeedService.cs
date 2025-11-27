using RSS_Feeds.Architecture.Providers;
using RSS_Feeds.Models.DTOs;
using RSS_Feeds.ServiceLocator.Extensions;
using RSS_Feeds.ServiceLocator.Services.Contracts;

namespace RSS_Feeds.ServiceLocator.Services
{
    public interface IFeedService : IService<FeedDTO>
    {
        Task<IEnumerable<FeedDTO>> GetDataByIdAsync(int id);
        Task<bool> CreateAsync(FeedDTO dto);
        Task<bool> UpdateAsync(int id, FeedDTO dto);
        Task<bool> DeleteAsync(int id);
    }

    public class FeedService(Architecture.IRestProvider restProvider, IConfiguration configuration)
        : IService<FeedDTO>, IFeedService
    {
        public async Task<IEnumerable<FeedDTO>> GetDataAsync()
        {
            var url = configuration.GetStringFromAppSettings("APIS", "Feed");
            var response = await restProvider.GetAsync(url, null);
            return await JsonProvider.DeserializeAsync<IEnumerable<FeedDTO>>(response);
        }

        public async Task<IEnumerable<FeedDTO>> GetDataByIdAsync(int id)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "Feed");
            var full = $"{url.TrimEnd('/')}/{id}";
            var response = await restProvider.GetAsync(full, null);
            return await JsonProvider.DeserializeAsync<IEnumerable<FeedDTO>>(response);
        }

        public async Task<bool> CreateAsync(FeedDTO dto)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "Feed");
            var body = JsonProvider.Serialize(dto);
            var response = await restProvider.PostAsync(url, body);

            var s = response?.Trim().Trim('"');
            return bool.TryParse(s, out var ok) && ok;
        }

        public async Task<bool> UpdateAsync(int id, FeedDTO dto)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "Feed");
            var body = JsonProvider.Serialize(dto);
            var full = $"{url.TrimEnd('/')}/{id}";
            var response = await restProvider.PutAsync(full, null, body);

            var s = response?.Trim().Trim('"');
            return bool.TryParse(s, out var ok) && ok;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "Feed");
            var full = $"{url.TrimEnd('/')}/{id}";
            var response = await restProvider.DeleteAsync(full, null);

            var s = response?.Trim().Trim('"');
            return bool.TryParse(s, out var ok) && ok;
        }
    }
}
