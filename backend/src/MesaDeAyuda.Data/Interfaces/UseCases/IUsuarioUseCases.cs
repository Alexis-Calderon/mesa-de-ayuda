using System.Collections.Generic;
using System.Threading.Tasks;
using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Data.Interfaces.UseCases;

public interface IUsuarioUseCases
{
    Task<Usuario?> GetUsuarioByRutAsync(string rut);
    Task<IEnumerable<Usuario>> GetAllUsuariosAsync();
    Task<Usuario> CreateUsuarioAsync(Usuario usuario);
    Task<Usuario?> UpdateUsuarioAsync(string rut, Usuario usuario);
    Task<bool> DeleteUsuarioAsync(string rut);
}
