using MesaDeAyuda.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Data.Persistency.Contexts;

public class MesaDeAyudaContext(DbContextOptions<MesaDeAyudaContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Comentario> Comentarios { get; set; }
}
