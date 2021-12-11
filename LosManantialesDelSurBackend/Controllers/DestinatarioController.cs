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

    public class DestinatarioController : ControllerBase {
        private readonly DBManantialesContext context;

        public DestinatarioController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]                                                           // Trae todos los destinatarios en una lista
        public async Task<ActionResult<List<Destinatario>>> Get() {
            var destinatario = await context.Destinatario.ToListAsync();
            return destinatario;
        }

        [HttpPost]                                                          // Crear un destinatario
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<string>> Post(Destinatario destinatario) {
            Guid uuid = Guid.NewGuid();
            destinatario.Uuid = uuid.ToString();
            destinatario.CreatedAt = DateTime.UtcNow;
            context.Destinatario.Add(destinatario);
            await context.SaveChangesAsync();
            return Ok(new { message = "Destinatario generado", data = destinatario, statusCode = 201 });
        }

        [HttpDelete]                                                        // Elimina un destinatario
        public async Task<ActionResult<int>> Delete(int id) {
            var destinatario = await context.Destinatario.FindAsync(id);
            context.Destinatario.Remove(destinatario);
            await context.SaveChangesAsync();
            return Ok(new { message = "Destinatario eliminado correctamente.", statusCode = 200 });
        }
    }
}
