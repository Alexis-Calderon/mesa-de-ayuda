using System;
using System.Threading.Tasks;
using BCrypt.Net;
using MesaDeAyuda.Data.Common.Helpers;
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
            await EnsureDefaultAdminExistsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en DataSeeder: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Garantiza que siempre exista el usuario administrador por defecto.
    /// Si no existe, lo crea. Si existe pero tiene datos incorrectos, lo corrige.
    /// </summary>
    private async Task EnsureDefaultAdminExistsAsync()
    {
        var existingAdmin = await _context.Usuarios.FirstOrDefaultAsync(u =>
            u.Rut == SystemConstants.DefaultAdminRut
        );

        if (existingAdmin == null)
        {
            // Crear administrador por defecto
            var adminUser = new MesaDeAyuda.Domain.Entities.Usuario
            {
                Rut = SystemConstants.DefaultAdminRut,
                Nombre = SystemConstants.DefaultAdminNombre,
                Email = SystemConstants.DefaultAdminEmail,
                Rol = Rol.Administrador,
                Contrasenia = BCrypt.Net.BCrypt.HashPassword(
                    SystemConstants.DefaultAdminContrasenia
                ),
                FechaCreacion = DateTime.UtcNow,
            };

            await _context.Usuarios.AddAsync(adminUser);
            await _context.SaveChangesAsync();
            Console.WriteLine("Usuario administrador por defecto creado exitosamente");
        }
        else
        {
            // Verificar y corregir datos del administrador si es necesario
            var needsUpdate = false;

            if (existingAdmin.Nombre != SystemConstants.DefaultAdminNombre)
            {
                existingAdmin.Nombre = SystemConstants.DefaultAdminNombre;
                needsUpdate = true;
            }

            // El email del administrador ahora puede ser modificado libremente
            // No corregir automáticamente el email

            if (existingAdmin.Rol != Rol.Administrador)
            {
                existingAdmin.Rol = Rol.Administrador;
                needsUpdate = true;
            }

            // Resetear contraseña si es necesario (opcional, solo si se detecta problema)
            if (string.IsNullOrEmpty(existingAdmin.Contrasenia))
            {
                existingAdmin.Contrasenia = BCrypt.Net.BCrypt.HashPassword(
                    SystemConstants.DefaultAdminContrasenia
                );
                needsUpdate = true;
                Console.WriteLine("Contraseña del administrador reseteada a valor por defecto");
            }

            if (needsUpdate)
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("Datos del administrador por defecto actualizados");
            }
            else
            {
                Console.WriteLine(
                    "Administrador por defecto ya existe y está correctamente configurado"
                );
            }
        }
    }
}
