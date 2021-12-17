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

    public class VehiculoController : ControllerBase {
        private readonly DBManantialesContext context;

        public VehiculoController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]                                             // Lista los vehiculos por destino y origen
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Vehiculo>>> Get(int origen, int destino) {
            var vehiculo = await context.Vehiculo.Where(x => x.SucursalActual == origen && x.SucursalFinal == destino && x.Estado == true).ToListAsync();
            return vehiculo;
        }

        [HttpGet("asientos/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<object>> GetAsientosDisponibles(int id) {
            // TODO: Verificar si el vehiculo existe o no
            var vehiculo = await context.Vehiculo.FirstOrDefaultAsync(x => x.Id == id);
            if (vehiculo == null)
                return NotFound(new { message = "Este vehiculo no esta registrado.", statusCode = 404 });
            var pasajes = await context.Pasaje.Where(x => x.VehiculoPasaje == id).ToListAsync();
            int[] asientos = new int[14];
            foreach (var pasaje in pasajes)
                asientos[(int) pasaje.NumeroAsiento - 1] = 1;
            return new { asientos };
        }

        [HttpPost]                                              //Registra un vehiculo
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Post(Vehiculo vehiculo) {
            var currentUser = getCurrentUser();
            if (currentUser.Rol != "administrador")
                return Unauthorized(new { message = "No estas autorizado para realizar esta acción.", statusCode = 401 });
            vehiculo.CreatedAt = DateTime.UtcNow;
            context.Vehiculo.Add(vehiculo);
            await context.SaveChangesAsync();
            return Ok(new { message = "Vehiculo registrado correctamente.", statusCode = 201, data = vehiculo });
        }

        [HttpPut]                                               //Actualiza los datos del Vehiculo
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Update(Vehiculo vehiculo) {
            var search = await context.Vehiculo.FirstOrDefaultAsync(x => x.Id == vehiculo.Id);
            search.SucursalActual = vehiculo.SucursalActual;
            search.SucursalFinal= vehiculo.SucursalFinal;
            vehiculo = search;
            context.Vehiculo.Update(vehiculo);
            await context.SaveChangesAsync();
            return Ok(new { message = "Vehiculo actualizado correctamente", statusCode = 200 });
        }

        [HttpDelete]                                            // Elimina un vehiculos
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Delete(int id) {
            var currentUser = getCurrentUser();
            if (currentUser.Rol != "administrador")
                return Unauthorized(new { message = "No estas autorizado para realizar esta acción.", statusCode = 401 });
            var vehiculo = await context.Vehiculo.FindAsync(id);
            context.Vehiculo.Remove(vehiculo);
            await context.SaveChangesAsync();
            return Ok(new { message = "Vehiculo eliminado correctamente", statusCode = 200 });
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
