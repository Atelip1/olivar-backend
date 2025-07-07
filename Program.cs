using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OlivarBackend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OlivarBackend.Services;
using OlivarBackend.Config; // <- Asegúrate de tener tu clase JwtSettings aquí
using System.Text;
using PdfSharpCore.Fonts;       // Para usar GlobalFontSettings
using OlivarBackend.Services;  // Donde está tu CustomFontResolver




var builder = WebApplication.CreateBuilder(args);
GlobalFontSettings.FontResolver = new CustomFontResolver();

// 🔐 JWT
var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSection); // Permite inyectar JwtSettings en TokenService

var jwtSettings = jwtSection.Get<JwtSettings>(); // ✅ Obtiene los valores reales

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

// ✅ Registrar servicios personalizados
builder.Services.AddScoped<TokenService>();

// 🔧 Agregar servicios de controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Olivar API", Version = "v1" });
});

// 🔗 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 🔌 DbContext
builder.Services.AddDbContext<RestauranteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// ✅ Establecer puerto en Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

// ✅ Verificar conexión a BD
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RestauranteDbContext>();
    try
    {
        if (!db.Database.CanConnect())
            throw new Exception("No se pudo conectar a la base de datos.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error de conexión a BD: {ex.Message}");
        throw;
    }
}

// Middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Olivar API V1");
    c.RoutePrefix = "swagger";
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// ⚠️ HTTPS solo en desarrollo
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseCors("AllowAll");
app.UseAuthentication(); // ✅ JWT
app.UseAuthorization();

app.MapControllers();

// Ruta para verificar que está activo
app.MapGet("/", () => "🚀 Backend Olivar activo en Render.");

// Ruta para errores generales
app.Map("/Error", (HttpContext httpContext) =>
{
    return Results.Problem("Ha ocurrido un error inesperado.");
});

app.Run();
