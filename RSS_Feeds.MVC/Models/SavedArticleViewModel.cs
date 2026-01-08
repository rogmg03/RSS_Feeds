namespace RSS_Feeds.MVC.Models
{
    public class SavedArticleViewModel
    {
        // Artículo
        public int ArticuloId { get; set; }
        public string Titulo { get; set; } = "";
        public string? Descripcion { get; set; }
        public string? Contenido { get; set; }
        public string? Autor { get; set; }
        public string? Imagen { get; set; }
        public string Link { get; set; } = "";
        public DateTime? FechaPublicacion { get; set; }

        // Feed
        public string FeedTitulo { get; set; } = "";

        // Usuario
        public int UsuarioArticuloGuardadoId { get; set; }
        public DateTime? FechaGuardado { get; set; }
        public string? Notas { get; set; }
    }
}
