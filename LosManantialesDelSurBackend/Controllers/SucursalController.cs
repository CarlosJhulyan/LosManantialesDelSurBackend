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

    public class SucursalController : ControllerBase {
        private readonly DBManantialesContext context;

        public SucursalController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]                                       // Lista los sucursales
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Sucursal>>> Get() {
            var sucursal = await context.Sucursal.ToListAsync();
            return sucursal;
        }

        [HttpPost]                                      // Añadir Sucursales
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Post(Sucursal sucursal) {
            var currentUser = getCurrentUser();
            if (currentUser.Rol != "administrador")
                return Unauthorized(new { message = "No estas autorizado para realizar esta acción.", statusCode = 401 });
            context.Sucursal.Add(sucursal);
            await context.SaveChangesAsync();
            return Ok(new { message = "Sucursal generado correctamente.", statusCode = 201, data = sucursal });
        }

        [HttpDelete]                                    // Elimina la sucursal
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Delete(int id) {
            var currentUser = getCurrentUser();
            if (currentUser.Rol != "administrador")
                return Unauthorized(new { message = "No estas autorizado para realizar esta acción.", statusCode = 401 });
            var sucursal = await context.Sucursal.FindAsync(id);
            context.Sucursal.Remove(sucursal);
            await context.SaveChangesAsync();
            return Ok(new { message = "Sucursal eliminada correctamente.", statusCode = 200 });
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
