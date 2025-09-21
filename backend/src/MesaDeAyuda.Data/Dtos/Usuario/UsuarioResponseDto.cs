namespace MesaDeAyuda.Data.Dtos.Usuario;

public class UsuarioResponseDto : UsuarioDto
{
    public string Rut { get; set; } = null!;
    public DateTime FechaCreacion { get; set; }
}
