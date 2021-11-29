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
    public class PaqueteController : ControllerBase {
        private readonly DBManantialesContext context;

        public PaqueteController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Paquete>>> Get() {
            var paquete = await context.Paquete.OrderByDescending(x => x.CreatedAt).ToListAsync();
            return paquete;
        }

        [HttpGet("{uuid}")]
        public async Task<ActionResult<List<Paquete>>> Get(string uuid) {
            var paquete = await context.Paquete.Where(x => x.Remitente == uuid).OrderByDescending(x => x.CreatedAt).ToListAsync();
            return paquete;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Paquete paquete) {
            paquete.CreatedAt = DateTime.UtcNow;
            context.Paquete.Add(paquete);
            await context.SaveChangesAsync();
            return paquete.Id;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(Paquete paquete) {
            context.Paquete.Update(paquete);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<int>> Delete(int id) {
            var paquete = await context.Paquete.FindAsync(id);
            context.Paquete.Remove(paquete);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
