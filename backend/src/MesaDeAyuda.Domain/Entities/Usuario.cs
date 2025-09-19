using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MesaDeAyuda.Domain.Enums;

namespace MesaDeAyuda.Domain.Entities;

[Table("Usuario", Schema = "MesaDeAyuda")]
public class Usuario
{
    [Key]
    [Required]
    [MaxLength(12)]
    public required string Rut { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Nombre { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; }

    [Required]
    public required Rol Rol { get; set; }

    [InverseProperty(nameof(Ticket.UsuarioRutCreadorNavigation))]
    public List<Ticket> ListTicketCreado { get; set; } = [];

    [InverseProperty(nameof(Ticket.UsuarioRutTecnicoNavigation))]
    public List<Ticket> ListTicketTecnico { get; set; } = [];
}
