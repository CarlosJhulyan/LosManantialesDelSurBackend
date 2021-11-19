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

    public class VehiculoController : ControllerBase {
        private readonly DBManantialesContext context;

        public VehiculoController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Vehiculo>>> Get() {
            var vehiculo = await context.Vehiculo.ToListAsync();
            return vehiculo;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Vehiculo vehiculo) {
            vehiculo.CreatedAt = DateTime.UtcNow;
            context.Vehiculo.Add(vehiculo);
            await context.SaveChangesAsync();
            return vehiculo.Id;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(Vehiculo vehiculo) {
            context.Vehiculo.Update(vehiculo);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<int>> Delete(int id) {
            var vehiculo = await context.Vehiculo.FindAsync(id);
            context.Vehiculo.Remove(vehiculo);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
