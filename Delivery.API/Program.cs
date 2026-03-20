using Delivery.Infrastructure.Data;
using Delivery.Infrastructure.Interfaces;
using Delivery.Infrastructure.Repositories;
using Delivery.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --- Banco de dados ---
builder.Services.AddDbContext<DeliveryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Repositórios ---
builder.Services.AddScoped<IClienteRepository, ClienteRepositoryPostgres>();
builder.Services.AddScoped<IMotoristaRepository, MotoristaRepositoryPostgres>();
builder.Services.AddScoped<IVeiculoRepository, VeiculoRepositoryPostgres>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepositoryPostgres>();
builder.Services.AddScoped<IEntregaRepository, EntregaRepositoryPostgres>();

// --- Services ---
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<MotoristaService>();
builder.Services.AddScoped<VeiculoService>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<EntregaService>();

// --- Controllers ---
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// --- Swagger com API Key ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Name = "X-Api-Key",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Digite sua API Key no campo abaixo"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("MinhaPolitica", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MinhaPolitica");
app.UseHttpsRedirection();


app.Use(async (context, next) =>
{
   
    if (context.Request.Path.StartsWithSegments("/swagger"))
    {
        await next();
        return;
    }

    var apiKey = builder.Configuration["ApiKey"];

    if (!context.Request.Headers.TryGetValue("X-Api-Key", out var receivedKey) || receivedKey != apiKey)
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("API Key inválida ou ausente.");
        return;
    }

    await next();
});

app.MapControllers();
app.Run();