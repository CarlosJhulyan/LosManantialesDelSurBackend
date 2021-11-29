using LosManantialesDelSurBackend.Dto;
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
        public async Task<ActionResult<object>> SignCliente(CredencialCliente cliente) {
            var clienteData = await autenticarCliente(cliente);
            if (clienteData is Cliente)
                return Ok(new { token = generarJWTCliente((Cliente) clienteData), data = clienteData });
            return clienteData;
        }

        private async Task<object> autenticarCliente(CredencialCliente cliente) {
            var response = await context.Cliente.FirstOrDefaultAsync(x => x.Correo == cliente.Correo);
            if (response == null)
                return NotFound(new { message = "Este correo no esta registrado." });
            else if (response.Dni != cliente.Dni)
                return Unauthorized(new { message = "DNI incorrecto." });
            else
                return response;
        }

        // GENERAR WEB TOKEN DEL CLIENTE 
        private RespuestaAutenticacion generarJWTCliente(Cliente cliente) {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuracion["JWT:secretKey"]));
            var signInCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signInCredentials);

            var _Claims = new[] {
                new Claim("uuid", cliente.Uuid),
                new Claim("nombres", cliente.Nombres),
                new Claim("correo", cliente.Correo),
                new Claim("dni", cliente.Dni)
            };

            var expiracion = DateTime.UtcNow.AddHours(24);

            var payload = new JwtPayload(
                    issuer: null,
                    audience: null,
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,                             // El  tiempo q se crea el token
                    expires: expiracion                                     // La expiracion del token
                );
            var token = new JwtSecurityToken(header, payload);

            return new RespuestaAutenticacion() {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };                                                              // Devuelve la respuesta del token
        }
    }
}
