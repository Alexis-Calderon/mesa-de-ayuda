using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using MesaDeAyuda.Data.Dtos.Ticket;
using MesaDeAyuda.Data.Interfaces.UseCases;
using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MesaDeAyuda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ITicketUseCases _ticketUseCases;
    private readonly IUsuarioUseCases _usuarioUseCases;

    public TicketsController(ITicketUseCases ticketUseCases, IUsuarioUseCases usuarioUseCases)
    {
        _ticketUseCases = ticketUseCases;
        _usuarioUseCases = usuarioUseCases;
    }

    [HttpGet]
    public async Task<IActionResult> GetTickets()
    {
        var rut = User.FindFirst("rut")?.Value;
        var rol = User.FindFirst(ClaimTypes.Role)?.Value;

        IEnumerable<Ticket> tickets;

        if (rol == Rol.Administrador.ToString())
        {
            tickets = await _ticketUseCases.GetAllTicketsAsync();
        }
        else if (rol == Rol.Técnico.ToString())
        {
            tickets = await _ticketUseCases.GetTicketsByTecnicoAsync(rut!);
        }
        else // Cliente
        {
            tickets = await _ticketUseCases.GetTicketsByUsuarioAsync(rut!);
        }

        var response = tickets.Adapt<List<TicketResponseDto>>();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicket(int id)
    {
        var ticket = await _ticketUseCases.GetTicketByIdAsync(id);
        if (ticket == null)
            return NotFound("Ticket no encontrado");

        var rut = User.FindFirst("rut")?.Value;
        var rol = User.FindFirst(ClaimTypes.Role)?.Value;

        // Verificar permisos
        if (
            rol != nameof(Rol.Administrador)
            && ticket.UsuarioRutCreador != rut
            && ticket.UsuarioRutTecnico != rut
        )
        {
            return Forbid();
        }

        var response = ticket.Adapt<TicketResponseDto>();
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "ClienteOrTecnicoOrAdmin")]
    public async Task<IActionResult> CreateTicket([FromBody] TicketCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var rut = User.FindFirst("rut")?.Value;
        var ticket = dto.Adapt<Ticket>();
        ticket.UsuarioRutCreador = rut!;

        var created = await _ticketUseCases.CreateTicketAsync(ticket);
        var response = created.Adapt<TicketResponseDto>();

        return CreatedAtAction(nameof(GetTicket), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "TecnicoOrAdmin")]
    public async Task<IActionResult> UpdateTicket(int id, [FromBody] TicketUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var ticket = await _ticketUseCases.GetTicketByIdAsync(id);
        if (ticket == null)
            return NotFound("Ticket no encontrado");

        var rut = User.FindFirst("rut")?.Value;
        var rol = User.FindFirst(ClaimTypes.Role)?.Value;

        // Solo técnico asignado o admin puede actualizar
        if (rol != nameof(Rol.Administrador) && ticket.UsuarioRutTecnico != rut)
            return Forbid();

        var update = dto.Adapt<Ticket>();
        var updated = await _ticketUseCases.UpdateTicketAsync(id, update);

        if (updated == null)
            return NotFound("Ticket no encontrado");

        var response = updated.Adapt<TicketResponseDto>();
        return Ok(response);
    }

    [HttpPut("{id}/asignar")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AssignTecnico(int id, [FromBody] AssignTecnicoDto dto)
    {
        var ticket = await _ticketUseCases.AssignTecnicoAsync(id, dto.RutTecnico);
        if (ticket == null)
            return NotFound("Ticket no encontrado");

        var response = ticket.Adapt<TicketResponseDto>();
        return Ok(response);
    }

    [HttpPut("{id}/resolver")]
    [Authorize(Policy = "TecnicoOrAdmin")]
    public async Task<IActionResult> ResolveTicket(int id)
    {
        var ticket = await _ticketUseCases.GetTicketByIdAsync(id);
        if (ticket == null)
            return NotFound("Ticket no encontrado");

        var rut = User.FindFirst("rut")?.Value;
        var rol = User.FindFirst(ClaimTypes.Role)?.Value;

        // Solo técnico asignado o admin puede resolver
        if (rol != nameof(Rol.Administrador) && ticket.UsuarioRutTecnico != rut)
            return Forbid();

        var resolved = await _ticketUseCases.ResolveTicketAsync(id);
        if (resolved == null)
            return NotFound("Ticket no encontrado");

        var response = resolved.Adapt<TicketResponseDto>();
        return Ok(response);
    }

    public class AssignTecnicoDto
    {
        public required string RutTecnico { get; set; }
    }
}
