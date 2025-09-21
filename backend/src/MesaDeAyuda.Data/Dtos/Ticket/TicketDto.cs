using System.ComponentModel.DataAnnotations;
using MesaDeAyuda.Domain.Enums;

namespace MesaDeAyuda.Data.Dtos.Ticket;

public abstract class TicketDto
{
    [Required(ErrorMessage = ErrorMessages.PrioridadRequerida)]
    [EnumDataType(typeof(Prioridad), ErrorMessage = ErrorMessages.PrioridadInvalida)]
    public required Prioridad Prioridad { get; set; }

    [Required(ErrorMessage = ErrorMessages.AreaRequerida)]
    [EnumDataType(typeof(Area), ErrorMessage = ErrorMessages.AreaInvalida)]
    public required Area Area { get; set; }

    [Required(ErrorMessage = ErrorMessages.DescripcionRequerida)]
    [MaxLength(100, ErrorMessage = ErrorMessages.DescripcionMaxLength)]
    public required string Descripcion { get; set; }

    [Required(ErrorMessage = ErrorMessages.ObservacionesRequeridas)]
    [MaxLength(500, ErrorMessage = ErrorMessages.ObservacionesMaxLength)]
    public required string Observaciones { get; set; }

    public static class ErrorMessages
    {
        // Tipo
        public const string TipoRequerido = "El tipo es requerido.";
        public const string TipoInvalido = "El tipo no es válido.";

        // Prioridad
        public const string PrioridadRequerida = "La prioridad es requerida.";
        public const string PrioridadInvalida = "La prioridad no es válida.";

        // Area
        public const string AreaRequerida = "El área es requerida.";
        public const string AreaInvalida = "El área no es válida.";

        // Estado
        public const string EstadoRequerido = "El estado es requerido.";
        public const string EstadoInvalido = "El estado no es válido.";

        // Descripcion
        public const string DescripcionRequerida = "La descripción es requerida.";
        public const string DescripcionMaxLength =
            "La descripción no puede exceder los 100 caracteres.";

        // Observaciones
        public const string ObservacionesRequeridas = "Las observaciones son requeridas.";
        public const string ObservacionesMaxLength =
            "Las observaciones no pueden exceder los 500 caracteres.";
    }
}
