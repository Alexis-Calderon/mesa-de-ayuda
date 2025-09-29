using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MesaDeAyuda.Domain.Entities;

[Table("Comentario", Schema = "MesaDeAyuda")]
public class Comentario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int TicketId { get; set; }

    [Required]
    [MaxLength(12)]
    public required string UsuarioRut { get; set; }

    [Required]
    [MaxLength(1000)]
    public required string Contenido { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TicketId))]
    public Ticket Ticket { get; set; } = null!;

    [ForeignKey(nameof(UsuarioRut))]
    public Usuario Usuario { get; set; } = null!;
}