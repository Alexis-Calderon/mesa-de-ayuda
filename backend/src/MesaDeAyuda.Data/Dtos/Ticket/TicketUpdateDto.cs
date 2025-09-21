using System.ComponentModel.DataAnnotations;
using MesaDeAyuda.Domain.Enums;

namespace MesaDeAyuda.Data.Dtos.Ticket;

public class TicketUpdateDto : TicketDto
{
    [Required(ErrorMessage = ErrorMessages.EstadoRequerido)]
    [EnumDataType(typeof(Estado), ErrorMessage = ErrorMessages.EstadoInvalido)]
    public required Estado Estado { get; set; }
}
