using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS_Feeds.Models.DTOs
{
    public class LikeArticleRequestDTO
    {
        public string Titulo { get; set; } = null!;
        public string Link { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Contenido { get; set; }
        public string? Autor { get; set; }
        public string? Imagen { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public int FeedId { get; set; }
    }
}
