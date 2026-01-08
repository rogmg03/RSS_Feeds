using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RSS_Feeds.Data.Models;

public partial class RssDbContext : DbContext
{
    public RssDbContext()
    {
    }

    public RssDbContext(DbContextOptions<RssDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Articulo> Articulos { get; set; }

    public virtual DbSet<Feed> Feeds { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioArticulosGuardado> UsuarioArticulosGuardados { get; set; }

    public virtual DbSet<UsuarioFeed> UsuarioFeeds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MORGON\\SQLEXPRESS;Database=rss_app;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Articulo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__articulo__3213E83F16F463D8");

            entity.ToTable("articulos");

            entity.HasIndex(e => e.Link, "UQ__articulo__A269238175C9FAB1").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Autor)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("autor");
            entity.Property(e => e.Contenido)
                .IsUnicode(false)
                .HasColumnName("contenido");
            entity.Property(e => e.CreadoEn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("creado_en");
            entity.Property(e => e.Descripcion)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaPublicacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_publicacion");
            entity.Property(e => e.FeedId).HasColumnName("feed_id");
            entity.Property(e => e.Imagen)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("imagen");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("link");
            entity.Property(e => e.Titulo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasOne(d => d.Feed).WithMany(p => p.Articulos)
                .HasForeignKey(d => d.FeedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_art_feed");
        });

        modelBuilder.Entity<Feed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__feeds__3213E83F539F0329");

            entity.ToTable("feeds");

            entity.HasIndex(e => e.Url, "UQ__feeds__DD77841745A22124").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categoria)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("categoria");
            entity.Property(e => e.Descripcion)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Idioma)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idioma");
            entity.Property(e => e.Titulo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("titulo");
            entity.Property(e => e.UltimaLectura)
                .HasColumnType("datetime")
                .HasColumnName("ultima_lectura");
            entity.Property(e => e.Url)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("url");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__usuarios__3213E83F1E2463B3");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "UQ__usuarios__AB6E6164BC7898C8").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreadoEn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("creado_en");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
        });

        modelBuilder.Entity<UsuarioArticulosGuardado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__usuario___3213E83FB58C469E");

            entity.ToTable("usuario_articulos_guardados");

            entity.HasIndex(e => new { e.UsuarioId, e.ArticuloId }, "UQ__usuario___AC4A850C11649CB0").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArticuloId).HasColumnName("articulo_id");
            entity.Property(e => e.FechaGuardado)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_guardado");
            entity.Property(e => e.Notas)
                .IsUnicode(false)
                .HasColumnName("notas");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Articulo).WithMany(p => p.UsuarioArticulosGuardados)
                .HasForeignKey(d => d.ArticuloId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_uag_art");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioArticulosGuardados)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_uag_user");
        });

        modelBuilder.Entity<UsuarioFeed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__usuario___3213E83F53F911F1");

            entity.ToTable("usuario_feeds");

            entity.HasIndex(e => new { e.UsuarioId, e.FeedId }, "UQ__usuario___A10A44341C8E183A").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Alias)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("alias");
            entity.Property(e => e.CreadoEn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("creado_en");
            entity.Property(e => e.FeedId).HasColumnName("feed_id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Feed).WithMany(p => p.UsuarioFeeds)
                .HasForeignKey(d => d.FeedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_uf_feed");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioFeeds)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_uf_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
