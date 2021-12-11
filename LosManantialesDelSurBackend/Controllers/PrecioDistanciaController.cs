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
    [Route("api/precio-distancia")]
    [ApiController]

    public class PrecioDistanciaController : ControllerBase {
        private readonly DBManantialesContext context;

        public PrecioDistanciaController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]                                   // Traer los precios por distancia
        public async Task<ActionResult<List<PrecioDistancia>>> Get() {
            var precio= await context.PrecioDistancia.ToListAsync();
            return precio;
        }

        [HttpPost]                                  // Generar precio por distancia
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Post(PrecioDistancia precio) {
            var currentUser = getCurrentUser();
            if (currentUser.Rol != "administrador")
                return Unauthorized(new { message = "No estas autorizado para realizar esta acción.", statudCode = 401 });
            context.PrecioDistancia.Add(precio);
            await context.SaveChangesAsync();
            return Ok(new { message = "Precios generados correctamente.", statusCode = 201 });
        }

        [HttpPut]                                   // Actualizar precio por distancia
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Update(PrecioDistancia precio) {
            var currentUser = getCurrentUser();
            if (currentUser.Rol != "administrador")
                return Unauthorized(new { message = "No estas autorizado para realizar esta acción.", statusCode = 401 });
            context.PrecioDistancia.Update(precio);
            await context.SaveChangesAsync();
            return Ok(new { message = "Precio actualizado correctamente.", statusCode = 200 });
        }

        [HttpDelete]                                // Eliminar precio por distancia
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Delete(int id) {
            var currentUser = getCurrentUser();
            if (currentUser.Rol != "administrador")
                return Unauthorized(new { message = "No estas autorizado para realizar esta acción.", statusCode = 401 });
            var precio= await context.PrecioDistancia.FindAsync(id);
            context.PrecioDistancia.Remove(precio);
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
