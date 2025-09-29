using System.Collections.Generic;
using System.Threading.Tasks;
using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Data.Interfaces.UseCases;

public interface ITicketUseCases
{
    Task<Ticket?> GetTicketByIdAsync(int id);
    Task<IEnumerable<Ticket>> GetAllTicketsAsync();
    Task<IEnumerable<Ticket>> GetTicketsByUsuarioAsync(string rutUsuario);
    Task<IEnumerable<Ticket>> GetTicketsByTecnicoAsync(string rutTecnico);
    Task<Ticket> CreateTicketAsync(Ticket ticket);
    Task<Ticket?> UpdateTicketAsync(int id, Ticket ticket);
    Task<bool> DeleteTicketAsync(int id);
    Task<Ticket?> AssignTecnicoAsync(int id, string rutTecnico);
    Task<Ticket?> ResolveTicketAsync(int id);
}
