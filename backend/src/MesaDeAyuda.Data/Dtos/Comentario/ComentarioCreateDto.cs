using System.ComponentModel.DataAnnotations;

namespace MesaDeAyuda.Data.Dtos.Comentario;

public class ComentarioCreateDto
{
    [Required(ErrorMessage = "El contenido es requerido.")]
    [MaxLength(1000, ErrorMessage = "El contenido no puede exceder los 1000 caracteres.")]
    public required string Contenido { get; set; }
}