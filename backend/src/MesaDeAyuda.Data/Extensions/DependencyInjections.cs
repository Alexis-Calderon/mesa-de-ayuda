using MesaDeAyuda.Data.Interfaces.UseCases;
using MesaDeAyuda.Data.Persistency.Contexts;
using MesaDeAyuda.Data.UseCases;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace MesaDeAyuda.Data.Extensions;

public static class DependencyInjections
{
    public static IServiceCollection AddData(this IServiceCollection services, IHostEnvironment env)
    {
        string dbPath = Path.Combine(env.ContentRootPath, "App_Data", "mesa_de_ayuda.db");
        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

        SqliteConnectionStringBuilder connectionStringBuilder = new() { DataSource = dbPath };

        services.AddDbContext<MesaDeAyudaContext>(options =>
            options.UseSqlite(connectionStringBuilder.ToString())
        );

        services.AddScoped<ITicketUseCases, TicketUseCases>();
        services.AddScoped<IUsuarioUseCases, UsuarioUseCases>();
        services.AddScoped<IComentarioUseCases, ComentarioUseCases>();

        return services;
    }
}
