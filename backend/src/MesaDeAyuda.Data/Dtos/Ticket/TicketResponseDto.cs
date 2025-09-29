using MesaDeAyuda.Domain.Enums;
using System;

namespace MesaDeAyuda.Data.Dtos.Ticket;

public class TicketResponseDto : TicketDto
{
    public int Id { get; set; }
    public required Tipo Tipo { get; set; }
    public required Estado Estado { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaResolucion { get; set; }
    public TicketUsuarioResponseDto UsuarioRutCreadorNavigation { get; set; } = null!;
    public TicketUsuarioResponseDto? UsuarioRutTecnicoNavigation { get; set; }

    public class TicketUsuarioResponseDto
    {
        public required string Rut { get; set; }
        public required string Nombre { get; set; }
    }
}
