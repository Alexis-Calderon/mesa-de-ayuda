using MesaDeAyuda.Domain.Enums;

namespace MesaDeAyuda.Data.Dtos.Ticket;

public class TicketResponseDto : TicketDto
{
    public int Id { get; set; }
    public required Tipo Tipo { get; set; }
    public required Estado Estado { get; set; }
    public TicketUsuarioResponseDto UsuarioRutCreadorNavigation { get; set; } = null!;
    public TicketUsuarioResponseDto UsuarioRutTecnicoNavigation { get; set; } = null!;

    public class TicketUsuarioResponseDto
    {
        public required string Rut { get; set; }

        public required string Nombre { get; set; }
    }
}
