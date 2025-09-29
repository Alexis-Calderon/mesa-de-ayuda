using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using MesaDeAyuda.Data.Dtos.Usuario;
using MesaDeAyuda.Data.Interfaces.UseCases;
using MesaDeAyuda.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MesaDeAyuda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class UsuariosController(IUsuarioUseCases usuarioUseCases) : ControllerBase
{
    private readonly IUsuarioUseCases _usuarioUseCases = usuarioUseCases;

    [HttpGet]
    public async Task<IActionResult> GetUsuarios()
    {
        var usuarios = await _usuarioUseCases.GetAllUsuariosAsync();
        var response = usuarios.Adapt<List<UsuarioResponseDto>>();
        return Ok(response);
    }

    [HttpGet("{rut}")]
    public async Task<IActionResult> GetUsuario(string rut)
    {
        var usuario = await _usuarioUseCases.GetUsuarioByRutAsync(rut);
        if (usuario == null)
            return NotFound("Usuario no encontrado");

        var response = usuario.Adapt<UsuarioResponseDto>();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUsuario([FromBody] UsuarioCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Verificar si RUT ya existe
        var existing = await _usuarioUseCases.GetUsuarioByRutAsync(dto.Rut);
        if (existing != null)
            return BadRequest("El RUT ya está registrado");

        var usuario = dto.Adapt<Usuario>();
        usuario.Contrasenia = BCrypt.Net.BCrypt.HashPassword(dto.Contrasenia);

        var created = await _usuarioUseCases.CreateUsuarioAsync(usuario);
        var response = created.Adapt<UsuarioResponseDto>();

        return CreatedAtAction(nameof(GetUsuario), new { rut = response.Rut }, response);
    }

    [HttpPut("{rut}")]
    public async Task<IActionResult> UpdateUsuario(string rut, [FromBody] UsuarioUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var usuario = dto.Adapt<Usuario>();
        var updated = await _usuarioUseCases.UpdateUsuarioAsync(rut, usuario);

        if (updated == null)
            return NotFound("Usuario no encontrado");

        var response = updated.Adapt<UsuarioResponseDto>();
        return Ok(response);
    }

    [HttpDelete("{rut}")]
    public async Task<IActionResult> DeleteUsuario(string rut)
    {
        var deleted = await _usuarioUseCases.DeleteUsuarioAsync(rut);
        if (!deleted)
            return NotFound("Usuario no encontrado");

        return NoContent();
    }
}
