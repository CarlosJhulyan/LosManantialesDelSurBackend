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

    public class SeguimientoController : ControllerBase {
        private readonly DBManantialesContext context;

        public SeguimientoController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Seguimiento>>> Get() {
            var seguimiento = await context.Seguimiento.ToListAsync();
            return seguimiento;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post(Seguimiento seguimiento) {
            seguimiento.CreatedAt = DateTime.UtcNow;
            context.Seguimiento.Add(seguimiento);
            await context.SaveChangesAsync();
            return seguimiento.Uuid;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(Seguimiento seguimiento) {
            context.Seguimiento.Update(seguimiento);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<int>> Delete(int id) {
            var seguimiento = await context.Seguimiento.FindAsync(id);
            context.Seguimiento.Remove(seguimiento);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
