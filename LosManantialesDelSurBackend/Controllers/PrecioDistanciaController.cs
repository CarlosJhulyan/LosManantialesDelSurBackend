using LosManantialesDelSurBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LosManantialesDelSurBackend.Controllers {
    [Route("api/precio-distancia")]
    [ApiController]

    public class PrecioDistanciaController : ControllerBase {
        private readonly DBManantialesContext context;

        public PrecioDistanciaController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<PrecioDistancia>>> Get() {
            var precio= await context.PrecioDistancia.ToListAsync();
            return precio;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(PrecioDistancia precio) {
            context.PrecioDistancia.Add(precio);
            await context.SaveChangesAsync();
            return precio.Id;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(PrecioDistancia precio) {
            context.PrecioDistancia.Update(precio);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<int>> Delete(int id) {
            var precio= await context.PrecioDistancia.FindAsync(id);
            context.PrecioDistancia.Remove(precio);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
