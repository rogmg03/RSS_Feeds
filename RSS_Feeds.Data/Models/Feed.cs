using System;
using System.Collections.Generic;

namespace RSS_Feeds.Data.Models;

public partial class Feed
{
    public int Id { get; set; }

    public string Url { get; set; } = null!;

    public string? Titulo { get; set; }

    public string? Descripcion { get; set; }

    public string? Idioma { get; set; }

    public string? Categoria { get; set; }

    public DateTime? UltimaLectura { get; set; }

    public virtual ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();

    public virtual ICollection<UsuarioFeed> UsuarioFeeds { get; set; } = new List<UsuarioFeed>();
}
