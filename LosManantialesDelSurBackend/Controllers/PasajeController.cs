using LosManantialesDelSurBackend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LosManantialesDelSurBackend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PasajeController : ControllerBase {
        private readonly DBManantialesContext context;

        public PasajeController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]                                                   // Lista Todos los pasajes por su vehiculo id
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Pasaje>>> Get(int id) {
            var pasaje = await context.Pasaje.Where(x => x.VehiculoPasaje == id).ToListAsync();
            return pasaje;
        }

        [HttpPut]                                               //Actualiza el campo vehiculoPasaje para que se elimine del vehiculo
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Update(Pasaje pasaje) {
            var search = await context.Pasaje.FirstOrDefaultAsync(x => x.Uuid == pasaje.Uuid);
            search.VehiculoPasaje = null;
            pasaje = search;
            context.Pasaje.Update(pasaje);
            await context.SaveChangesAsync();
            return Ok(new { message = "Pasaje actualizado correctamente", statusCode = 200 });
        }

        [HttpPost]                                                  // Registra un pasaje
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<string>> Post(Pasaje pasaje) {
            Guid uuid = Guid.NewGuid();
            pasaje.Uuid = uuid.ToString();
            pasaje.CreatedAt = DateTime.UtcNow;
            pasaje.VehiculoPasajeStatic = pasaje.VehiculoPasaje;
            context.Pasaje.Add(pasaje);
            await context.SaveChangesAsync();
            return Ok(new { message = "Pasaje registrado correctamente.", statusCode = 201, data = pasaje });
        }

        [HttpDelete]                                                    // Elimina un pasaje registrado
        public async Task<ActionResult<int>> Delete(int id) {
            var pasaje = await context.Pasaje.FindAsync(id);
            context.Pasaje.Remove(pasaje);
            await context.SaveChangesAsync();
            return Ok(new { message = "Registro de pasaje eliminado correctamente.", statusCode = 200 });
        }
    }
}
