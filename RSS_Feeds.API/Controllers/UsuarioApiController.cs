using Microsoft.AspNetCore.Mvc;
using RSS_Feeds.Core.BusinessLogic;
using RSS_Feeds.Data.Models;

namespace RSS_Feeds.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioApiController(IUsuarioBusiness usuarioBusiness) : ControllerBase
    {
        // GET: api/UsuarioApi
        [HttpGet]
        public async Task<IEnumerable<Usuario>> Get()
        {
            return await usuarioBusiness.GetAsync();
        }

        // GET: api/UsuarioApi/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<Usuario>> Get(int id)
        {
            return await usuarioBusiness.GetByIdAsync(id);
        }

        // POST: api/UsuarioApi
        [HttpPost]
        public Task<bool> Post([FromBody] Usuario usuario)
        {
            return usuarioBusiness.SaveAsync(usuario);
        }

        // PUT: api/UsuarioApi/5
        [HttpPut("{id}")]
        public async Task<bool> Put(int id, [FromBody] Usuario usuario)
        {
            if (usuario is null) return false;

            if (usuario.Id == 0)
                usuario.Id = id;
            else if (usuario.Id != id)
                return false;

            return await usuarioBusiness.SaveAsync(usuario);
        }

        // DELETE: api/UsuarioApi/5
        [HttpDelete("{id}")]
        public Task<bool> Delete(int id)
        {
            return usuarioBusiness.DeleteAsync(id);
        }
    }
}
