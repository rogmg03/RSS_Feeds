using Microsoft.AspNetCore.Mvc;
using RSS_Feeds.Models.DTOs;
using RSS_Feeds.MVC.Models;
using RSS_Feeds.MVC.Services;
using System.Diagnostics;
using System.Net.Http.Json;

namespace RSS_Feeds.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _http;
        private readonly IRssReaderService _rss;

        public HomeController(
            ILogger<HomeController> logger,
            IHttpClientFactory factory,
            IRssReaderService rss)
        {
            _logger = logger;
            _http = factory.CreateClient("ApiClient");
            _rss = rss;
        }

        public async Task<IActionResult> Index()
        {
            // ---------- AUTH ----------
            if (!int.TryParse(
                HttpContext.Session.GetString("UserId"),
                out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // ---------- FEEDS SEGUIDOS ----------
            var usuarioFeeds = await _http
                .GetFromJsonAsync<List<UsuarioFeedDTO>>(
                    "api/ServiceLocator/usuario-feed"
                ) ?? new();

            var misFeedIds = usuarioFeeds
                .Where(x => x.UsuarioId == userId)
                .Select(x => x.FeedId)
                .ToHashSet();

            if (!misFeedIds.Any())
            {
                ViewBag.HasFeeds = false;
                return View(new List<FeedTimelineItemViewModel>());
            }

            ViewBag.HasFeeds = true;

            // ---------- FEEDS ----------
            var feeds = await _http
                .GetFromJsonAsync<List<FeedDTO>>(
                    "api/ServiceLocator/feeds"
                ) ?? new();

            var feedsById = feeds.ToDictionary(f => f.Id);

            // ---------- ARTÍCULOS EXISTENTES ----------
            var articulos = await _http
                .GetFromJsonAsync<List<ArticuloDTO>>(
                    "api/ServiceLocator/articulos"
                ) ?? new();

            var articulosByLink = articulos
                .Where(a => !string.IsNullOrWhiteSpace(a.Link))
                .ToDictionary(a => a.Link);

            // ---------- GUARDADOS ----------
            var guardados = await _http
                .GetFromJsonAsync<List<UsuarioArticulosGuardadoDTO>>(
                    "api/ServiceLocator/usuario-articulos-guardados"
                ) ?? new();

            var guardadosByArticuloId = guardados
                .Where(g => g.UsuarioId == userId)
                .ToDictionary(g => g.ArticuloId, g => g.Id);

            // ---------- RSS ( AQUÍ ESTÁ LA CLAVE) ----------
            var rssItems = new List<(RssItem Item, int FeedId)>();

            foreach (var feedId in misFeedIds)
            {
                if (!feedsById.TryGetValue(feedId, out var feed))
                    continue;

                try
                {
                    var items = await _rss.ReadAsync(
                        feed.Url!,
                        feed.Titulo ?? "Feed"
                    );

                    foreach (var item in items)
                    {
                        rssItems.Add((item, feed.Id)); //  NO se pierde el FeedId
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error leyendo RSS {Url}", feed.Url);
                }
            }

            // ---------- TIMELINE ----------
            var timeline = rssItems
                .OrderByDescending(x => x.Item.PublishedAt)
                .Select(x =>
                {
                    var item = x.Item;
                    var feedId = x.FeedId;

                    ArticuloDTO? articulo = null;
                    int? usuarioArticuloGuardadoId = null;

                    if (articulosByLink.TryGetValue(item.Link, out var a))
                    {
                        articulo = a;

                        if (guardadosByArticuloId.TryGetValue(a.Id, out var gId))
                            usuarioArticuloGuardadoId = gId;
                    }

                    return new FeedTimelineItemViewModel
                    {
                        //  ESTO YA NO ES 0
                        FeedId = feedId,

                        // RSS
                        Titulo = item.Title,
                        Descripcion = item.Description,
                        Contenido = null,
                        Autor = item.Author,
                        Imagen = item.ImageUrl,
                        FechaPublicacion = item.PublishedAt,
                        Link = item.Link,

                        // Feed
                        FeedTitulo = item.FeedTitle,
                        FeedUrl = item.Link,

                        // BD
                        ArticuloId = articulo?.Id,

                        // Usuario
                        IsLiked = usuarioArticuloGuardadoId.HasValue,
                        UsuarioArticuloGuardadoId = usuarioArticuloGuardadoId
                    };
                })
                .ToList();

            return View(timeline);
        }



        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
