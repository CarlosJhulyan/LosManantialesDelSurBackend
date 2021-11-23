using LosManantialesDelSurBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LosManantialesDelSurBackend.Controllers {
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase {
        private readonly IConfiguration configuracion;
        private readonly DBManantialesContext context;

        public AuthController(DBManantialesContext context, IConfiguration config) {
            this.context = context;
            this.configuracion = config;
        }

        [HttpPost("cliente")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> SignCliente(Cliente cliente) {
            var clienteData = await autenticarCliente(cliente.Correo, cliente.Dni);
            if (clienteData is Cliente)
                return Ok(new { token = generarJWTCliente((Cliente)clienteData) });
            return clienteData;
        }

        [HttpGet("usuario")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> SignUsuario(Usuario usuario) {
            var usuarioData = await autenticarUsuario(usuario.Correo, usuario.Pass);
            if (usuarioData == null)
                return Unauthorized(new { message = "Este usuario no existe.", code = 401, usuarioData });
            return Ok(new { token = generarJWTUsuario((Usuario) usuarioData) });
        }

        private async Task<object> autenticarCliente(string correo, string dni) {
            var response = await context.Cliente.FirstOrDefaultAsync(x => x.Correo == correo);
            if (response == null)
                return NotFound(new { message = "Este correo no esta registrado." });
            else {
                var auth = await context.Cliente.FirstOrDefaultAsync(x => x.Correo == correo && x.Dni == dni);
                if (auth == null)
                    return Unauthorized(new { message = "DNI incorrecto." });
                return response;
            }
        }

        private async Task<object> autenticarUsuario(string correo, string pass) {
            var response = await context.Usuario.FirstOrDefaultAsync(x => x.Correo == correo && x.Pass == pass);
            return response;
        }

        // GENERAR WEB TOKEN DEL CLIENTE 
        private string generarJWTCliente(Cliente cliente) {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuracion["JWT:secretKey"]));
            var signInCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signInCredentials);

            var _Claims = new[] {
                new Claim("uuid", cliente.Uuid),
                new Claim("nombres", cliente.Nombres),
                new Claim("correo", cliente.Correo),
                new Claim("dni", cliente.Dni)
            };

            var payload = new JwtPayload(
                    issuer: configuracion["JWT:Issuer"],
                    audience: configuracion["JWT:Audience"],
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,                             // El  tiempo q se crea el token
                    expires: DateTime.UtcNow.AddHours(24)                   // La expiracion del token
                );
            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);         // Devuelve la respuesta del token
        }

        // GENERAR WEB TOKEN  USUARIO
        private string generarJWTUsuario(Usuario usuario) {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuracion["JWT:secretKey"]));
            var signInCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signInCredentials);

            var _Claims = new[] {
                new Claim("uuid", usuario.Uuid),
                new Claim("nombres", usuario.Nombres),
                new Claim("correo", usuario.Correo),
                new Claim("pass", usuario.Pass)
            };

            var payload = new JwtPayload(
                    issuer: configuracion["JWT:Issuer"],
                    audience: configuracion["JWT:Audience"],
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,                             // El  tiempo q se crea el token
                    expires: DateTime.UtcNow.AddHours(24)                   // La expiracion del token
                );
            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);         // Devuelve la respuesta del token
        }
    }
}
