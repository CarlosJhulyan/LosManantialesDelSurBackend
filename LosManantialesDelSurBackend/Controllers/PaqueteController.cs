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
    public class PaqueteController : ControllerBase {
        private readonly DBManantialesContext context;

        public PaqueteController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]                                               // Listar todos los paquetes
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Paquete>>> Get() {
            var paquetes = await context.Paquete.OrderByDescending(x => x.CreatedAt).ToListAsync();
            return paquetes;
        }

        [HttpGet("{uuid}")]                                     // Listar todos los paquetes de un cliente
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Paquete>>> Get(string uuid) {
            var paquete = await context.Paquete.Where(x => x.Remitente == uuid).OrderByDescending(x => x.CreatedAt).ToListAsync();
            return paquete;
        }

        [HttpPost]                                              // Generar registro de paqueteria
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Post(Paquete paquete) {
            paquete.CreatedAt = DateTime.UtcNow;
            context.Paquete.Add(paquete);
            await context.SaveChangesAsync();
            return Ok(new { message = "Paquete registrado correctamente.", data = paquete, statusCode = 201 });
        }

        [HttpDelete]                                            // Elimina un registro de paquete
        public async Task<ActionResult<int>> Delete(int id) {
            var paquete = await context.Paquete.FindAsync(id);
            context.Paquete.Remove(paquete);
            await context.SaveChangesAsync();
            return Ok(new { message = "Paquete eliminado correctamente.", statusCode = 200 });
        }
    }
}
