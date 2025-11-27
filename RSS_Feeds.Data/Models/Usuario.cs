using System;
using System.Collections.Generic;

namespace RSS_Feeds.Data.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreadoEn { get; set; }

    public virtual ICollection<UsuarioArticulosGuardado> UsuarioArticulosGuardados { get; set; } = new List<UsuarioArticulosGuardado>();

    public virtual ICollection<UsuarioFeed> UsuarioFeeds { get; set; } = new List<UsuarioFeed>();
}
