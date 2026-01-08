using Microsoft.AspNetCore.Mvc;
using RSS_Feeds.Models.DTOs;

namespace RSS_Feeds.MVC.Controllers
{
    [IgnoreAntiforgeryToken]
    public class ArticlesController : Controller
    {
        private readonly HttpClient _http;

        public ArticlesController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // -------- LIKE --------
        [HttpPost]
        public async Task<IActionResult> Like([FromBody] ArticuloDTO articulo)
        {
            try
            {
                if (!int.TryParse(HttpContext.Session.GetString("UserId"), out var userId))
                    return Unauthorized();

                // 1️ Crear artículo
                var articuloResponse = await _http.PostAsJsonAsync(
                    "api/ServiceLocator/articulos",
                    articulo
                );

                if (!articuloResponse.IsSuccessStatusCode)
                {
                    var err = await articuloResponse.Content.ReadAsStringAsync();
                    return StatusCode(500, $"Error creando artículo: {err}");
                }

                // 2️ Volver a traer artículos
                var articulos = await _http.GetFromJsonAsync<List<ArticuloDTO>>(
                    "api/ServiceLocator/articulos"
                );

                if (articulos == null)
                    return StatusCode(500, "Lista de artículos es null");

                var articuloCreado = articulos
                    .OrderByDescending(a => a.Id)
                    .FirstOrDefault(a => a.Link == articulo.Link);

                if (articuloCreado == null)
                    return StatusCode(500, "Artículo no encontrado luego de crearlo");

                // 3️ Crear usuario_articulos_guardado
                var guardado = new UsuarioArticulosGuardadoDTO
                {
                    UsuarioId = userId,
                    ArticuloId = articuloCreado.Id
                };

                var guardadoResponse = await _http.PostAsJsonAsync(
                    "api/ServiceLocator/usuario-articulos-guardados",
                    guardado
                );

                if (!guardadoResponse.IsSuccessStatusCode)
                {
                    var err = await guardadoResponse.Content.ReadAsStringAsync();
                    return StatusCode(500, $"Error guardando relación: {err}");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        // -------- UNLIKE --------
        [HttpDelete]
        public async Task<IActionResult> Unlike(int id)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized();

            var response = await _http.DeleteAsync(
                $"api/ServiceLocator/usuario-articulos-guardados/{id}"
            );

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            return Ok();
        }
    }

    public class LikeRequest
    {
        public int ArticuloId { get; set; }
    }
}
