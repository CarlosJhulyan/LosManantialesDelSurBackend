using LosManantialesDelSurBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LosManantialesDelSurBackend.Controllers {
    [Route("api/codigo")]
    [ApiController]

    public class CodigoValidacionController : ControllerBase {
        private readonly DBManantialesContext context;

        public CodigoValidacionController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<CodigoValidacionPago>>> Get() {
            var codigo = await context.CodigoValidacionPago.ToListAsync();
            return codigo;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CodigoValidacionPago codigo) {
            codigo.Usado = false;
            context.CodigoValidacionPago.Add(codigo);
            await context.SaveChangesAsync();
            return codigo.Id;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(Vehiculo vehiculo) {
            context.Vehiculo.Update(vehiculo);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<int>> Delete(int id) {
            var codigo = await context.CodigoValidacionPago.FindAsync(id);
            context.CodigoValidacionPago.Remove(codigo);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
