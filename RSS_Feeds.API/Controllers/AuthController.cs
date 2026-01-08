using Microsoft.AspNetCore.Mvc;
using RSS_Feeds.Architecture.Helpers;
using RSS_Feeds.Core.BusinessLogic;
using RSS_Feeds.Data.Models;
using RSS_Feeds.Models.DTOs;

namespace RSS_Feeds.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioBusiness _usuarioBusiness;

        public AuthController(IUsuarioBusiness usuarioBusiness)
        {
            _usuarioBusiness = usuarioBusiness;
        }

        // ---------- REGISTER ----------
        [HttpPost("register")]
        public async Task<ActionResult<bool>> Register(RegisterRequestDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(false);

            var existing = await _usuarioBusiness.GetByEmailAsync(dto.Email);
            if (existing != null)
                return Conflict(false);

            var user = new Usuario
            {
                Email = dto.Email,
                Nombre = dto.Nombre,
                PasswordHash = PasswordHasherHelper.Hash(dto.Password),
                CreadoEn = DateTime.UtcNow
            };

            return Ok(await _usuarioBusiness.SaveAsync(user));
        }

        // ---------- LOGIN ----------
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO dto)
        {
            var user = await _usuarioBusiness.GetByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized();

            if (!PasswordHasherHelper.Verify(user.PasswordHash, dto.Password))
                return Unauthorized();

            return Ok(new LoginResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                Nombre = user.Nombre
            });
        }
    }
}
