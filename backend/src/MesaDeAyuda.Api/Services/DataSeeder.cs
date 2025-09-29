using System;
using System.Threading.Tasks;
using BCrypt.Net;
using MesaDeAyuda.Data.Persistency.Contexts;
using MesaDeAyuda.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Api.Services;

public class DataSeeder
{
    private readonly MesaDeAyudaContext _context;

    public DataSeeder(MesaDeAyudaContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        try
        {
            // Verificar si el usuario administrador ya existe
            var existingAdmin = await _context.Usuarios.FirstOrDefaultAsync(u => u.Rut == "11111111-1");
            if (existingAdmin == null)
            {
                var adminUser = new MesaDeAyuda.Domain.Entities.Usuario
                {
                    Rut = "11111111-1",
                    Nombre = "Administrador Sistema",
                    Email = "admin@mesaayuda.com",
                    Rol = Rol.Administrador,
                    Contrasenia = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    FechaCreacion = DateTime.UtcNow,
                };

                await _context.Usuarios.AddAsync(adminUser);
                await _context.SaveChangesAsync();
                Console.WriteLine("Usuario administrador creado exitosamente");
            }
            else
            {
                Console.WriteLine("Usuario administrador ya existe");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en DataSeeder: {ex.Message}");
            throw;
        }
    }
}
