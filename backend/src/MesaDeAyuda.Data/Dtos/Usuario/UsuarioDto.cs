using System.ComponentModel.DataAnnotations;
using MesaDeAyuda.Domain.Enums;

namespace MesaDeAyuda.Data.Dtos.Usuario;

public abstract class UsuarioDto
{
    [Required(ErrorMessage = ErrorMessages.NombreRequerido)]
    [MaxLength(100, ErrorMessage = ErrorMessages.NombreMaxLength)]
    public required string Nombre { get; set; }

    [Required(ErrorMessage = ErrorMessages.EmailRequerido)]
    [MaxLength(100, ErrorMessage = ErrorMessages.EmailMaxLength)]
    [EmailAddress(ErrorMessage = ErrorMessages.EmailInvalido)]
    public required string Email { get; set; }

    [Required(ErrorMessage = ErrorMessages.RolRequerido)]
    [EnumDataType(typeof(Rol), ErrorMessage = ErrorMessages.RolInvalido)]
    public required Rol Rol { get; set; }

    public static class ErrorMessages
    {
        // Rut
        public const string RutRequerido = "El RUT es requerido.";
        public const string RutMaxLength = "El RUT no puede exceder los 12 caracteres.";
        public const string RutInvalido = "El RUT no es válido.";

        // Nombre
        public const string NombreRequerido = "El nombre es requerido.";
        public const string NombreMaxLength = "El nombre no puede exceder los 100 caracteres.";

        // Email
        public const string EmailRequerido = "El email es requerido.";
        public const string EmailMaxLength = "El email no puede exceder los 100 caracteres.";
        public const string EmailInvalido = "El email no es válido.";

        // Rol
        public const string RolRequerido = "El rol es requerido.";
        public const string RolInvalido = "El rol no es válido.";
    }
}
