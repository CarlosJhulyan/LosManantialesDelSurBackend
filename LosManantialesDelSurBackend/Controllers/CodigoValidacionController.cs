using LosManantialesDelSurBackend.Dto;
using LosManantialesDelSurBackend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LosManantialesDelSurBackend.Controllers {
    [Route("api/codigo")]
    [ApiController]

    public class CodigoValidacionController : ControllerBase {
        private readonly DBManantialesContext context;

        public CodigoValidacionController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]                                                   // Lista todos los codigos
        public async Task<ActionResult<List<CodigoValidacionPago>>> Get() {
            var codigo = await context.CodigoValidacionPago.ToListAsync();
            return codigo;
        }

        [HttpPost]                                                  // Genera un codigo
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<object>> Post(CodigoValidacionPago codigo) {
            var currentUser = getCurrentUser();
            if (currentUser.Rol != "coder")
                return Unauthorized(new { message = "No estas autorizado para realizar esta acción.", statusCode = 401 });
            codigo.Usado = false;
            context.CodigoValidacionPago.Add(codigo);
            await context.SaveChangesAsync();
            return new { message = "Código generado correctamente", statusCode = 201, data = codigo };
        }

        [HttpPut]                                                   // Actualizar el código
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CodigoValidacionPago>> Update(string codigo) {
            var code = await context.CodigoValidacionPago.Where(x => x.Codigo == codigo).FirstOrDefaultAsync();
            if (code == null) {
                return NotFound(new { message = "Este código no esta registrado.", statusCode = 404 });
            } else if (code.Usado == true) {
                return NotFound(new { message = "Este código ya fue utilizado", statusCode = 404 });
            }
            code.Usado = true;
            context.CodigoValidacionPago.Update(code);
            await context.SaveChangesAsync();
            return Ok(new { message = "Código actualizado correctamente", statusCode = 200 });
        }

        [HttpDelete]                                                // Elimina un código
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Delete(int id) {
            var currentUser = getCurrentUser();
            if (currentUser.Rol != "coder")
                return Unauthorized(new { message = "No estas autorizado para realizar esta acción.", statusCode = 401 });
            var codigo = await context.CodigoValidacionPago.FindAsync(id);
            context.CodigoValidacionPago.Remove(codigo);
            await context.SaveChangesAsync();
            return Ok(new { message = "Código eliminado correctamente.", statusCode = 200 });
        }

        private CredencialUser getCurrentUser() {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null) {
                var userClaims = identity.Claims;

                return new CredencialUser {
                    Dni = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value,
                    Correo = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    Nombres = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                    Uuid = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Rol = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }
    }
}
