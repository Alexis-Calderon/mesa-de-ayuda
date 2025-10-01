using System.ComponentModel.DataAnnotations;

namespace MesaDeAyuda.Data.Dtos.Usuario;

public class UsuarioPerfilUpdateDto
{
    [Required(ErrorMessage = ErrorMessages.NombreRequerido)]
    [MaxLength(100, ErrorMessage = ErrorMessages.NombreMaxLength)]
    public required string Nombre { get; set; }

    [Required(ErrorMessage = ErrorMessages.EmailRequerido)]
    [MaxLength(100, ErrorMessage = ErrorMessages.EmailMaxLength)]
    [EmailAddress(ErrorMessage = ErrorMessages.EmailInvalido)]
    public required string Email { get; set; }

    // Campos opcionales para cambio de contraseña
    [MinLength(8, ErrorMessage = ErrorMessages.CurrentContraseniaMinLength)]
    public string? CurrentContrasenia { get; set; }

    [MinLength(8, ErrorMessage = ErrorMessages.NewContraseniaMinLength)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = ErrorMessages.NewContraseniaInvalidFormat)]
    public string? NewContrasenia { get; set; }

    [MinLength(8, ErrorMessage = ErrorMessages.ConfirmNewContraseniaMinLength)]
    public string? ConfirmNewContrasenia { get; set; }

    public static class ErrorMessages
    {
        // Nombre
        public const string NombreRequerido = "El nombre es requerido.";
        public const string NombreMaxLength = "El nombre no puede exceder los 100 caracteres.";

        // Email
        public const string EmailRequerido = "El email es requerido.";
        public const string EmailMaxLength = "El email no puede exceder los 100 caracteres.";
        public const string EmailInvalido = "El email no es válido.";

        // CurrentContrasenia
        public const string CurrentContraseniaMinLength = "La contraseña actual debe tener al menos 8 caracteres.";

        // NewContrasenia
        public const string NewContraseniaMinLength = "La nueva contraseña debe tener al menos 8 caracteres.";
        public const string NewContraseniaInvalidFormat = "La nueva contraseña debe contener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.";

        // ConfirmNewContrasenia
        public const string ConfirmNewContraseniaMinLength = "La confirmación de la nueva contraseña debe tener al menos 8 caracteres.";
    }
}