using LosManantialesDelSurBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LosManantialesDelSurBackend.Controllers {
    [Route("api/precio-asiento")]
    [ApiController]

    public class PrecioAsientoController : ControllerBase {
        private readonly DBManantialesContext context;

        public PrecioAsientoController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<PrecioAsiento>>> Get() {
            var precio = await context.PrecioAsiento.ToListAsync();
            return precio;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(PrecioAsiento precio) {
            context.PrecioAsiento.Add(precio);
            await context.SaveChangesAsync();
            return precio.Id;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(PrecioAsiento precio) {
            context.PrecioAsiento.Update(precio);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<int>> Delete(int id) {
            var precio = await context.PrecioAsiento.FindAsync(id);
            context.PrecioAsiento.Remove(precio);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
