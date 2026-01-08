using RSS_Feeds.Architecture;
using RSS_Feeds.Architecture.Providers;
using RSS_Feeds.Models.DTOs;
using RSS_Feeds.ServiceLocator.Extensions;
using RSS_Feeds.ServiceLocator.Services.Contracts;

namespace RSS_Feeds.ServiceLocator.Services
{
    public interface IArticuloService : IService<ArticuloDTO>
    {
        Task<IEnumerable<ArticuloDTO>> GetDataByIdAsync(int id);
        Task<int> CreateAsync(ArticuloDTO dto);
        Task<bool> UpdateAsync(int id, ArticuloDTO dto);
        Task<bool> DeleteAsync(int id);
    }

    public class ArticuloService(IRestProvider restProvider, IConfiguration configuration)
        : IService<ArticuloDTO>, IArticuloService
    {
        public async Task<IEnumerable<ArticuloDTO>> GetDataAsync()
        {
            var url = configuration.GetStringFromAppSettings("APIS", "Articulo");
            var response = await restProvider.GetAsync(url, null);
            return await JsonProvider.DeserializeAsync<IEnumerable<ArticuloDTO>>(response);
        }

        public async Task<IEnumerable<ArticuloDTO>> GetDataByIdAsync(int id)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "Articulo");
            var full = $"{url.TrimEnd('/')}/{id}";
            var response = await restProvider.GetAsync(full, null);
            return await JsonProvider.DeserializeAsync<IEnumerable<ArticuloDTO>>(response);
        }

        public async Task<int> CreateAsync(ArticuloDTO dto)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "Articulo");
            var body = JsonProvider.Serialize(dto);

            var response = await restProvider.PostAsync(url, body);

            if (string.IsNullOrWhiteSpace(response))
                return 0;

            return int.TryParse(response.Trim(), out var id)
                ? id
                : 0;
        }

        public async Task<bool> UpdateAsync(int id, ArticuloDTO dto)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "Articulo");
            var body = JsonProvider.Serialize(dto);
            var full = $"{url.TrimEnd('/')}/{id}";
            var response = await restProvider.PutAsync(full, null, body);

            var s = response?.Trim().Trim('"');
            return bool.TryParse(s, out var ok) && ok;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "Articulo");
            var full = $"{url.TrimEnd('/')}/{id}";
            var response = await restProvider.DeleteAsync(full, null);

            var s = response?.Trim().Trim('"');
            return bool.TryParse(s, out var ok) && ok;
        }
    }
}
