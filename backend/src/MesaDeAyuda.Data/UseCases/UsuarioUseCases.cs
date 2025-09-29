using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MesaDeAyuda.Data.Interfaces.UseCases;
using MesaDeAyuda.Data.Persistency.Contexts;
using MesaDeAyuda.Domain.Entities;
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
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
    {
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

        existingUsuario.Nombre = usuario.Nombre;
        existingUsuario.Email = usuario.Email;
        existingUsuario.Rol = usuario.Rol;
        if (!string.IsNullOrEmpty(usuario.Contrasenia))
            existingUsuario.Contrasenia = usuario.Contrasenia;

        await _context.SaveChangesAsync();
        return existingUsuario;
    }

    public async Task<bool> DeleteUsuarioAsync(string rut)
    {
        var usuario = await _context.Usuarios.FindAsync(rut);
        if (usuario == null)
            return false;

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return true;
    }
}
