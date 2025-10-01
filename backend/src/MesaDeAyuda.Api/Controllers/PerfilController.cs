using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Mapster;
using MesaDeAyuda.Data.Common.Helpers;
using MesaDeAyuda.Data.Dtos.Usuario;
using MesaDeAyuda.Data.Interfaces.UseCases;
using MesaDeAyuda.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MesaDeAyuda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Cualquier usuario autenticado puede acceder a su propio perfil
public class PerfilController(IUsuarioUseCases usuarioUseCases) : ControllerBase
{
    private readonly IUsuarioUseCases _usuarioUseCases = usuarioUseCases;

    /// <summary>
    /// Obtiene el perfil del usuario actualmente autenticado
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPerfil()
    {
        // Intentar obtener el RUT del token JWT
        var rut =
            User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? User.FindFirst("rut")?.Value;

        if (string.IsNullOrEmpty(rut))
            return Unauthorized("No se pudo obtener el RUT del usuario autenticado");

        var usuario = await _usuarioUseCases.GetCurrentUserAsync(rut);
        if (usuario == null)
            return NotFound("Usuario no encontrado");

        var response = usuario.Adapt<UsuarioResponseDto>();
        return Ok(response);
    }

    /// <summary>
    /// Actualiza el perfil del usuario actualmente autenticado
    /// Solo permite cambiar nombre, email y contraseña (no RUT ni rol)
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdatePerfil([FromBody] UsuarioPerfilUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Intentar obtener el RUT del token JWT
        var rut =
            User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? User.FindFirst("rut")?.Value;

        if (string.IsNullOrEmpty(rut))
            return Unauthorized("No se pudo obtener el RUT del usuario autenticado");

        // Sin restricciones especiales para el administrador - puede cambiar email libremente

        try
        {
            var updated = await _usuarioUseCases.UpdatePerfilAsync(rut, dto);

            if (updated == null)
                return NotFound("Usuario no encontrado");

            var response = updated.Adapt<UsuarioResponseDto>();
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}