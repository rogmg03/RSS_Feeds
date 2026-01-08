using RSS_Feeds.Models.DTOs;

namespace RSS_Feeds.ServiceLocator.Services.Contracts
{
    public interface IAuthService
    {
        Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO dto);
        Task<bool> RegisterAsync(RegisterRequestDTO dto);
    }
}
