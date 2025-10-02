using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MesaDeAyuda.Data.Dtos.Contracts;
using MesaDeAyuda.Data.Interfaces.UseCases;
using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MesaDeAyuda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioUseCases _usuarioUseCases;
    private readonly IConfiguration _configuration;

    public AuthController(IUsuarioUseCases usuarioUseCases, IConfiguration configuration)
    {
        _usuarioUseCases = usuarioUseCases;
        _configuration = configuration;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Buscar usuario por RUT
        var usuario = await _usuarioUseCases.GetUsuarioByRutAsync(loginDto.Rut);
        if (usuario == null || string.IsNullOrEmpty(usuario.Contrasenia))
            return Unauthorized("Credenciales inválidas");

        // Verificar contraseña
        if (!BCrypt.Net.BCrypt.Verify(loginDto.Contrasenia, usuario.Contrasenia))
            return Unauthorized("Credenciales inválidas");

        // Generar token
        var token = GenerateJwtToken(usuario);

        return Ok(
            new
            {
                Token = token,
                Usuario = new
                {
                    usuario.Rut,
                    usuario.Nombre,
                    usuario.Email,
                    usuario.Rol,
                },
            }
        );
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        // En una implementación real, podrías invalidar el token en una blacklist
        // Por simplicidad, solo devolvemos OK
        return Ok("Sesión cerrada exitosamente");
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expiryInMinutes = int.Parse(jwtSettings["ExpiryInMinutes"]!);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Rut),
            new Claim(JwtRegisteredClaimNames.Name, usuario.Nombre),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol.ToString()),
            new Claim("rut", usuario.Rut),
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256
        );
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
