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

        [HttpGet]                                                   // Lista todos los pasajes
        public async Task<ActionResult<List<Pasaje>>> Get() {
            var pasaje = await context.Pasaje.ToListAsync();
            return pasaje;
        }

        [HttpPost]                                                  // Registra un pasaje
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<string>> Post(Pasaje pasaje) {
            Guid uuid = Guid.NewGuid();
            pasaje.Uuid = uuid.ToString();
            pasaje.CreatedAt = DateTime.UtcNow;
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
