using System.ComponentModel.DataAnnotations;

namespace MesaDeAyuda.Data.Dtos.Contracts;

public class RefreshTokenDto
{
    [Required(ErrorMessage = ErrorMessages.TokenRequired)]
    public required string Token { get; set; }

    [Required(ErrorMessage = ErrorMessages.RefreshTokenRequired)]
    public required string RefreshToken { get; set; }

    private static class ErrorMessages
    {
        public const string TokenRequired = "El token es obligatorio";
        public const string RefreshTokenRequired = "El refresh token es obligatorio";
    }
}
