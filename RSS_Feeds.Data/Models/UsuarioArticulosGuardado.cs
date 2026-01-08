using System;
using System.Collections.Generic;

namespace RSS_Feeds.Data.Models;

public partial class UsuarioArticulosGuardado
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int ArticuloId { get; set; }

    public DateTime? FechaGuardado { get; set; }

    public string? Notas { get; set; }

    public virtual Articulo? Articulo { get; set; } = null!;

    public virtual Usuario? Usuario { get; set; } = null!;
}
