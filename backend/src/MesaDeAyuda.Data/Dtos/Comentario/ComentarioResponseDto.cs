using System;

namespace MesaDeAyuda.Data.Dtos.Comentario;

public class ComentarioResponseDto
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public required string UsuarioRut { get; set; }
    public required string UsuarioNombre { get; set; }
    public required string Contenido { get; set; }
    public DateTime FechaCreacion { get; set; }
}