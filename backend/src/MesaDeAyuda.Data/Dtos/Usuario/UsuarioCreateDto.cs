using System.ComponentModel.DataAnnotations;
using MesaDeAyuda.Data.Common.Helpers;

namespace MesaDeAyuda.Data.Dtos.Usuario;

public class UsuarioCreateDto : UsuarioDto
{
    [Required(ErrorMessage = ErrorMessages.RutRequerido)]
    [MaxLength(12, ErrorMessage = ErrorMessages.RutMaxLength)]
    [RegularExpression(RegularExpresions.Rut, ErrorMessage = ErrorMessages.RutInvalido)]
    public required string Rut { get; set; }

    [Required(ErrorMessage = "La contraseña es requerida.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public required string Contrasenia { get; set; }
}
