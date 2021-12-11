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
    [Route("api/[controller]")]
    [ApiController]

    public class SeguimientoController : ControllerBase {
        private readonly DBManantialesContext context;

        public SeguimientoController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]                                   // Traer los seguimientos
        public async Task<ActionResult<List<Seguimiento>>> Get() {
            var seguimiento = await context.Seguimiento.OrderByDescending(x => x.CreatedAt).ToListAsync();
            return seguimiento;
        }

        [HttpGet("{uuid}")]                         // Traer un seguimiento
        public async Task<ActionResult<Seguimiento>> Get(string uuid) {
            var seguimiento = await context.Seguimiento.FirstOrDefaultAsync(x => x.Uuid == uuid);
            return seguimiento;
        }

        [HttpPost]                                  // Generar seguimiento
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<string>> Post(Seguimiento seguimiento) {
            Guid uuid = Guid.NewGuid();
            seguimiento.Uuid = uuid.ToString();
            seguimiento.CreatedAt = DateTime.UtcNow;
            seguimiento.NumeroSeguimiento = generarCodigo();
            context.Seguimiento.Add(seguimiento);
            await context.SaveChangesAsync();
            return Ok(new { message = "Seguimiento generado", data = seguimiento, statusCode = 201 });
        }

        [HttpPut]                                   // Actualizar el seguimiento
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Update(Seguimiento seguimiento) {
            var currentUser = getCurrentUser();
            if (currentUser.Rol == "cliente" || currentUser.Rol == "editor") {
                context.Seguimiento.Update(seguimiento);
                await context.SaveChangesAsync();
                return Ok(new { message = "Seguimiento Actualizado", statusCode = 200 });
            }
            return Unauthorized(new { message = "No estas autorizado para realizar esta acción.", statusCode = 401 });
        }

        [HttpDelete]                                // Eliminar el seguimiento
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Delete(int id) {
            var seguimiento = await context.Seguimiento.FindAsync(id);
            context.Seguimiento.Remove(seguimiento);
            await context.SaveChangesAsync();
            return Ok(new { message = "Seguimiento eliminado correctamente.", statusCode = 200 });
        }

        private string generarCodigo() {
            Guid uuid = Guid.NewGuid();
            var uuidString = uuid.ToString();
            var codigoArray = uuidString.Split('-');
            string codigo = string.Join("", codigoArray.Skip(1));
            return codigo.Substring(0, 15).ToUpper();
        }

        /*================ ESTADOS SEGUIMIENTO ==================*/

        [HttpGet("estados/{uuid}")]                             // Traer todos los estados por seguimiento
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<EstadoSeguimiento>>> GetStatus(string uuid) {
            var estado = await context.EstadoSeguimiento.Where(x => x.Seguimiento == uuid).OrderByDescending(x => x.Fecha).ToListAsync();
            return estado;
        }

        [HttpPost("estados")]                                  // Generar el estado del seguimiento
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<object>> PostStatus(EstadoSeguimiento estado) {
            var currentUser = getCurrentUser();
            if (currentUser.Rol == "cliente" || currentUser.Rol == "editor") {
                context.EstadoSeguimiento.Add(estado);
                await context.SaveChangesAsync();
                return new { message = "Estado del seguimiento actualizado", id = estado.Id, statusCode = 200 };
            }
            return Unauthorized(new { message = "No estas autorizado para realizar esta acción.", statusCode = 401 });
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
