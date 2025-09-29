using System.Collections.Generic;
using System.Threading.Tasks;
using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Data.Interfaces.UseCases;

public interface IComentarioUseCases
{
    Task<IEnumerable<Comentario>> GetComentariosByTicketAsync(int ticketId);
    Task<Comentario> CreateComentarioAsync(Comentario comentario);
}