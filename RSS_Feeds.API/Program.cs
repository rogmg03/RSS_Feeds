using Microsoft.EntityFrameworkCore;
using RSS_Feeds.Core.BusinessLogic;
using RSS_Feeds.Data.Models;
using RSS_Feeds.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ========= Business / Repositories =========

// Usuarios
builder.Services.AddScoped<IRepositoryUsuario, RepositoryUsuario>();
builder.Services.AddScoped<IUsuarioBusiness, UsuarioBusiness>();

// Feeds
builder.Services.AddScoped<IRepositoryFeed, RepositoryFeed>();
builder.Services.AddScoped<IFeedBusiness, FeedBusiness>();

// Artículos
builder.Services.AddScoped<IRepositoryArticulo, RepositoryArticulo>();
builder.Services.AddScoped<IArticuloBusiness, ArticuloBusiness>();

// Relación usuario-feeds
builder.Services.AddScoped<IRepositoryUsuarioFeed, RepositoryUsuarioFeed>();
builder.Services.AddScoped<IUsuarioFeedBusiness, UsuarioFeedBusiness>();

// Artículos guardados
builder.Services.AddScoped<IRepositoryUsuarioArticulosGuardado, RepositoryUsuarioArticulosGuardado>();
builder.Services.AddScoped<IUsuarioArticulosGuardadoBusiness, UsuarioArticulosGuardadoBusiness>();

// ========= DbContext =========

builder.Services.AddDbContext<RssDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
