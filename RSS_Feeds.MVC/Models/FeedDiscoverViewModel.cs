namespace RSS_Feeds.MVC.Models
{
    public class FeedDiscoverViewModel
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public string? Categoria { get; set; }

        // UI state
        public bool IsFollowed { get; set; }
        public int? UsuarioFeedId { get; set; }
    }
}
