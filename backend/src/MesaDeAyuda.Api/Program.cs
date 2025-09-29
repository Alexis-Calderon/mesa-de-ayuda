using System.Text;
using MesaDeAyuda.Api.Services;
using MesaDeAyuda.Data.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddData(builder.Environment);

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
        }
    );

builder
    .Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireRole("Administrador"))
    .AddPolicy("TecnicoOrAdmin", policy => policy.RequireRole("Técnico", "Administrador"))
    .AddPolicy(
        "ClienteOrTecnicoOrAdmin",
        policy => policy.RequireRole("Cliente", "Técnico", "Administrador")
    );

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "Mesa de Ayuda API v1")
    );
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Aplicar migraciones y ejecutar seeder de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context =
        services.GetRequiredService<MesaDeAyuda.Data.Persistency.Contexts.MesaDeAyudaContext>();

    // Crear base de datos si no existe (SQLite)
    await context.Database.EnsureCreatedAsync();

    // Ejecutar seeder de datos
    var seeder = new DataSeeder(context);
    await seeder.SeedAsync();
}

app.Run();
