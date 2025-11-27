using System;
using System.Collections.Generic;

namespace RSS_Feeds.Data.Models;

public partial class Articulo
{
    public int Id { get; set; }

    public int FeedId { get; set; }

    public string Titulo { get; set; } = null!;

    public string Link { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Contenido { get; set; }

    public string? Autor { get; set; }

    public string? Imagen { get; set; }

    public DateTime? FechaPublicacion { get; set; }

    public DateTime? CreadoEn { get; set; }

    public virtual Feed Feed { get; set; } = null!;

    public virtual ICollection<UsuarioArticulosGuardado> UsuarioArticulosGuardados { get; set; } = new List<UsuarioArticulosGuardado>();
}
