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
    [Route("api/precio-asiento")]
    [ApiController]

    public class PrecioAsientoController : ControllerBase {
        private readonly DBManantialesContext context;

        public PrecioAsientoController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]                                       // Listar los precios de asiento
        public async Task<ActionResult<List<PrecioAsiento>>> Get() {
            var precio = await context.PrecioAsiento.OrderBy(x => x.NumeroAsiento).ToListAsync();
            return precio;
        }

        [HttpPost]                                      // Generar los precios del asiento
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Post(PrecioAsiento precio) {
            var currentUser = getCurrentUser();
            if (currentUser.Rol != "administrador")
                return Unauthorized(new { message = "No estas autorizado para realizar esta acción.", statusCode = 401 });
            context.PrecioAsiento.Add(precio);
            await context.SaveChangesAsync();
            return Ok(new { message = "Precio de asiento generado correctamente.", statusCode = 201, data = precio });
        }

        [HttpPut]                                       // Actualizar los precios del asiento
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Update(PrecioAsiento precio) {
            var currentUser = getCurrentUser();
            if (currentUser.Rol != "administrador")
                return Unauthorized(new { message = "No estas autorizado para realizar esta acción.", statusCode = 401 });
            context.PrecioAsiento.Update(precio);
            await context.SaveChangesAsync();
            return Ok(new { message = "Asiento " + precio.NumeroAsiento + " actualizado correctamente", statusCode = 200 });
        }

        [HttpDelete]                                    // Eliminar los precios del asiento
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Delete(int id) {
            var currentUser = getCurrentUser();
            if (currentUser.Rol != "administrador")
                return Unauthorized(new { message = "No estas autorizado para realizar esta acción.", statusCode = 401 });
            var precio = await context.PrecioAsiento.FindAsync(id);
            context.PrecioAsiento.Remove(precio);
            await context.SaveChangesAsync();
            return Ok(new { message = "Eliminado correctamente.", statusCode = 200 });
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
