using RSS_Feeds.Architecture;
using RSS_Feeds.Architecture.Providers;
using RSS_Feeds.ServiceLocator.Services;
using RSS_Feeds.ServiceLocator.Services.Contracts;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HTTP + Rest provider
builder.Services.AddHttpClient();
builder.Services.AddScoped<IRestProvider, RestProvider>();

// Services
builder.Services.AddScoped<IFeedService, FeedService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IArticuloService, ArticuloService>();
builder.Services.AddScoped<IUsuarioFeedService, UsuarioFeedService>();
builder.Services.AddScoped<IUsuarioArticulosGuardadoService, UsuarioArticulosGuardadoService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services
    .AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });


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
