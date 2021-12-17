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

        [HttpPost("signIn")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> SignIn(CredencialUser user) {
            var userData = await autenticarUser(user);
            if (userData is Usuario)
                return Ok(new { token = generarJWT((Usuario) userData), data = userData, statusCode = 200 });
            return userData;
        }

        [HttpPost("signUp")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> SignUp(Usuario user) {
            var correo = await context.Usuario.FirstOrDefaultAsync(x => x.Correo == user.Correo);
            var dni = await context.Usuario.FirstOrDefaultAsync(x => x.Dni == user.Dni);
            var celular = await context.Usuario.FirstOrDefaultAsync(x => x.Celular == user.Celular);
            if (correo != null)
                return BadRequest(new { message = "Este correo ya esta registrado.", statusCode = 400 });
            else if (dni != null)
                return BadRequest(new { message = "Este número de DNI ya existe.", statusCode = 400 });
            else if (celular != null)
                return BadRequest(new { message = "Este número de celular ya existe.", statusCode = 400 });
            else {
                Guid uuid = Guid.NewGuid();
                user.Uuid = uuid.ToString();
                user.CreatedAt = DateTime.UtcNow;
                user.Pass = BCrypt.Net.BCrypt.HashPassword(user.Pass);
                context.Usuario.Add(user);
                await context.SaveChangesAsync();
                return Ok(new { message = "Usuario registrado correctamente.", data = user, statusCode = 201, token = generarJWT(user) });
            }
        }

        private async Task<object> autenticarUser(CredencialUser user) {
            var response = await context.Usuario.FirstOrDefaultAsync(x => x.Correo == user.Correo);
            if (response == null)
                return NotFound(new { message = "Este correo no esta registrado.", statusCode = 404 });
            else if (!BCrypt.Net.BCrypt.Verify(user.Password, response.Pass))
                return BadRequest(new { message = "Contraseña incorrecta.", StatusCode = 404 });
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
