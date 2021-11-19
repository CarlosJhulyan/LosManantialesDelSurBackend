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

    public class DestinatarioController : ControllerBase {
        private readonly DBManantialesContext context;

        public DestinatarioController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Destinatario>>> Get() {
            var destinatario = await context.Destinatario.ToListAsync();
            return destinatario;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post(Destinatario destinatario) {
            destinatario.CreatedAt = DateTime.UtcNow;
            context.Destinatario.Add(destinatario);
            await context.SaveChangesAsync();
            return destinatario.Uuid;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(Destinatario destinatario) {
            context.Destinatario.Update(destinatario);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<int>> Delete(int id) {
            var destinatario = await context.Destinatario.FindAsync(id);
            context.Destinatario.Remove(destinatario);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
