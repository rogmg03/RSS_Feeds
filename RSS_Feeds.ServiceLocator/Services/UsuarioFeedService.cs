using RSS_Feeds.Architecture;
using RSS_Feeds.Architecture.Providers;
using RSS_Feeds.Models.DTOs;
using RSS_Feeds.ServiceLocator.Extensions;
using RSS_Feeds.ServiceLocator.Services.Contracts;

namespace RSS_Feeds.ServiceLocator.Services
{
    public interface IUsuarioFeedService : IService<UsuarioFeedDTO>
    {
        Task<IEnumerable<UsuarioFeedDTO>> GetDataByIdAsync(int id);
        Task<bool> CreateAsync(UsuarioFeedDTO dto);
        Task<bool> UpdateAsync(int id, UsuarioFeedDTO dto);
        Task<bool> DeleteAsync(int id);
    }

    public class UsuarioFeedService(IRestProvider restProvider, IConfiguration configuration)
        : IService<UsuarioFeedDTO>, IUsuarioFeedService
    {
        public async Task<IEnumerable<UsuarioFeedDTO>> GetDataAsync()
        {
            var url = configuration.GetStringFromAppSettings("APIS", "UsuarioFeed");
            var response = await restProvider.GetAsync(url, null);
            return await JsonProvider.DeserializeAsync<IEnumerable<UsuarioFeedDTO>>(response);
        }

        public async Task<IEnumerable<UsuarioFeedDTO>> GetDataByIdAsync(int id)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "UsuarioFeed");
            var full = $"{url.TrimEnd('/')}/{id}";
            var response = await restProvider.GetAsync(full, null);
            return await JsonProvider.DeserializeAsync<IEnumerable<UsuarioFeedDTO>>(response);
        }

        public async Task<bool> CreateAsync(UsuarioFeedDTO dto)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "UsuarioFeed");
            var body = JsonProvider.Serialize(dto);

            var response = await restProvider.PostAsync(url, body);

            var s = response?.Trim().Trim('"');
            return bool.TryParse(s, out var ok) && ok;
        }

        public async Task<bool> UpdateAsync(int id, UsuarioFeedDTO dto)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "UsuarioFeed");
            var body = JsonProvider.Serialize(dto);
            var full = $"{url.TrimEnd('/')}/{id}";
            var response = await restProvider.PutAsync(full, null, body);

            var s = response?.Trim().Trim('"');
            return bool.TryParse(s, out var ok) && ok;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "UsuarioFeed");
            var full = $"{url.TrimEnd('/')}/{id}";
            var response = await restProvider.DeleteAsync(full, null);

            var s = response?.Trim().Trim('"');
            return bool.TryParse(s, out var ok) && ok;
        }
    }
}
