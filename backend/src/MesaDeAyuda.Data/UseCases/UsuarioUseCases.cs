using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using MesaDeAyuda.Data.Common.Helpers;
using MesaDeAyuda.Data.Dtos.Usuario;
using MesaDeAyuda.Data.Interfaces.UseCases;
using MesaDeAyuda.Data.Persistency.Contexts;
using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Data.UseCases;

public class UsuarioUseCases(MesaDeAyudaContext context) : IUsuarioUseCases
{
    private readonly MesaDeAyudaContext _context = context;

    public async Task<Usuario?> GetUsuarioByRutAsync(string rut)
    {
        return await _context.Usuarios.FindAsync(rut);
    }

    public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
    {
        return await _context.Usuarios.Where(u => u.Rol != Rol.Administrador).ToListAsync();
    }

    public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
    {
        // Validar que no se cree otro administrador si ya existe uno
        if (usuario.Rol == Rol.Administrador)
        {
            var existingAdmin = await _context.Usuarios.FirstOrDefaultAsync(u =>
                u.Rut == SystemConstants.DefaultAdminRut
            );
            if (existingAdmin != null && usuario.Rut != SystemConstants.DefaultAdminRut)
            {
                throw new InvalidOperationException(
                    "Ya existe un usuario administrador en el sistema. No se pueden crear múltiples administradores."
                );
            }
        }

        usuario.FechaCreacion = DateTime.UtcNow;
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<Usuario?> UpdateUsuarioAsync(string rut, Usuario usuario)
    {
        var existingUsuario = await _context.Usuarios.FindAsync(rut);
        if (existingUsuario == null)
            return null;

        // Prevenir cambio de rol del administrador por defecto
        if (rut == SystemConstants.DefaultAdminRut && usuario.Rol != Rol.Administrador)
        {
            throw new InvalidOperationException(
                "No se puede cambiar el rol del administrador por defecto."
            );
        }

        existingUsuario.Nombre = usuario.Nombre;
        existingUsuario.Email = usuario.Email;

        // Solo permitir cambio de rol si no es el administrador por defecto
        if (rut != SystemConstants.DefaultAdminRut)
        {
            existingUsuario.Rol = usuario.Rol;
        }

        if (!string.IsNullOrEmpty(usuario.Contrasenia))
            existingUsuario.Contrasenia = usuario.Contrasenia;

        await _context.SaveChangesAsync();
        return existingUsuario;
    }

    public async Task<bool> DeleteUsuarioAsync(string rut)
    {
        // Prevenir eliminación del administrador por defecto
        if (rut == SystemConstants.DefaultAdminRut)
        {
            throw new InvalidOperationException(
                "No se puede eliminar el usuario administrador por defecto."
            );
        }

        var usuario = await _context.Usuarios.FindAsync(rut);
        if (usuario == null)
            return false;

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return true;
    }

    // Nuevos métodos para manejo de perfil
    public async Task<Usuario?> GetCurrentUserAsync(string rut)
    {
        return await GetUsuarioByRutAsync(rut);
    }

    public async Task<Usuario?> UpdatePerfilAsync(string rut, UsuarioPerfilUpdateDto dto)
    {
        var existingUsuario = await _context.Usuarios.FindAsync(rut);
        if (existingUsuario == null)
            return null;

        // Validar cambio de contraseña si se proporciona
        if (
            !string.IsNullOrEmpty(dto.CurrentContrasenia)
            || !string.IsNullOrEmpty(dto.NewContrasenia)
            || !string.IsNullOrEmpty(dto.ConfirmNewContrasenia)
        )
        {
            // Verificar que se proporcionen todos los campos de contraseña
            if (
                string.IsNullOrEmpty(dto.CurrentContrasenia)
                || string.IsNullOrEmpty(dto.NewContrasenia)
                || string.IsNullOrEmpty(dto.ConfirmNewContrasenia)
            )
            {
                throw new InvalidOperationException(
                    "Para cambiar la contraseña debe proporcionar la contraseña actual, la nueva contraseña y la confirmación."
                );
            }

            // Verificar que las contraseñas nuevas coincidan
            if (dto.NewContrasenia != dto.ConfirmNewContrasenia)
            {
                throw new InvalidOperationException(
                    "La nueva contraseña y su confirmación no coinciden."
                );
            }

            // Verificar contraseña actual
            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentContrasenia, existingUsuario.Contrasenia))
            {
                throw new InvalidOperationException("La contraseña actual es incorrecta.");
            }

            // Actualizar contraseña
            existingUsuario.Contrasenia = BCrypt.Net.BCrypt.HashPassword(dto.NewContrasenia);
        }

        // Actualizar nombre y email
        existingUsuario.Nombre = dto.Nombre;
        existingUsuario.Email = dto.Email;

        await _context.SaveChangesAsync();
        return existingUsuario;
    }
}
