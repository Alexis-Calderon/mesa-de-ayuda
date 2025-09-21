using System.ComponentModel.DataAnnotations;

namespace MesaDeAyuda.Data.Dtos.Contracts;

public class LoginDto
{
    [Required(ErrorMessage = ErrorMessages.RutRequired)]
    public required string Rut { get; set; }

    [Required(ErrorMessage = ErrorMessages.ContraseniaRequired)]
    public required string Contrasenia { get; set; }

    public static class ErrorMessages
    {
        public const string RutRequired = "El RUT es obligatorio";
        public const string ContraseniaRequired = "La contraseña es obligatoria";
    }
}
