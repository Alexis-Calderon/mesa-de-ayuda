using System.ComponentModel.DataAnnotations;
using MesaDeAyuda.Domain.Enums;

namespace MesaDeAyuda.Data.Dtos.Ticket;

public class TicketCreateDto : TicketDto
{
    [Required(ErrorMessage = ErrorMessages.TipoRequerido)]
    [EnumDataType(typeof(Tipo), ErrorMessage = ErrorMessages.TipoInvalido)]
    public required Tipo Tipo { get; set; }
}
