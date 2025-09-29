using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MesaDeAyuda.Data.Interfaces.UseCases;
using MesaDeAyuda.Data.Persistency.Contexts;
using MesaDeAyuda.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Data.UseCases;

public class ComentarioUseCases(MesaDeAyudaContext context) : IComentarioUseCases
{
    private readonly MesaDeAyudaContext _context = context;

    public async Task<IEnumerable<Comentario>> GetComentariosByTicketAsync(int ticketId)
    {
        return await _context.Comentarios
            .Where(c => c.TicketId == ticketId)
            .Include(c => c.Usuario)
            .OrderBy(c => c.FechaCreacion)
            .ToListAsync();
    }

    public async Task<Comentario> CreateComentarioAsync(Comentario comentario)
    {
        comentario.FechaCreacion = DateTime.UtcNow;
        _context.Comentarios.Add(comentario);
        await _context.SaveChangesAsync();
        return comentario;
    }
}