using System.ComponentModel.DataAnnotations;
using MesaDeAyuda.Data.Common.Helpers;

namespace MesaDeAyuda.Data.Dtos.Contracts;

public class ResetContraseniaDto
{
    [Required(ErrorMessage = ErrorMessages.RutRequired)]
    [MaxLength(12, ErrorMessage = ErrorMessages.RutMaxLength)]
    public required string Rut { get; set; }

    [Required(ErrorMessage = ErrorMessages.CurrentContraseniaRequired)]
    [MinLength(8, ErrorMessage = ErrorMessages.CurrentContraseniaMinLength)]
    public required string CurrentContrasenia { get; set; }

    [Required(ErrorMessage = ErrorMessages.NewContraseniaRequired)]
    [MinLength(8, ErrorMessage = ErrorMessages.NewContraseniaMinLength)]
    [RegularExpression(
        RegularExpresions.Contrasenia,
        ErrorMessage = ErrorMessages.NewContraseniaInvalidFormat
    )]
    public required string NewContrasenia { get; set; }

    [Required(ErrorMessage = ErrorMessages.ConfirmNewContraseniaRequired)]
    [MinLength(8, ErrorMessage = ErrorMessages.ConfirmNewContraseniaMinLength)]
    public required string ConfirmNewContrasenia { get; set; }

    public static class ErrorMessages
    {
        // Rut
        public const string RutRequired = "El RUT es obligatorio";
        public const string RutMaxLength = "El RUT no puede tener más de 12 caracteres";

        // CurrentContrasenia
        public const string CurrentContraseniaRequired = "La contraseña actual es obligatoria";
        public const string CurrentContraseniaMinLength =
            "La contraseña actual debe tener al menos 8 caracteres";

        // NewContrasenia
        public const string NewContraseniaRequired = "La nueva contraseña es obligatoria";
        public const string NewContraseniaMinLength =
            "La nueva contraseña debe tener al menos 8 caracteres";
        public const string NewContraseniaInvalidFormat =
            "La nueva contraseña debe contener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial";

        // ConfirmNewContrasenia
        public const string ConfirmNewContraseniaRequired =
            "La confirmación de la nueva contraseña es obligatoria";
        public const string ConfirmNewContraseniaMinLength =
            "La confirmación de la nueva contraseña debe tener al menos 8 caracteres";
    }
}
