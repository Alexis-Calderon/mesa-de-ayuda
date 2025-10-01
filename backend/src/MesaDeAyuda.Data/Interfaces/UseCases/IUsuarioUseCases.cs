using System.Collections.Generic;
using System.Threading.Tasks;
using MesaDeAyuda.Data.Dtos.Usuario;
using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Data.Interfaces.UseCases;

public interface IUsuarioUseCases
{
    Task<Usuario?> GetUsuarioByRutAsync(string rut);
    Task<IEnumerable<Usuario>> GetAllUsuariosAsync();
    Task<Usuario> CreateUsuarioAsync(Usuario usuario);
    Task<Usuario?> UpdateUsuarioAsync(string rut, Usuario usuario);
    Task<bool> DeleteUsuarioAsync(string rut);

    // Nuevos métodos para manejo de perfil
    Task<Usuario?> GetCurrentUserAsync(string rut);
    Task<Usuario?> UpdatePerfilAsync(string rut, UsuarioPerfilUpdateDto dto);
}
