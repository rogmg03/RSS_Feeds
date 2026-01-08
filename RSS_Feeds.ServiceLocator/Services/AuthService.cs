using RSS_Feeds.Architecture;
using RSS_Feeds.Architecture.Providers;
using RSS_Feeds.Models.DTOs;
using RSS_Feeds.ServiceLocator.Extensions;
using RSS_Feeds.ServiceLocator.Services.Contracts;

namespace RSS_Feeds.ServiceLocator.Services
{
    public class AuthService(
        IRestProvider restProvider,
        IConfiguration configuration
    ) : IAuthService
    {
        private string GetBaseUrl()
            => configuration.GetStringFromAppSettings("APIS", "Auth");

        // ---------- LOGIN ----------
        public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO dto)
        {
            var url = $"{GetBaseUrl().TrimEnd('/')}/login";

            var body = JsonProvider.Serialize(dto);
            var response = await restProvider.PostAsync(url, body);

            if (string.IsNullOrWhiteSpace(response))
                return null;

            return await JsonProvider.DeserializeAsync<LoginResponseDTO>(response);
        }

        // ---------- REGISTER ----------
        public async Task<bool> RegisterAsync(RegisterRequestDTO dto)
        {
            var url = $"{GetBaseUrl().TrimEnd('/')}/register";

            var body = JsonProvider.Serialize(dto);
            var response = await restProvider.PostAsync(url, body);

            var s = response?.Trim().Trim('"');
            return bool.TryParse(s, out var ok) && ok;
        }
    }
}
