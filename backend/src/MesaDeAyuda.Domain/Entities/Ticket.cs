using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MesaDeAyuda.Domain.Enums;

namespace MesaDeAyuda.Domain.Entities;

public class Ticket
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public required Tipo Tipo { get; set; }

    [Required]
    public required Prioridad Prioridad { get; set; }

    [Required]
    public required Area Area { get; set; }

    [Required]
    public required Estado Estado { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Descripcion { get; set; }

    [Required]
    [MaxLength(500)]
    public required string Observaciones { get; set; }

    [Required]
    public required string UsuarioRutCreador { get; set; } = null!;

    [Required]
    public required DateTime FechaCreacion { get; set; }
    public string? UsuarioRutTecnico { get; set; }
    public DateTime? FechaResolucion { get; set; }

    [ForeignKey(nameof(UsuarioRutCreador))]
    public Usuario UsuarioRutCreadorNavigation { get; set; } = null!;

    [ForeignKey(nameof(UsuarioRutTecnico))]
    public Usuario? UsuarioRutTecnicoNavigation { get; set; }
}
