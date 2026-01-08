using Microsoft.AspNetCore.Mvc;
using RSS_Feeds.Core.BusinessLogic;
using RSS_Feeds.Data.Models;

namespace RSS_Feeds.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioArticulosGuardadoApiController(
        IUsuarioArticulosGuardadoBusiness usuarioArticulosGuardadoBusiness
    ) : ControllerBase
    {
        // GET: api/UsuarioArticulosGuardadoApi
        [HttpGet]
        public async Task<IEnumerable<UsuarioArticulosGuardado>> Get()
        {
            return await usuarioArticulosGuardadoBusiness.GetAsync();
        }

        // GET: api/UsuarioArticulosGuardadoApi/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<UsuarioArticulosGuardado>> Get(int id)
        {
            return await usuarioArticulosGuardadoBusiness.GetByIdAsync(id);
        }

        // GET: api/UsuarioArticulosGuardadoApi/usuario/1
        // artículos guardados por un usuario
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IEnumerable<UsuarioArticulosGuardado>> GetByUsuario(int usuarioId)
        {
            return await usuarioArticulosGuardadoBusiness.GetByUsuarioAsync(usuarioId);
        }

        // POST: api/UsuarioArticulosGuardadoApi
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] UsuarioArticulosGuardado entity)
        {
            var id = await usuarioArticulosGuardadoBusiness.SaveAndReturnIdAsync(entity);
            if (id <= 0)
                return BadRequest();

            return Ok(id);
        }

        // PUT: api/UsuarioArticulosGuardadoApi/5
        [HttpPut("{id}")]
        public async Task<int> Put(int id, [FromBody] UsuarioArticulosGuardado entity)
        {
            if (entity is null) return 0;

            if (entity.Id == 0)
                entity.Id = id;
            else if (entity.Id != id)
                return 0;

            return await usuarioArticulosGuardadoBusiness.SaveAndReturnIdAsync(entity);
        }

        // DELETE: api/UsuarioArticulosGuardadoApi/5
        [HttpDelete("{id}")]
        public Task<bool> Delete(int id)
        {
            return usuarioArticulosGuardadoBusiness.DeleteAsync(id);
        }
    }
}
