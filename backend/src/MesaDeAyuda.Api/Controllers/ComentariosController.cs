using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using MesaDeAyuda.Data.Dtos.Comentario;
using MesaDeAyuda.Data.Interfaces.UseCases;
using MesaDeAyuda.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MesaDeAyuda.Api.Controllers;

[ApiController]
[Route("api/tickets/{ticketId}/comentarios")]
[Authorize]
public class ComentariosController(
    IComentarioUseCases comentarioUseCases,
    ITicketUseCases ticketUseCases
    ) : ControllerBase
{
    private readonly IComentarioUseCases _comentarioUseCases = comentarioUseCases;
    private readonly ITicketUseCases _ticketUseCases = ticketUseCases;

    [HttpGet]
    public async Task<IActionResult> GetComentarios(int ticketId)
    {
        // Verificar que el usuario tenga acceso al ticket
        var rut = User.FindFirst("rut")?.Value;
        var rol = User.FindFirst(ClaimTypes.Role)?.Value;

        var ticket = await _ticketUseCases.GetTicketByIdAsync(ticketId);
        if (ticket == null)
            return NotFound("Ticket no encontrado");

        if (
            rol != "Administrador"
            && ticket.UsuarioRutCreador != rut
            && ticket.UsuarioRutTecnico != rut
        )
        {
            return Forbid();
        }

        var comentarios = await _comentarioUseCases.GetComentariosByTicketAsync(ticketId);
        var response = comentarios.Adapt<List<ComentarioResponseDto>>();

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateComentario(
        int ticketId,
        [FromBody] ComentarioCreateDto dto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Verificar que el usuario tenga acceso al ticket
        var rut = User.FindFirst("rut")?.Value;
        var rol = User.FindFirst(ClaimTypes.Role)?.Value;

        var ticket = await _ticketUseCases.GetTicketByIdAsync(ticketId);
        if (ticket == null)
            return NotFound("Ticket no encontrado");

        if (
            rol != "Administrador"
            && ticket.UsuarioRutCreador != rut
            && ticket.UsuarioRutTecnico != rut
        )
        {
            return Forbid();
        }

        var comentario = new Comentario
        {
            TicketId = ticketId,
            UsuarioRut = rut!,
            Contenido = dto.Contenido,
        };

        var created = await _comentarioUseCases.CreateComentarioAsync(comentario);
        var response = created.Adapt<ComentarioResponseDto>();

        return CreatedAtAction(nameof(GetComentarios), new { ticketId }, response);
    }
}
