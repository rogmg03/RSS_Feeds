using Microsoft.AspNetCore.Mvc;
using RSS_Feeds.Models.DTOs;
using RSS_Feeds.MVC.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace RSS_Feeds.MVC.Controllers
{
    public class FeedsController : Controller
    {
        private readonly HttpClient _http;
        private readonly IHttpClientFactory _httpClientFactory;

        public FeedsController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
            _httpClientFactory = factory;
        }

        // ---------- DISCOVER ----------
        public async Task<IActionResult> Discover()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrWhiteSpace(userIdStr) || !int.TryParse(userIdStr, out var userId))
                return RedirectToAction("Login", "Account");

            var feeds = await _http
                .GetFromJsonAsync<List<FeedDTO>>(
                    "api/ServiceLocator/feeds"
                ) ?? new();

            var usuarioFeeds = await _http
                .GetFromJsonAsync<List<UsuarioFeedDTO>>(
                    "api/ServiceLocator/usuario-feed"
                ) ?? new();

            var misFeeds = usuarioFeeds
                .Where(x => x.UsuarioId == userId)
                .ToList();

            var model = feeds.Select(feed =>
            {
                var uf = misFeeds.FirstOrDefault(x => x.FeedId == feed.Id);

                return new FeedDiscoverViewModel
                {
                    Id = feed.Id,
                    Url = feed.Url,
                    Titulo = feed.Titulo,
                    Descripcion = feed.Descripcion,
                    Categoria = feed.Categoria,

                    IsFollowed = uf != null,
                    UsuarioFeedId = uf?.Id
                };
            }).ToList();

            return View(model);
        }

        // ---------- FOLLOW ----------
        [HttpPost]
        public async Task<IActionResult> Follow([FromBody] FollowFeedRequestDTO request)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrWhiteSpace(userIdStr))
                return Unauthorized();

            var payload = new
            {
                usuarioId = int.Parse(userIdStr),
                feedId = request.FeedId,
                alias = request.Alias
            };

            var response = await _http.PostAsJsonAsync(
                "api/ServiceLocator/usuario-feed",
                payload
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }

            return Ok();
        }

        // ---------- CREATE FEED ----------
        [HttpPost]
        public async Task<IActionResult> CreateFeed([FromBody] FeedDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Url))
            {
                return BadRequest("La URL del feed es requerida");
            }

            var response = await _http.PostAsJsonAsync(
                "api/ServiceLocator/feeds",
                dto
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }

            var result = await response.Content.ReadFromJsonAsync<bool>();
            if (!result)
            {
                return BadRequest("No se pudo crear el feed. Puede que ya exista un feed con esta URL.");
            }

            return Ok();
        }

        // ---------- SEARCH RSS FINDER ----------
        [HttpGet]
        public async Task<IActionResult> SearchRssFinder([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword) || keyword.Length < 3)
            {
                return BadRequest("El keyword debe tener al menos 3 caracteres");
            }

            try
            {
                // Crear HttpClient para RSS Finder
                var rssFinderClient = _httpClientFactory.CreateClient();
                rssFinderClient.Timeout = TimeSpan.FromSeconds(15);
                rssFinderClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                rssFinderClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                rssFinderClient.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9,en;q=0.8");

                // Obtener la página web de RSS Finder
                var pageUrl = $"https://rssfinder.app/?q={Uri.EscapeDataString(keyword)}";
                var response = await rssFinderClient.GetAsync(pageUrl);

                if (!response.IsSuccessStatusCode)
                {
                    // Si falla, usar feeds sugeridos como fallback
                    var fallbackKeyword = keyword.ToLowerInvariant();
                    var fallbackFeeds = GetSuggestedFeedsByKeyword(fallbackKeyword);
                    return Json(fallbackFeeds);
                }

                var htmlContent = await response.Content.ReadAsStringAsync();

                // Buscar JSON embebido en la página (Next.js/React apps suelen usar window.__NEXT_DATA__)
                var jsonDataPatterns = new[]
                {
                    @"window\.__NEXT_DATA__\s*=\s*({.+?});",
                    @"<script[^>]*id=""__NEXT_DATA__""[^>]*type=""application/json"">(.+?)</script>",
                    @"window\.__APOLLO_STATE__\s*=\s*({.+?});",
                    @"<script[^>]*>.*?var\s+__INITIAL_STATE__\s*=\s*({.+?});"
                };

                string? jsonContent = null;
                foreach (var pattern in jsonDataPatterns)
                {
                    var match = System.Text.RegularExpressions.Regex.Match(
                        htmlContent,
                        pattern,
                        System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase
                    );

                    if (match.Success && match.Groups.Count > 1)
                    {
                        jsonContent = match.Groups[1].Value;
                        break;
                    }
                }

                // Si encontramos JSON, intentar deserializar
                if (!string.IsNullOrWhiteSpace(jsonContent))
                {
                    try
                    {
                        // Limpiar el JSON si tiene comentarios HTML o caracteres extra
                        jsonContent = System.Text.RegularExpressions.Regex.Replace(
                            jsonContent,
                            @"<!--.*?-->",
                            "",
                            System.Text.RegularExpressions.RegexOptions.Singleline
                        );

                        var rssFinderResponse = JsonSerializer.Deserialize<RssFinderFeedResponse>(
                            jsonContent,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );

                        if (rssFinderResponse?.Data?.SearchInFinder?.TextResult?.Feeds != null &&
                            rssFinderResponse.Data.SearchInFinder.TextResult.Feeds.Any())
                        {
                            var feeds = rssFinderResponse.Data.SearchInFinder.TextResult.Feeds
                                .Where(f => !string.IsNullOrWhiteSpace(f.Url))
                                .Select(f => new FeedDTO
                                {
                                    Url = f.Url!,
                                    Titulo = f.Title,
                                    Descripcion = f.Description,
                                    Categoria = null,
                                    Idioma = null
                                })
                                .ToList();

                            if (feeds.Any())
                            {
                                return Json(feeds);
                            }
                        }
                    }
                    catch (JsonException ex)
                    {
                        // Continuar con fallback si falla el parsing
                        System.Diagnostics.Debug.WriteLine($"Error parsing JSON: {ex.Message}");
                    }
                }

                // Si no se encontraron datos en el HTML, usar feeds sugeridos
                var keywordLower = keyword.ToLowerInvariant();
                var suggestedFeeds = GetSuggestedFeedsByKeyword(keywordLower);
                return Json(suggestedFeeds);
            }
            catch (Exception ex)
            {
                // En caso de error, retornar feeds sugeridos
                var keywordLower = keyword.ToLowerInvariant();
                var suggestedFeeds = GetSuggestedFeedsByKeyword(keywordLower);
                return Json(suggestedFeeds);
            }
        }

        // Método helper para obtener feeds sugeridos por keyword
        private List<FeedDTO> GetSuggestedFeedsByKeyword(string keyword)
        {
            var feeds = new List<FeedDTO>();

            // Feeds de tecnología
            if (keyword.Contains("tech") || keyword.Contains("tecnolog") || keyword.Contains("program"))
            {
                feeds.AddRange(new List<FeedDTO>
                {
                    new FeedDTO { Url = "https://www.wired.com/feed/rss", Titulo = "Wired - Technology", Descripcion = "Technology news and insights" },
                    new FeedDTO { Url = "https://feeds.feedburner.com/oreilly/radar", Titulo = "O'Reilly Radar", Descripcion = "Technology trends and insights" },
                    new FeedDTO { Url = "https://www.theverge.com/rss/index.xml", Titulo = "The Verge", Descripcion = "Technology news" }
                });
            }

            // Feeds de noticias
            if (keyword.Contains("news") || keyword.Contains("notici"))
            {
                feeds.AddRange(new List<FeedDTO>
                {
                    new FeedDTO { Url = "https://feeds.bbci.co.uk/news/rss.xml", Titulo = "BBC News", Descripcion = "Latest news from BBC" },
                    new FeedDTO { Url = "https://rss.cnn.com/rss/edition.rss", Titulo = "CNN", Descripcion = "CNN News" },
                    new FeedDTO { Url = "https://feeds.npr.org/1001/rss.xml", Titulo = "NPR News", Descripcion = "NPR News Feed" }
                });
            }

            // Feeds de ciencia
            if (keyword.Contains("science") || keyword.Contains("ciencia"))
            {
                feeds.AddRange(new List<FeedDTO>
                {
                    new FeedDTO { Url = "https://www.scientificamerican.com/rss/all/", Titulo = "Scientific American", Descripcion = "Science news" },
                    new FeedDTO { Url = "https://www.nature.com/nature.rss", Titulo = "Nature", Descripcion = "Nature Science Journal" }
                });
            }

            // Feeds de desarrollo
            if (keyword.Contains("dev") || keyword.Contains("code") || keyword.Contains("desarroll"))
            {
                feeds.AddRange(new List<FeedDTO>
                {
                    new FeedDTO { Url = "https://dev.to/feed", Titulo = "Dev.to", Descripcion = "Developer community blog" },
                    new FeedDTO { Url = "https://stackoverflow.blog/feed/", Titulo = "Stack Overflow Blog", Descripcion = "Developer insights" },
                    new FeedDTO { Url = "https://www.reddit.com/r/programming/.rss", Titulo = "r/programming", Descripcion = "Programming discussions" }
                });
            }

            return feeds;
        }

        // ---------- UNFOLLOW ----------
        [HttpPost]
        public async Task<IActionResult> Unfollow(int usuarioFeedId)
        {
            await _http.DeleteAsync(
                $"api/ServiceLocator/usuario-feed/{usuarioFeedId}"
            );

            return RedirectToAction(nameof(Discover));
        }

        // ---------- POPULAR ----------
        public IActionResult Popular()
        {
            return RedirectToAction(nameof(Discover));
        }

        public async Task<IActionResult> Saved()
        {
            // ---------- AUTH ----------
            if (!int.TryParse(
                HttpContext.Session.GetString("UserId"),
                out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // ---------- GUARDADOS ----------
            var guardados = await _http
                .GetFromJsonAsync<List<UsuarioArticulosGuardadoDTO>>(
                    "api/ServiceLocator/usuario-articulos-guardados"
                ) ?? new();

            var misGuardados = guardados
                .Where(g => g.UsuarioId == userId)
                .ToList();

            if (!misGuardados.Any())
            {
                ViewBag.EmptySaved = true;
                return View(new List<SavedArticleViewModel>());
            }

            ViewBag.EmptySaved = false;

            // ---------- ARTÍCULOS ----------
            var articulos = await _http
                .GetFromJsonAsync<List<ArticuloDTO>>(
                    "api/ServiceLocator/articulos"
                ) ?? new();

            var articulosById = articulos.ToDictionary(a => a.Id);

            // ---------- FEEDS ----------
            var feeds = await _http
                .GetFromJsonAsync<List<FeedDTO>>(
                    "api/ServiceLocator/feeds"
                ) ?? new();

            var feedsById = feeds.ToDictionary(f => f.Id);

            // ---------- VIEW MODEL ----------
            var model = misGuardados
                .Where(g => articulosById.ContainsKey(g.ArticuloId))
                .Select(g =>
                {
                    var articulo = articulosById[g.ArticuloId];
                    feedsById.TryGetValue(articulo.FeedId, out var feed);

                    return new SavedArticleViewModel
                    {
                        ArticuloId = articulo.Id,
                        Titulo = articulo.Titulo,
                        Descripcion = articulo.Descripcion,
                        Contenido = articulo.Contenido,
                        Autor = articulo.Autor,
                        Imagen = articulo.Imagen,
                        Link = articulo.Link,
                        FechaPublicacion = articulo.FechaPublicacion,

                        FeedTitulo = feed?.Titulo ?? "Feed",

                        UsuarioArticuloGuardadoId = g.Id,
                        FechaGuardado = g.FechaGuardado,
                        Notas = g.Notas
                    };
                })
                .OrderByDescending(x => x.FechaGuardado)
                .ToList();

            return View(model);
        }
    }
}
