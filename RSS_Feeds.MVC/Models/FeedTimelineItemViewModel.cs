namespace RSS_Feeds.MVC.Models
{
    public class FeedTimelineItemViewModel
    {
        // Artículo (si existe en BD)
        public int? ArticuloId { get; set; }

        // RSS
        public int? FeedId { get; set; }
        public string Titulo { get; set; } = "";
        public string? Descripcion { get; set; }
        public string? Contenido { get; set; }
        public string? Autor { get; set; }
        public string? Imagen { get; set; }
        public string Link { get; set; } = "";

        public DateTime? FechaPublicacion { get; set; }

        // Feed
        public string FeedTitulo { get; set; } = "";
        public string FeedUrl { get; set; } = "";

        // Estado usuario
        public bool IsLiked { get; set; }
        public int? UsuarioArticuloGuardadoId { get; set; }
    }
}
