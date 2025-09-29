using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MesaDeAyuda.Data.Interfaces.UseCases;
using MesaDeAyuda.Data.Persistency.Contexts;
using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Data.UseCases;

public class TicketUseCases(MesaDeAyudaContext context) : ITicketUseCases
{
    private readonly MesaDeAyudaContext _context = context;

    public async Task<Ticket?> GetTicketByIdAsync(int id)
    {
        return await _context
            .Tickets.Include(t => t.UsuarioRutCreadorNavigation)
            .Include(t => t.UsuarioRutTecnicoNavigation)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
    {
        return await _context
            .Tickets.Include(t => t.UsuarioRutCreadorNavigation)
            .Include(t => t.UsuarioRutTecnicoNavigation)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByUsuarioAsync(string rutUsuario)
    {
        return await _context
            .Tickets.Where(t => t.UsuarioRutCreador == rutUsuario)
            .Include(t => t.UsuarioRutCreadorNavigation)
            .Include(t => t.UsuarioRutTecnicoNavigation)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByTecnicoAsync(string rutTecnico)
    {
        return await _context
            .Tickets.Where(t => t.UsuarioRutTecnico == rutTecnico)
            .Include(t => t.UsuarioRutCreadorNavigation)
            .Include(t => t.UsuarioRutTecnicoNavigation)
            .ToListAsync();
    }

    public async Task<Ticket> CreateTicketAsync(Ticket ticket)
    {
        ticket.FechaCreacion = DateTime.UtcNow;
        ticket.Estado = Estado.Abierto;
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task<Ticket?> UpdateTicketAsync(int id, Ticket ticket)
    {
        var existing = await _context.Tickets.FindAsync(id);
        if (existing == null)
            return null;

        existing.Tipo = ticket.Tipo;
        existing.Prioridad = ticket.Prioridad;
        existing.Area = ticket.Area;
        existing.Estado = ticket.Estado;
        existing.Descripcion = ticket.Descripcion;
        existing.Observaciones = ticket.Observaciones;
        existing.UsuarioRutTecnico = ticket.UsuarioRutTecnico;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteTicketAsync(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
            return false;

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Ticket?> AssignTecnicoAsync(int id, string rutTecnico)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
            return null;

        ticket.UsuarioRutTecnico = rutTecnico;
        ticket.Estado = Estado.Revisión;
        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task<Ticket?> ResolveTicketAsync(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
            return null;

        ticket.Estado = Estado.Resuelto;
        ticket.FechaResolucion = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return ticket;
    }
}
