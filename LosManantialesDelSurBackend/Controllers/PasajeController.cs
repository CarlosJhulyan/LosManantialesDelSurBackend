using LosManantialesDelSurBackend.Models;
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

        [HttpGet]
        public async Task<ActionResult<List<Pasaje>>> Get() {
            var pasaje = await context.Pasaje.ToListAsync();
            return pasaje;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post(Pasaje pasaje) {
            pasaje.CreatedAt = DateTime.UtcNow;
            context.Pasaje.Add(pasaje);
            await context.SaveChangesAsync();
            return pasaje.Uuid;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(Pasaje pasaje) {
            context.Pasaje.Update(pasaje);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<int>> Delete(int id) {
            var pasaje = await context.Pasaje.FindAsync(id);
            context.Pasaje.Remove(pasaje);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
