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

        [HttpPost()]
        [AllowAnonymous]
        public async Task<ActionResult<object>> SignIn(CredencialUser user) {
            var userData = await autenticarUser(user);
            if (userData is Usuario)
                return Ok(new { token = generarJWT((Usuario) userData), data = userData });
            return userData;
        }

        private async Task<object> autenticarUser(CredencialUser user) {
            var response = await context.Usuario.FirstOrDefaultAsync(x => x.Correo == user.Correo);
            if (response == null)
                return NotFound(new { message = "Este correo no esta registrado." });
            else if (response.Pass != user.Password)
                return BadRequest(new { message = "Contraseña incorrecta." });
            else
                return response;
        }

        // GENERAR WEB TOKEN DEL USUARIO
        private RespuestaAutenticacion generarJWT(Usuario user) {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuracion["JWT:secretKey"]));
            var signInCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signInCredentials);

            var _Claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Uuid),
                new Claim(ClaimTypes.Name, user.Nombres),
                new Claim(ClaimTypes.Email, user.Correo),
                new Claim(ClaimTypes.SerialNumber, user.Dni),
                new Claim(ClaimTypes.Role, user.Rol)
            };

            var expiracion = DateTime.UtcNow.AddHours(24);

            var payload = new JwtPayload(
                    issuer: null,
                    audience: null,
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,                             // El  momento que se crea el token
                    expires: expiracion                                     // La expiracion del token
                );
            var token = new JwtSecurityToken(header, payload);

            return new RespuestaAutenticacion() {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = expiracion
            };                                                              // Devuelve la respuesta del token
        }
    }
}
